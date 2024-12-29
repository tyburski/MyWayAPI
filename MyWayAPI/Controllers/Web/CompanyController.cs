using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Services;
using MyWayAPI.Services.Web;

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

    }
}
