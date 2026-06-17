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
            return Ok(await _mediator.Send(new GetAllFoldersQuery {CourseOfferingId = courseOfferingId}));
        }

    }
}