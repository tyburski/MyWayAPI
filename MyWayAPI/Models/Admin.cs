namespace MyWayAPI.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Login {  get; private set; }
        public string Password { get; private set; }

        public int AccessLevel { get; private set;}

        public void Credentials(string username, string password)
        {
            Login = username;
            Password = password;
            AccessLevel = 3;
        }
    }
}
