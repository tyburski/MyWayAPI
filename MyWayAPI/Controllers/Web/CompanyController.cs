using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Services;
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

        [Route("admin/createcompany")]
        [HttpPost]
        public IActionResult CreateCompany([FromHeader] string accessToken, [FromBody] CreateCompanyModel model)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess) < 3)
            {
                return Unauthorized();
            }
            bool result = companyService.CreateCompany(model);
            if (result is true)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}
