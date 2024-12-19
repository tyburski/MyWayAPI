namespace MyWayAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<WebUser> WebUsers { get; set; }
    }
}
