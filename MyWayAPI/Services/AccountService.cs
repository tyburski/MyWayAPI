using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI.Models;
using MyWayAPI.Models.Body;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWayAPI.Services
{
    public interface IAccountService
    {
        public int? Login(string username, string password);
        public bool Register(RegisterModel model);
        public string GenerateToken(int? userId);
    }
    public class AccountService : IAccountService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public AccountService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public int? Login(string username, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress.Equals(username));
            if (user != null)
            {
                if (password.Equals(user.Password))
                {
                    return user.Id;
                }
                else return null;
            }
            else return null;
        }
        public bool Register(RegisterModel model)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress.Equals(model.EmailAddress));
            if (user != null) return false;
            else
            {
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                newUser.SetEmailAddress(model.EmailAddress);
                newUser.SetPassword(model.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                return true;
            }
        }
        public string GenerateToken(int? userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            var claim = new Claim("sub", userId.ToString());
            claims.Add(claim);

            var Sectoken = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}
