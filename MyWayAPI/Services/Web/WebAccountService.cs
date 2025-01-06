using MailKit;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyWayAPI.Services
{
    public interface IWebAccountService
    {
        public int? Login(LoginModel model);
        public bool Register(WebRegisterModel model);
        public string GenerateToken(int? userId);
        public bool ChangeAccess(int userId, int accessLevel);
        public List<WebUser> GetWebUsers(int  userId);
        public List<AppUser> GetAppUsers(int userId);
    }
    public class WebAccountService: IWebAccountService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        private IMailService mailService;
        public WebAccountService(IConfiguration config, MWDbContext dbContext, IMailService mailService)
        {
            this.config = config;
            this.dbContext = dbContext;
            this.mailService = mailService;
        }
        public int? Login(LoginModel model)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.EmailAddress.Equals(model.emailAddress));
            if (user != null && user.AccessLevel > 0)
            {
                if (model.password.Equals(user.Password))
                {
                    return user.Id;
                }
                else return null;
            }
            else return null;

        }

        public bool Register(WebRegisterModel model)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.EmailAddress.Equals(model.emailAddress));
            var crator = dbContext.WebUsers.FirstOrDefault(u => u.Id == model.CreatorId);
            if (user != null)
            {
                return false;
            }
            else
            {
                var newUser = new WebUser
                {
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    Company = crator.Company
                };
                newUser.SetEmailAddress(model.emailAddress);
                newUser.SetPassword(new PasswordBuilder().CreatePassword());
                newUser.SetAccessLevel(1); //0 - niekatywny, 1 - zwykły użytkownik, 2 - administrator
                dbContext.WebUsers.Add(newUser);
                mailService.SendEmail(newUser.EmailAddress, "Rejestracja w systemie",
                $"Witaj,{newUser.FirstName} Twój login to: {newUser.EmailAddress} \n, a wygenerowane hasło to: {newUser.Password}. \n Pamiętaj aby zmienić hasło jak najszybciej to możliwe!");
                dbContext.SaveChanges();
                return true;
            }
        }
        public string GenerateToken(int? userId)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.Id == userId);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            var claim = new Claim("sub", userId.ToString());
            var accessClaim = new Claim("access", user.AccessLevel.ToString());
            claims.Add(accessClaim);

            var Sectoken = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        public bool ChangeAccess(int userId, int accessLevel)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.Id == userId);
            if(user is null)
            {
                return false;
            }
            else
            {
                user.SetAccessLevel(accessLevel);
                dbContext.SaveChanges();
                return true;
            }
        }

        public List<WebUser> GetWebUsers(int userId)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.Id == userId);
            var users = dbContext.WebUsers.Where(u => u.Company == user.Company).ToList();
            return users;
        }
        public List<AppUser> GetAppUsers(int userId)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.Id == userId);
            var userCompany = user.Company;
            var users = dbContext.AppUsers.ToList();
            var list = new List<AppUser>();
            foreach(var u in users)
            {
                foreach(var c in u.Companies)
                {
                    if(c == userCompany)
                    {
                        list.Add(u);
                    }                 
                }
            }
            return list;
        }
    }
}
