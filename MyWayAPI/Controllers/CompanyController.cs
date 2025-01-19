using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Services;

namespace MyWayAPI.Controllers.Web
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly ITokenDecoder tokenDecoder;
        public CompanyController(ICompanyService companyService, ITokenDecoder tokenDecoder)
        {
            this.companyService = companyService;
            this.tokenDecoder = tokenDecoder;
        }

        [Route("api/company/create")]
        [HttpPost]
        public IActionResult CreateCompany([FromHeader] string accessToken, [FromQuery] string name, [FromQuery] string email)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = companyService.CreateCompany(userId, name, email);
            if (result is true) return Created();
            else return BadRequest("Company already exists");
        }

        [Route("api/company/delete")]
        [HttpPost]
        public IActionResult DeleteCompany([FromHeader] string accessToken, [FromQuery]int companyId)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = companyService.DeleteCompany(userId, companyId);
            if (result is true) return NoContent();
            else return BadRequest("Company not found");
        }

        [Route("api/company/getByUser")]
        [HttpGet]
        public IActionResult GetCompanies([FromHeader] string accessToken)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = companyService.GetCompanies(userId);
            return Ok(result);
        }
    }
}