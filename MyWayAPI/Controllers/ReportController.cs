using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Services;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Utilities;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class ReportController: ControllerBase
    {
        IReportService reportService;
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [Route("api/report/generate")]
        [HttpPost]
        public IActionResult GetReport([FromQuery]int id)
        {
            reportService.GenerateReport(id);
            return Ok();
        }
    }
}
