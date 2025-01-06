using MyWayAPI.Models;
using MyWayAPI.Models.Web;
using System.Text;

namespace MyWayAPI.Services.Web
{
    public interface ICompanyService
    {
        public bool CreateCompany(CreateCompanyModel model);
    }
    public class CompanyService: ICompanyService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        private IMailService mailService;
        public CompanyService(IConfiguration config, MWDbContext dbContext, IMailService mailService)
        {
            this.config = config;
            this.dbContext = dbContext;
            this.mailService = mailService;
        }
        public bool CreateCompany(CreateCompanyModel model)
        {
            var company = new Company();
            company.Name = model.Name;
            company.TaxId = model.TaxId;

            var user = dbContext.WebUsers.FirstOrDefault(u => u.EmailAddress.Equals(model.OwnerEmailAddress));
            if (user != null)
            {
                return false;
            }
            else
            {
                var newUser = new WebUser
                {
                    FirstName = model.OwnerFirstName,
                    LastName = model.OwnerLastName,
                    Company = company
                };
                newUser.SetEmailAddress(model.OwnerEmailAddress);

                newUser.SetPassword(new PasswordBuilder().CreatePassword());
                newUser.SetAccessLevel(2); //0 - niekatywny, 1 - zwykły użytkownik, 2 - administrator
                dbContext.WebUsers.Add(newUser);
                company.AddWebUser(newUser);
                

                mailService.SendEmail(newUser.EmailAddress, "Utworzenie firmy w systemie",
                 $"Witaj,{newUser.FirstName} \n Firma {company.Name} została utworzona. \n Twój login to: {newUser.EmailAddress} \n, a wygenerowane hasło to: {newUser.Password}. \n Pamiętaj aby zmienić hasło jak najszybciej to możliwe!");
                dbContext.SaveChanges();
                return true;
            }

        }      
    }
}
