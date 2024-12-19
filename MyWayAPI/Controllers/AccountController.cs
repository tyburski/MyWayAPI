using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
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

        [Route("account/login")]
        [HttpPost]
        public IActionResult Login([FromBody]LoginModel model)
        {
            int? loginResponse = accountService.Login(model);
            if (loginResponse != null)
            {
                return Ok(accountService.GenerateToken(loginResponse, model.type));
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Incorrect credentials" });
        }
    }
}
