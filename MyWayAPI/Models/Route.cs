namespace MyWayAPI.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public List<RouteEvent> RouteEvents { get; set; }
        public bool Finished { get; set; }

        public string CurrentCountry { get; set; }
    }
}
