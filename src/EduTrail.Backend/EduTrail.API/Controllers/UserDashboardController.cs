using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    public class UserDashboardController : BaseController
    {
        public UserDashboardController(IMediator mediator) : base(mediator)
        {
        }
        // [HttpGet("{userId}")]
        // public async Task<ActionResult<string[]>>GetAllCoursesByUser(Guid userId)
        // {
            
        // }
    }
}