using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Services.App;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers.App
{
    [ApiController]
    public class VehicleController: ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [Route("app/createVehicle/{licensePlate}")]
        [HttpPost]
        public IActionResult CreateVehicle(string licensePlate, [FromHeader]string accessToken, [FromQuery]string type)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = vehicleService.CreateVehicle(userId, licensePlate, type);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/deleteVehicle/{id}")]
        [HttpPost]
        public IActionResult DeleteVehicle(int id, [FromHeader]string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = vehicleService.DeleteVehicle(userId, id);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/vehicles")]
        [HttpPost]
        public IActionResult GetVehicles([FromHeader]string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = vehicleService.GetVehicles(userId);
            return Ok(result);
        }


    }
}
