using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.Body;
using MyWayAPI.Models.RequestModels;
using MyWayAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService routeService;
        private readonly ITokenDecoder tokenDecoder;

        public RouteController(IRouteService routeService, ITokenDecoder tokenDecoder)
        {
            this.routeService = routeService;
            this.tokenDecoder = tokenDecoder;
        }

        [Route("api/route/start")]
        [HttpPost]
        public IActionResult StartRoute([FromHeader] string accessToken, [FromBody]StartModel body)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = routeService.StartRoute(userId, body);
            if (result is true) return Created();
            else return BadRequest("Unable to create");
        }

        [Route("api/pickup/create")]
        [HttpPost]
        public IActionResult CreatePickup([FromBody] CreatePickupModel body)
        {
            var result = routeService.NewPickup(body);
            if (result is true) return Created();
            else return BadRequest("Route not found");
        }

        [Route("api/refuel/create")]
        [HttpPost]
        public IActionResult CreateRefuel([FromBody] CreateRefuelModel body)
        {
            var result = routeService.NewRefuel(body);
            if (result is true) return Created();
            else return BadRequest("Route not found");
        }

        [Route("api/border/create")]
        [HttpPost]
        public IActionResult CreateBorder([FromBody] CreateBorderModel body)
        {
            var result = routeService.NewBorder(body);
            if (result is true) return Created();
            else return BadRequest("Route not found");
        }

        [Route("api/pickup/drop")]
        [HttpPost]
        public IActionResult Drop([FromBody] DropModel body)
        {
            var result = routeService.Drop(body);
            if (result is true) return Ok();
            else return BadRequest("Event not found");
        }

        [Route("api/route/getStarted")]
        [HttpGet]
        public IActionResult GetStartedRoute([FromHeader] string accessToken)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = routeService.GetStartedRoute(userId);
            return Ok(result);
        }
        
        [Route("api/route/finish")]
        [HttpPost]
        public IActionResult FinishRoute([FromHeader] string accessToken, [FromQuery] int routeId)
        {
            var userId = tokenDecoder.Decode(accessToken);
            if (userId is null) return Unauthorized("Invalid token");

            var result = routeService.FinishRoute(routeId, userId);
            if (result is true) return Ok();
            else return BadRequest();
        }
    }
}
