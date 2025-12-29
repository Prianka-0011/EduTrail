using EduTrail.Application.Tests;
using Microsoft.AspNetCore.Mvc;
using MediatR;
namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public TestController(IMediator mediator) : base(mediator) { }
        public async Task<IActionResult> Create([FromBody] TestDto testDto)
        {
            var result = await _mediator.Send(new CreateTestCommand { testDto = testDto });
            return Ok(result);
        }
    }
}