using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssesmentController : BaseController
    {
        public AssesmentController(IMediator mediator) : base(mediator)
        {
        }
    }
}