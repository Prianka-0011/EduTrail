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
        [HttpPost("vote")]
        public async Task<ActionResult<PollVoteDto>> PollVote([FromBody] PollVoteCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("poll/{pollId}/results")]
        public async Task<ActionResult> GetPollResults(Guid pollId, [FromQuery] Guid courseOfferingId)
        {
            return Ok(await _mediator.Send(new GetPollResultsQuery
            {
                PollId = pollId,
                CourseOfferingId = courseOfferingId

            }));

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
        [HttpPut("{id}/archive")]
        public async Task<ActionResult<bool>> Archive(Guid id)
        {
            var result = await _mediator.Send(new ArchivePostCommand
            {
                PostId = id
            });

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}/pinned")]
        public async Task<ActionResult<bool>> Pinned(Guid id, bool isPinned)
        {
            var result = await _mediator.Send(new PinPostCommand
            {
                PostId = id,
                IsPinned = isPinned
            });

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}/readed")]
        public async Task<ActionResult<bool>> Readed(Guid id, bool isReaded)
        {
            var result = await _mediator.Send(new ReadOrUnreadPostCommand
            {
                PostId = id,
                IsRead = isReaded
            });

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}/favorite")]
        public async Task<ActionResult<bool>> Favorite(Guid id, bool IsFavorite)
        {
            var result = await _mediator.Send(new IsFavoritePostCommand
            {
                PostId = id,
                IsFavorite = IsFavorite
            });

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            return await _mediator.Send(new DeletePostCommand { Id = id });
        }
    }
}