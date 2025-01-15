using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.Body;
using MyWayAPI.Services;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
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
    }
}
