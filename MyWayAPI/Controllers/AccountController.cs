using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.Body;
using MyWayAPI.Models.RequestModels;
using MyWayAPI.Services;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenDecoder tokenDecoder;
        public AccountController(IAccountService accountService, ITokenDecoder tokenDecoder)
        {
            this.accountService = accountService;
            this.tokenDecoder = tokenDecoder;
        }

        [Route("api/account/login")]
        [HttpPost]
        public IActionResult Login([FromQuery]string username, [FromQuery]string password)
        {
            int? loginResponse = accountService.Login(username, password);
            if (loginResponse != null)
            {
                return Ok(accountService.GenerateToken(loginResponse));
            }
            else return Unauthorized("Incorrect credentials");
        }

        [Route("api/account/register")]
        [HttpPost]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            bool registerResponse = accountService.Register(model);
            if (registerResponse is true) return Created();
            else return BadRequest("User already exists");
        }
        [Route("api/account/getUser")]
        [HttpGet]
        public IActionResult GetUser([FromHeader] string accessToken)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = accountService.GetUser(userId);
            if (result is null) return BadRequest();
            else return Ok(result);
    }
    }
    
}
