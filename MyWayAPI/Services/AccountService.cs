using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyWayAPI.Services
{
    public interface IAccountService
    {
        public int? Login(LoginModel model);
        public string GenerateToken(int? userId, string type);
        public Role GetRole(int? userId);
    }
    public class AccountService: IAccountService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public AccountService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public int? Login(LoginModel model)
        {
            if (model.type.Equals("app"))
            {
                var user = dbContext.AppUsers.FirstOrDefault(u => u.EmailAddress == model.emailAddress);
                if (user != null)
                {
                    if (model.password.Equals(user.Password))
                    {
                        return user.Id;
                    }
                    else return null;
                }
                else return null;
            }
            else if (model.type.Equals("web"))
            {
                var user = dbContext.WebUsers.FirstOrDefault(u => u.EmailAddress == model.emailAddress);
                if (user != null)
                {
                    if (model.password.Equals(user.Password))
                    {
                        return user.Id;
                    }
                    else return null;
                }
                else return null;
            }
            else return null;
            
        }
        public string GenerateToken(int? userId, string type)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            var claim = new Claim("sub", userId.ToString());
            claims.Add(claim);
            if (type.Equals("web"))
            {
                var roleClaim = new Claim("role", GetRole(userId).ToString());
                claims.Add(roleClaim);
            }

            var Sectoken = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
        public Role GetRole(int? userId)
        {
            return dbContext.WebUsers.FirstOrDefault(u => u.Id == userId).Role;
        }

    }
}
