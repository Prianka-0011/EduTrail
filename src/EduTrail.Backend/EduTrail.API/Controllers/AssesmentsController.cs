using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentsController : BaseController
    {
        public AssesmentsController(IMediator mediator) : base(mediator)
        {
        }
        public async Task<>
    }
}