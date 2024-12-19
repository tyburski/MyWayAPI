namespace MyWayAPI.Models
{
    public class WebUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }
        public Role Role { get; private set; }
        public Company Company { get; set; }

        public void SetEmailAddress(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public void SetPassword(string password)
        {
            Password = password;
        }
        public void SetRole(Role role)
        {
            Role = role;
        }
    }
}
