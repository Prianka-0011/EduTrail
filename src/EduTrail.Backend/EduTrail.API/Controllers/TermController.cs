using EduTrail.Application.Courses;
using EduTrail.Application.Terms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    public class TermController : BaseController
    {
        public TermController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<TermDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllTermsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TermDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetCourseByIdQuery { Id = id }));
        }
    }
}