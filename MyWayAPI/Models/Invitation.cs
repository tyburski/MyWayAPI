using MyWayAPI.Models.App;

namespace MyWayAPI.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public Company Company { get; set; } 
    }
}
