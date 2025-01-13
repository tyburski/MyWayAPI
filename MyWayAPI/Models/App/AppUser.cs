namespace MyWayAPI.Models.App
{
    public class AppUser: User
    {
        public List<Company> Companies { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }

    }
}
