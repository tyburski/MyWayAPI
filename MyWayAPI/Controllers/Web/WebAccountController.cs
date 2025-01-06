using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.Web;
using MyWayAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class WebAccountController : ControllerBase
    {
        private readonly IWebAccountService accountService;
        public WebAccountController(IWebAccountService accountService)
        {
            this.accountService = accountService;
        }

        [Route("web/account/login")]
        [HttpPost]
        public IActionResult Login([FromBody]LoginModel model)
        {
            int? loginResponse = accountService.Login(model);
            if (loginResponse != null)
            {
                return Ok(accountService.GenerateToken(loginResponse));
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Incorrect credentials" });
        }
        [Route("web/account/register")]
        [HttpPost]
        public IActionResult Register([FromHeader]string accessToken, [FromBody]WebRegisterModel model)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess)<2) 
            {
                return Unauthorized();
            }

            bool registerResponse = accountService.Register(model);
            if (registerResponse is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "User already exists" });
        }
        [Route("web/account/access/{userId}")]
        [HttpPost]
        public IActionResult ChangeAccess(int userId, [FromHeader]string accessToken, [FromQuery] int accessLvl)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess) < 2)
            {
                return Unauthorized();
            }
            bool changeAccessResponse = accountService.ChangeAccess(userId, accessLvl);
            if (changeAccessResponse is true)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [Route("web/getWebUsers")]
        [HttpGet]
        public IActionResult GetWebUsers([FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess) == 0)
            {
                return Unauthorized();
            }

            var userId = int.Parse(tokenS.Subject.ToString());
            return Ok(accountService.GetWebUsers(userId));
        }
        [Route("web/getAppUsers")]
        [HttpGet]
        public IActionResult GetAppUsers([FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess) == 0)
            {
                return Unauthorized();
            }

            var userId = int.Parse(tokenS.Subject.ToString());
            return Ok(accountService.GetAppUsers(userId));
        }

    }
}
