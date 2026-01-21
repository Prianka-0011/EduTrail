using EduTrail.Application.Assesments;
using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : BaseController
    {
        public AssessmentsController(IMediator mediator) : base(mediator)
        {
        }
        public async Task<ActionResult<List<AssessmentDetailDto>>> GetAll()
        {
            var result = _mediator.Send(new GetAllAssessmentQuery());
            return Ok(result);
        }

        public async Task<ActionResult<List<AssessmentDetailDto>>> GetById(Guid id)
        {
            var result = _mediator.Send(new GetAssessmentByIdQuery());
            return Ok(result);
        }

        public async Task<ActionResult<AssessmentDetailDto>> Create([FromBody] CreateAssessmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        public async Task<ActionResult<AssessmentDetailDto>> Update([FromBody] UpdateAssessmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }   
    }
}