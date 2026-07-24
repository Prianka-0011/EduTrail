using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authorization;
using EduTrail.Application.Posts;
namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : BaseController
    {
        public PostsController(IMediator mediator) : base(mediator) { }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] Guid courseOfferingId)
        {
            return Ok(await _mediator.Send(new GetAllPostQuery
            {
                CourseOfferingId = courseOfferingId
            }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id, [FromQuery] Guid courseOfferingId)
        {
            return Ok(await _mediator.Send(new GetPostByIdQuery
            {
                Id = id,
                CourseOfferingId = courseOfferingId
            }));
        }

        [Authorize]
        [HttpGet("view/{id}")]
        public async Task<ActionResult> GetViewById(Guid id)
        {
            return Ok(await _mediator.Send(new GetPostViewByIdQuery
            {
                Id = id,
                
            }));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostCommand command)
        {
            var postDto = await _mediator.Send(command);
            return postDto;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdatePostCommand command)
        {
            if (id != command.PostDetailDto.Id)
            {
                return BadRequest("Post ID mismatch");
            }
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            return await _mediator.Send(new DeletePostCommand { Id = id });
        }
    }
}