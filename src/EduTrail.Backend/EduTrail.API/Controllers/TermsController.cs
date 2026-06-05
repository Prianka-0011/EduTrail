using EduTrail.Application.Courses;
using EduTrail.Application.Terms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsController : BaseController
    {
        public TermsController(IMediator mediator) : base(mediator)
        {
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<TermDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllTermsQuery()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TermDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetTermByIdQuery { Id = id }));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TermDto>> Create(CreateTermCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<TermDto>> Update(Guid id, UpdateTermCommand command)
        {

            if (id != command.TermDetailDto.Id)
            {
                return BadRequest("Term ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
    }
}