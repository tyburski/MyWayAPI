using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWayAPI.Services
{
    public interface IAdminService
    {
        public string Login(string username, string password);
    }
    public class AdminService: IAdminService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public AdminService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public string Login(string username, string password)
        {
            var user = dbContext.Admin.FirstOrDefault(u => u.Login.Equals(username));
            if (user != null && user.AccessLevel == 3)
            {
                if (password.Equals(user.Password))
                {
                    return(GenerateToken(user.Id));

                }
                else return String.Empty;
            }
            else return String.Empty;

        }
        public string GenerateToken(int? userId)
        {
            var user = dbContext.Admin.FirstOrDefault(u => u.Id == userId);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            var claim = new Claim("sub", userId.ToString());
            var accessClaim = new Claim("access", user.AccessLevel.ToString());
            claims.Add(claim);
            claims.Add(accessClaim);

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
