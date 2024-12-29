namespace MyWayAPI.Models.Web
{
    public class WebUser: User
    {
        public int AccessLevel { get; private set; }
        public Company Company { get; set; }

        public void SetAccessLevel(int level)
        {
            AccessLevel = level;
        }
    }
}
