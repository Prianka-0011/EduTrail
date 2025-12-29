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

        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllCoursesQuery()));
        }

        
    }
}