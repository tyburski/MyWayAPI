namespace MyWayAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Route> Routes { get; set; }

    }
}
