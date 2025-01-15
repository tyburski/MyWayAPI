namespace MyWayAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }
        public List<Company> Companies { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }

        public void SetEmailAddress(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public void SetPassword(string password)
        {
            Password = password;
        }
    }
}
