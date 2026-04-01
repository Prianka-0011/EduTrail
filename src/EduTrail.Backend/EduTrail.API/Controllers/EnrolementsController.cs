using EduTrail.Application.Courses;
using EduTrail.Application.Enrolements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolementsController : BaseController
    {
        public EnrolementsController(IMediator mediator) : base(mediator)
        {
        }
        [Authorize]
        [HttpGet("course-offerings/{courseOfferingId}")]
        public async Task<ActionResult<EnrolementDto>> GetAll(Guid? courseOfferingId)
        {
            var result = await _mediator.Send(new GetAllEnrolementsQuery { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }

        [Authorize]
        [HttpGet("active-ta/{courseOfferingId}")]
        public async Task<ActionResult<EnrolementDto>> GetAllActiveTAs(Guid? courseOfferingId)
        {
            var result = await _mediator.Send(new GetAllActiveTAsQuery { CourseOfferingId = courseOfferingId });
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrolementDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetEnrolementByIdQuery { Id = id }));
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EnrolementDto>> Create(CreateEnrolementCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<EnrolementDto>> Update(Guid id, UpdateEnrolementCommand command)
        {

            if (id != command.enrolementDto.Id)
            {
                return BadRequest("Enrolement ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("bulk-upload")]
        public async Task<IActionResult> BulkUpload([FromForm] BulkEnrollmentUploadCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}