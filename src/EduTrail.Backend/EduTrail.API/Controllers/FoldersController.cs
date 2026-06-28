using EduTrail.Application.Folders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : BaseController
    {
        public FoldersController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<FolderDto>> GetAll(Guid? courseOfferingId)
        {
            return Ok(await _mediator.Send(new GetAllFoldersQuery { CourseOfferingId = courseOfferingId }));
        }

        [Authorize]
        [HttpGet("subfolders")]
        public async Task<ActionResult<FolderDto>> GetAllSubFolder([FromQuery] Guid? parentId)
        {
            var result = await _mediator.Send(new GetAllSubFoldersQuery
            {
                ParentId = parentId
            });

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FolderDto>> Create([FromBody] CreateFolderCommand command)
        {
            var courseDto = await _mediator.Send(command);
            return courseDto;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<FolderDetailsDto>> Update(Guid id, [FromBody] UpdateFolderCommand command)
        {
            command.Folder.Id = id;

            var folderDto = await _mediator.Send(command);

            return Ok(folderDto);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<Guid> folderIds)
        {
            var command = new DeleteFoldersCommand
            {
                FolderIds = folderIds
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}