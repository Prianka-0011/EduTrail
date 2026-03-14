
using System.Security.Claims;
using EduTrail.Application.Assessments;
using EduTrail.Application.Auths;
using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : BaseController
    {
        public AuthsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("sign-in")]
        public async Task<ActionResult> SignIn(SignInCommand command)
        {
            var res = await _mediator.Send(command);

            if (!res)
                return Unauthorized();

            return Ok(res);
        }

        [HttpGet("is-login")]
        // [Authorize]
        public async Task<ActionResult> IsLogin()
        {
            var isLoggedIn = await _mediator.Send(new IsLoginQuery());
            return Ok(new { IsAuthenticated = isLoggedIn });
        }
        // public async Task<ActionResult>ResetPassword()
        // {

        // }
    }
}