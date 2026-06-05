using Microsoft.AspNetCore.Mvc;
using EduTrail.Application.Courses;
using MediatR;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authorization;
namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        public CoursesController(IMediator mediator) : base(mediator) { }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllCoursesQuery()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetCourseByIdQuery { Id = id }));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create([FromBody] CreateCourseCommand command)
        {
            var courseDto = await _mediator.Send(command);
            return courseDto;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCourseCommand command)
        {
            if (id != command.courseDto.Id)
            {
                return BadRequest("Course ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteCourseCommand { Id = id });
        }
    }
}