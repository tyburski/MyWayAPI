using Microsoft.AspNetCore.Mvc;
using MyWayAPI.Models;
using MyWayAPI.Models.Web;
using MyWayAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWayAPI.Controllers
{
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService invitationService;
        public InvitationController(IInvitationService invitationService)
        {
            this.invitationService = invitationService;
        }

        [Route("web/invite/{emailAddress}")]
        [HttpPost]
        public IActionResult Invite(string emailAddress, [FromHeader]string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var creatorAccess = tokenS.Claims.First(claim => claim.Type == "access").Value;

            if (int.Parse(creatorAccess) < 1)
            {
                return Unauthorized();
            }
            var creatorId = int.Parse(tokenS.Subject.ToString());

            bool inviteResponse = invitationService.Invite(emailAddress, creatorId);
            if (inviteResponse is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "User does not exist" });
        }

        [Route("app/invite/accept/{id}")]
        [HttpPost]
        public IActionResult AcceptInvite(int id)
        {
            bool acceptInvitationResult = invitationService.AcceptInvitation(id);
            if (acceptInvitationResult is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "" });
        }

        [Route("app/invite/decline/{id}")]
        [HttpPost]
        public IActionResult DeclineInvite(int id)
        {
            bool acceptInvitationResult = invitationService.DeclineInvitation(id);
            if (acceptInvitationResult is true)
            {
                return Ok();
            }
            else return StatusCode(StatusCodes.Status500InternalServerError, new { message = "" });
        }

        [Route("app/invite/getall")]
        [HttpGet]
        public IActionResult GetByUser([FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            return Ok(invitationService.GetByUser(userId));
        }
        [Route("web/invite/getall")]
        [HttpGet]
        public IActionResult GetByCompany([FromHeader] string accessToken)
        {
            var stream = accessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = int.Parse(tokenS.Subject.ToString());

            return Ok(invitationService.GetByCompany(userId));
        }

    }
}
