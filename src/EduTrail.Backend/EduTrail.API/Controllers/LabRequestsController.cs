using EduTrail.Application.Enrolements;
using EduTrail.Application.LabRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabRequestsController : BaseController
    {
        public LabRequestsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("help-request")]
        public async Task<ActionResult> SubmitHelpRequest(SubmitHelpRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("help-request-list/{courseOfferingId}")]
        public async Task<ActionResult> GetAllLabRequest(Guid courseOfferingId)
        {
            var result = await _mediator.Send(new GetAllLabRequest { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }

    }
}