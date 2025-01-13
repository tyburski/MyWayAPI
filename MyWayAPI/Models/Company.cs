using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;

namespace MyWayAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Route> Routes { get; set; }
    }
}
