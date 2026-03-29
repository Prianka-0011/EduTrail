using EduTrail.Application.Enrolements;
using EduTrail.Application.UserDashboards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardsController : BaseController
    {
        public UserDashboardsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCourseOfferingByUser()
        {
            var result = await _mediator.Send(new GetAllCourseOfferingByUserQuery());
            return Ok(result);
        }
        [HttpGet("{courseOfferingId}")]
        public async Task<ActionResult> GetEnrollementById(Guid courseOfferingId)
        {
            var result = await _mediator.Send(new GetEnrollmentByUserId { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }
        [HttpGet("ta-hours/{courseOfferingId}")]
        public async Task<ActionResult> GetTAAndLabHoursByCourseOffering(Guid courseOfferingId)
        {
            var result = await _mediator.Send(new GetTAAndLabHoursByCourseOfferingQuery { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UserEnrollementDto>> Update(Guid id, UpdateTAHourForEnrolementCommand command)
        {

            if (id != command.EnrolementDto.Id)
            {
                return BadRequest("Enrolement ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpGet("current-login-user")]
        public async Task<ActionResult> GetCurrentLoginUser()
        {
            var result = await _mediator.Send(new GetCurrentLoginUserQuery());
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}