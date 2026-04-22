using EduTrail.Application.HelpRequestDashboards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpRequestDashboardsController : BaseController
    {
        public HelpRequestDashboardsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("weekly-lab-requests")]
        public async Task<IActionResult> GetWeeklyLabRequests()
        {
            var query = new GetWeeklyLabRequestDashboardQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}