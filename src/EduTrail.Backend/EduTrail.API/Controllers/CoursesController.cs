using Microsoft.AspNetCore.Mvc;
using EduTrail.Application.Courses;
using MediatR;
using System.Xml.Serialization;
namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        public CoursesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllCoursesQuery()));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetById(Guid id)
        {
            return Ok(await _mediator.Send(new GetCourseByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateCourseCommand command)
        {
            var courseId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = courseId }, courseId);
        }
        [HttpPut]
        public async Task<ActionResult> Update(Guid id, UpdateCourseCommand command)
        {
            if (id != command.courseDto.Id)
            {
                return BadRequest("Course ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteCourseCommand { Id = id });
        }
    }
}