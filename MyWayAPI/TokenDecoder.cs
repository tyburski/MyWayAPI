using Azure.Core;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI
{
    public interface ITokenDecoder
    {
        public int? Decode(string token);
    }
    public class TokenDecoder: ITokenDecoder
    {
        public int? Decode(string token)
        {
            var stream = token;
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            if (tokenS is null) return null;
            if(tokenS.Subject is null) return null;

            var userId = int.Parse(tokenS.Subject.ToString());

            return userId;
        }
    }
}
