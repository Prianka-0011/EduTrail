using EduTrail.Application.Enrolements;
using EduTrail.Application.UserDashboards;
using MediatR;
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

            if (id != command.enrolementDto.Id)
            {
                return BadRequest("Enrolement ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
        // [HttpPut]
        // public async Task<ActionResult>SubmitHelpRequest()
        // {
            
        // }
    }
}