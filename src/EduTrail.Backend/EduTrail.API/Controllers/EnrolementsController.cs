using EduTrail.Application.Courses;
using EduTrail.Application.Enrolements;
using MediatR;
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

        [HttpGet("{courseOfferingId}")]
        public async Task<ActionResult<List<EnrolementDto>>> GetAll(Guid? courseOfferingId)
        {
            return Ok(await _mediator.Send(new GetAllEnrolementsQuery { CourseOfferingId = courseOfferingId }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrolementDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetEnrolementByIdQuery { Id = id }));
        }
        [HttpPost]
        public async Task<ActionResult<EnrolementDto>> Create(CreateEnrolementCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<EnrolementDto>> Update(Guid id, UpdateEnrolementCommand command)
        {

            if (id != command.enrolementDetailDto.Id)
            {
                return BadRequest("Enrolement ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
    }
}