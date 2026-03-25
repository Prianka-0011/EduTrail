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

        [HttpGet("help-request-list/{courseOfferingId}")]
        public async Task<ActionResult> GetAllLabRequest(Guid courseOfferingId)
        {
            var result = await _mediator.Send(new GetAllLabRequestQuery { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetLabRequestById(Guid Id)
        {
            var result = await _mediator.Send(new GetRequestByIdQuery { Id = Id });
            return Ok(result);
        }

        [HttpPost("help-request")]
        public async Task<ActionResult> SubmitHelpRequest(SubmitHelpRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("help-request-by-current-user-list/{courseOfferingId}")]
        public async Task<ActionResult> GetAllLabRequestByCurrentUser(Guid courseOfferingId)
        {
            var result = await _mediator.Send(new GetAllLabRequestByCurrentUserQuery { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRequest(UpdateHelpRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}