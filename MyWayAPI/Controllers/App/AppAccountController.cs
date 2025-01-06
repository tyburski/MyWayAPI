using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;
using MyWayAPI.Services;

namespace MyWayAPI.Controllers.App
{
    [ApiController]
    public class AppAccountController : ControllerBase
    {
        private readonly IAppAccountService accountService;
        public AppAccountController(IAppAccountService accountService)
        {
            this.accountService = accountService;
        }

        [Route("app/account/login")]
        [HttpPost]
        public IActionResult AppLogin([FromBody] LoginModel model)
        {
            int? loginResponse = accountService.Login(model);
            if (loginResponse != null)
            {
                return Ok(accountService.GenerateToken(loginResponse));
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Incorrect credentials" });
        }

        [Route("app/account/register")]
        [HttpPost]
        public IActionResult AppRegister([FromBody]AppRegisterModel model)
        {
            bool registerResponse = accountService.Register(model);
            if (registerResponse is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "User already exists" });
        }
    }
}
