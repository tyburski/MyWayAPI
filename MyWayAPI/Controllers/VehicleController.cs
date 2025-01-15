using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;
        private readonly ITokenDecoder tokenDecoder;

        public VehicleController(IVehicleService vehicleService, ITokenDecoder tokenDecoder)
        {
            this.vehicleService = vehicleService;
            this.tokenDecoder = tokenDecoder;
        }

        [Route("api/vehicle/create")]
        [HttpPost]
        public IActionResult CreateVehicle([FromHeader] string accessToken, [FromQuery] string type, [FromQuery] string licensePlate)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = vehicleService.CreateVehicle(userId, licensePlate, type);
            if (result is true) return Created();
            else return BadRequest("Unable to create");
        }

        [Route("api/vehicle/delete")]
        [HttpPost]
        public IActionResult DeleteVehicle([FromHeader] string accessToken, [FromQuery] int vehicleId)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = vehicleService.DeleteVehicle(userId, vehicleId);
            if (result is true) return NoContent();
            else return BadRequest();
        }

        [Route("api/vehicle/getByUser")]
        [HttpGet]
        public IActionResult GetVehicles([FromHeader] string accessToken)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = vehicleService.GetVehicles(userId);
            return Ok(result);
        }
    }
}
