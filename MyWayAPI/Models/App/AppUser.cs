namespace MyWayAPI.Models.App
{
    public class AppUser: User
    {
        public List<Invitation> Invitations { get; set; }
        public List<Company> Companies { get; set; }

    }
}
