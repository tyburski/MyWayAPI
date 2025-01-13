using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.Web;
using System.Text;

namespace MyWayAPI.Services.Web
{
    public interface ICompanyService
    {
        public bool CreateCompany(int userId, string name, string email);
        public bool DeleteCompany(int userId, int companyId);
        public List<Company> GetCompanies(int userId);
    }
    public class CompanyService: ICompanyService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public CompanyService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public bool CreateCompany(int userId, string name, string email)
        {
            var user = dbContext.AppUsers.Include(u => u.Companies).FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;


            var checkCompanyName = user.Companies.FirstOrDefault(v => v.Name.Equals(name));
            if (checkCompanyName is not null) return false;

            var checkCompanyEmail = user.Companies.FirstOrDefault(v => v.Email.Equals(email));
            if (checkCompanyEmail is not null) return false;


            var newCom = new Company() { AppUser = user, Name = name.ToUpper(), Email = email };
            dbContext.Companies.Add(newCom);
            dbContext.SaveChanges();
            return true;
        }

        public bool DeleteCompany(int userId, int companyId)
        {
            var user = dbContext.AppUsers.Include(u => u.Companies).FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;


            var company = user.Companies.FirstOrDefault(v => v.Id == companyId);
            if (company is null) return false;

            dbContext.Companies.Remove(company);
            dbContext.SaveChanges();
            return true;

        }

        public List<Company> GetCompanies(int userId)
        {
            var companies = dbContext.Companies.Where(v => v.AppUserId == userId).ToList();
            if (companies is not null) return companies;
            else return new List<Company>();

        }
    }
}
