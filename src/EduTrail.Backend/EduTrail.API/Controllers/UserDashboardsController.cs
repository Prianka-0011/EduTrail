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
            var result = await _mediator.Send(new GetEnrollmentByUserId {CourseOfferingId = courseOfferingId});
            return Ok(result);
        }
    }
}