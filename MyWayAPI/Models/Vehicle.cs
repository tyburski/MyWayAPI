using MyWayAPI.Models.App;

namespace MyWayAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Type { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Route> Routes { get; set; }

    }
}
