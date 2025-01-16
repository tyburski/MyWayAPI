namespace MyWayAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public List<Route> Routes { get; set; }
    }
}
