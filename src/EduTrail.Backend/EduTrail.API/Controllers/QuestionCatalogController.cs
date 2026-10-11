using EduTrail.Application.QuestionCatalog;
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

        [HttpGet]
        public async Task<ActionResult<List<QuestionCatalogItemDto>>> GetCatalog()
        {
            return Ok(await _mediator.Send(new GetQuestionCatalogQuery()));
        }
    }
}
