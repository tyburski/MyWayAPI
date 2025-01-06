using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;
using MyWayAPI.Services;
using System.IdentityModel.Tokens.Jwt;

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
        public IActionResult AppRegister([FromBody] AppRegisterModel model)
        {
            bool registerResponse = accountService.Register(model);
            if (registerResponse is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "User already exists" });
        }

        [Route("app/account/companies")]
        [HttpGet]
        public IActionResult GetCompanies([FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var companies = accountService.GetCompanies(userId);
            return Ok(companies);
        }

        [Route("app/account/deleteCompany/{companyId}")]
        [HttpPost]
        public IActionResult DeleteCompany(int companyId, [FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = accountService.DeleteCompany(userId,  companyId);
            if (result is true)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}
