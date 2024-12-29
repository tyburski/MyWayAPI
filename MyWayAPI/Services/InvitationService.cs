using MyWayAPI.Models.Web;
using MyWayAPI.Models;

namespace MyWayAPI.Services
{
    public interface IInvitationService
    {
        public bool Invite(string emailAddress, int creatorId);
        public bool AcceptInvitation(int id);
        public bool DeclineInvitation(int id);
        public List<Invitation> GetByUser(int userId);
        public List<Invitation> GetByCompany(int userId);
    }
    public class InvitationService : IInvitationService
    {
        private IConfiguration config;
        private readonly MWDbContext dbContext;
        public InvitationService(IConfiguration config, MWDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }
        public bool Invite(string emailAddress, int creatorId)
        {
            var creator = dbContext.WebUsers.FirstOrDefault(u=>u.Id == creatorId);
            var company = creator.Company;
            var user = dbContext.AppUsers.FirstOrDefault(u=>u.EmailAddress.Equals(emailAddress));
            if(user is null || creator is null)
            {
                return false;
            }

            var invitation = new Invitation
            {
                AppUser = user,
                Company = company
            };
            dbContext.Invitations.Add(invitation);
            dbContext.SaveChanges();
            return true;
        }
        public bool AcceptInvitation(int id)
        {
            var invitation = dbContext.Invitations.FirstOrDefault(u => u.Id == id);
            var user = invitation.AppUser;
            var company = invitation.Company;

            if(invitation is null)
            {
                return false;
            }
            user.Companies.Add(invitation.Company);
            company.AppUsers.Add(user);
            dbContext.Invitations.Remove(invitation);
            dbContext.SaveChanges();
            return true;            
        }
        public bool DeclineInvitation(int id)
        {
            var invitation = dbContext.Invitations.FirstOrDefault(u => u.Id == id);

            if (invitation is null)
            {
                return false;
            }
            dbContext.Invitations.Remove(invitation);
            dbContext.SaveChanges();
            return true;

        }

        public List<Invitation> GetByUser(int userId)
        {
            var list = dbContext.Invitations.Where(u => u.AppUser.Id == userId).ToList();
            return list;
        }
        public List<Invitation> GetByCompany(int userId)
        {
            var user = dbContext.WebUsers.FirstOrDefault(u => u.Id==userId);
            var company = user.Company;
            var list = dbContext.Invitations.Where(u => u.Company.Id == company.Id).ToList();
            return list;
        }
    }
}
