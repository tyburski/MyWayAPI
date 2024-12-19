namespace MyWayAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaxId { get; set; }
        public WebUser Owner { get; set; }
        public List<WebUser> WebUsers { get; private set; }
        public List<AppUser> AppUsers { get; private set; }

        public List<Route> Routes { get; private set; }

        public void AddWebUser(WebUser user)
        {
            WebUsers.Add(user);
        }
        public void AddAppUsser(AppUser user) 
        { 
            AppUsers.Add(user); 
        }
        public void AddRoute(Route route) 
        { 
            Routes.Add(route);
        }
    }
}
