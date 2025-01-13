using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Services;
using MyWayAPI.Services.App;
using MyWayAPI.Services.Web;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers.Web
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;
        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [Route("app/createCompany/{name}")]
        [HttpPost]
        public IActionResult CreateCompany(string name, [FromHeader] string accessToken, [FromQuery] string email)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = companyService.CreateCompany(userId, name, email);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/deleteCompany/{id}")]
        [HttpPost]
        public IActionResult DeleteCompany(int id, [FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = companyService.DeleteCompany(userId, id);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/companies")]
        [HttpGet]
        public IActionResult GetCompanies([FromHeader] string accessToken)
        {
            Console.WriteLine(accessToken);
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;


            var userId = int.Parse(tokenS.Subject.ToString());


            var result = companyService.GetCompanies(userId);
            Console.WriteLine(result);
            return Ok(result);
        }
    }
}
