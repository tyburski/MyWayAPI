﻿using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyWayAPI.Services
{
    public interface IAppAccountService
    {
        public int? Login(LoginModel model);
        public bool Register(AppRegisterModel model);
        public string GenerateToken(int? userId);
        public List<Company> GetCompanies(int userId);
        public bool DeleteCompany(int userId, int companyId);
    }
    public class AppAccountService : IAppAccountService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public AppAccountService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public int? Login(LoginModel model)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.EmailAddress.Equals(model.emailAddress));
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
        public bool Register(AppRegisterModel model)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.EmailAddress.Equals(model.emailAddress));
            if (user != null)
            {
                return false;
            }
            else
            {
                var newUser = new AppUser
                {
                    FirstName = model.firstName,
                    LastName = model.lastName,
                };
                newUser.SetEmailAddress(model.emailAddress);
                newUser.SetPassword(model.password);
                dbContext.AppUsers.Add(newUser);
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

        public List<Company> GetCompanies(int userId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            return user.Companies;
        }

        public bool DeleteCompany(int userId, int companyId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            var company = dbContext.Companies.FirstOrDefault(c=> c.Id == companyId);
            var userInCompany = user.Companies.FirstOrDefault(c => c.Id == companyId);

            if (user is null || company is null) return false;
            else if (userInCompany is null) return false;
            else
            {
                user.Companies.Remove(company);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}
