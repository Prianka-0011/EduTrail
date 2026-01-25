
using EduTrail.Application.Assessments;
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

        [HttpGet]
        public async Task<ActionResult<List<AssessmentDetailDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllAssessmentQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AssessmentDetailDto>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAssessmentByIdQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AssessmentDetailDto>> Create([FromBody] CreateAssessmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AssessmentDetailDto>> Update(Guid id, [FromBody] UpdateAssessmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}