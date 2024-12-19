namespace MyWayAPI.Models
{
    public class Route
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public Company Company { get; set; }
        public List<RouteEvent> RouteEvents { get; set; }
        public bool Finished { get; set; }
    }
}
