using EduTrail.Application.Assesments;
using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentsController : BaseController
    {
        public AssesmentsController(IMediator mediator) : base(mediator)
        {
        }
        public async Task<ActionResult<List<AssesmentDetailDto>>> GetAll()
        {
            var result = _mediator.Send(new GetAllAssesmentQuery());
            return Ok(result);
        }

        public async Task<ActionResult<List<AssesmentDetailDto>>> GetById(Guid id)
        {
            var result = _mediator.Send(new GetAssesmentByIdQuery());
            return Ok(result);
        }
    }
}