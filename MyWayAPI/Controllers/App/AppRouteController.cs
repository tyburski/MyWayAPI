using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Services.App;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers.App
{
    [ApiController]
    public class AppRouteController : ControllerBase
    {
        private readonly IAppRouteService routeService;

        public AppRouteController(IAppRouteService routeService)
        {
            this.routeService = routeService;
        }

        [Route("app/newRoute")]
        [HttpPost]
        public IActionResult StartRoute([FromHeader]string accessToken, [FromQuery]int vehicleId, [FromQuery]int companyId)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = routeService.StartRoute(userId, vehicleId, companyId);
            if (result > 0) return Ok(result);
            else return BadRequest(result);
        }

        [Route("app/newEvent")]
        [HttpPost]
        public IActionResult NewEvent([FromBody]RouteEventBody body)
        {
            var result = routeService.NewEvent(body);
            if(result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/drop")]
        [HttpPost]
        public IActionResult Drop([FromBody] DropBody body)
        {
            var result = routeService.Drop(body);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/startedRoute")]
        [HttpGet]
        public IActionResult GetStartedRoute([FromHeader]string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = routeService.GetStartedRoute(userId);
            return Ok(result);
        }
        [Route("app/finishRoute/{id}")]
        [HttpPost]
        public IActionResult FinishRoute(int id, [FromHeader]string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = routeService.FinishRoute(id, userId);
            if (result is true) return Ok();
            else return BadRequest();
        }
    }
}
