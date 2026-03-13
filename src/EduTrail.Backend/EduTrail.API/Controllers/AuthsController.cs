
using EduTrail.Application.Assessments;
using EduTrail.Application.Auths;
using EduTrail.Application.Courses;
using MediatR;
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

            if (string.IsNullOrEmpty(res))
                return Unauthorized();

            return Ok(new { token = res });
        }

        // public async Task<ActionResult>ResetPassword()
        // {

        // }
    }
}