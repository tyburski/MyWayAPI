using Microsoft.AspNetCore.Mvc;
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

        [Route("app/route/start/{companyId}")]
        [HttpPost]
        public IActionResult StartRoute(int companyId, [FromHeader]string accessToken, [FromQuery]int vehicleId)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = routeService.StartRoute(userId, companyId, vehicleId);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [Route("app/route/finish")]
        [HttpPost]
        public IActionResult FinishRoute([FromHeader]string accessToken, [FromQuery]int routeId)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            var result = routeService.FinishRoute(routeId, userId);
            if (result is true) return Ok();
            else return BadRequest();
        }
    }
}
