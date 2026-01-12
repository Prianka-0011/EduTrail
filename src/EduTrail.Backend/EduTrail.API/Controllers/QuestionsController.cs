using EduTrail.Application.Questions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : BaseController
    {

        public QuestionsController(IMediator mediator) : base(mediator) { }
        [HttpGet]
        public async Task<ActionResult<List<QuestionDetailDto>>> GetAll()
        {
            return Ok( await _mediator.Send(new GetAllQuestionQuery()));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDetailDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetQuestionByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDetailDto>> Create([FromBody] CreateQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDetailDto>> Update(Guid id, UpdateQuestionCommand command)
        {
            if (id != command.questionDetailDto.Id)
            {
                return BadRequest("Question ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
    }
}