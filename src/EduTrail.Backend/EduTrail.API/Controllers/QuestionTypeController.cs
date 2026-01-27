using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionTypesController : BaseController
    {
        public QuestionTypesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<QuestionTypeDetailDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllQuestionTypeQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionTypeDetailDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetQuestionTypeByIdQuery { Id = id }));
        }
        [HttpPost]
        public async Task<ActionResult<QuestionTypeDetailDto>> Create(CreateQuestionTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionTypeDetailDto>> Update(Guid id, UpdateQuestionTypeCommand command)
        {

            if (id != command.TypeDto.Id)
            {
                return BadRequest("Course ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
    }
}
