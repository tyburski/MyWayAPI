using System.Text;

namespace MyWayAPI
{
    public class PasswordBuilder
    {
        public string CreatePassword()
        {
            int length = 8;
            const string valid = "abcdefghkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
