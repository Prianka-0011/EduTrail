using System.Text.Json;
using EduTrail.Application.QuestionCatalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace EduTrail.API.Controllers
{
    // Read-only catalog of PrairieLearn-style questions discovered on disk.
    // GET api/QuestionCatalog
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionCatalogController : BaseController
    {
        public QuestionCatalogController(IMediator mediator) : base(mediator) { }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<QuestionCatalogItemDto>>> GetCatalog()
        {
            return Ok(await _mediator.Send(new GetQuestionCatalogQuery()));
        }

        // Start a new attempt at a question: runs server.py and returns rendered HTML.
        [Authorize]
        [HttpPost("{slug}/instances")]
        public async Task<ActionResult<QuestionInstanceDto>> CreateInstance(string slug)
        {
            var result = await _mediator.Send(new CreateQuestionInstanceCommand { Slug = slug });
            return result is null ? NotFound($"Question '{slug}' not found.") : Ok(result);
        }

        // Grade a submitted set of answers for an instance.
        [Authorize]
        [HttpPost("instances/{id:guid}/submit")]
        public async Task<ActionResult<SubmissionResultDto>> Submit(Guid id, [FromBody] JsonElement body)
        {
            var answers = body.TryGetProperty("answers", out var a) ? a : body;
            var result = await _mediator.Send(new SubmitAnswerCommand { InstanceId = id, Answers = answers });
            return result is null ? NotFound("Instance not found.") : Ok(result);
        }

        // Return the correct answers for an instance ("Show Correct Answer").
        [Authorize]
        [HttpGet("instances/{id:guid}/correct")]
        public async Task<ActionResult<JsonElement>> GetCorrect(Guid id)
        {
            var result = await _mediator.Send(new GetCorrectAnswersQuery { InstanceId = id });
            return result is null ? NotFound("Instance not found.") : Ok(result.Value);
        }
    }
}
