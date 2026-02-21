using EduTrail.Application.CourseOfferings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseOfferingsController : BaseController
    {
        public CourseOfferingsController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllCourseOfferings()
        {
            var result = await _mediator.Send(new GetAllCourseOfferingQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourseOfferingById(Guid id)
        {
            var result = await _mediator.Send(new GetCourseOfferingByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourseOffering([FromBody] CreateCourseOfferingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourseOffering(Guid id, [FromBody] UpdateCourseOfferingCommand command)
        {
            if (id != command.CourseOfferingDetailDto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}