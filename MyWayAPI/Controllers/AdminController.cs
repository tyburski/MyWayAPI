using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Services;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [Route("admin/login")]
        [HttpPost]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            string loginResponse = adminService.Login(username, password);
            if (!loginResponse.Equals(String.Empty))
            {
                return Ok(loginResponse);
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Incorrect credentials" });
        }
    }
}
