using EduTrail.Application.Courses;
using EduTrail.Application.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : BaseController
    {
        public TypesController(IMediator mediator) : base(mediator)
        {
        }
    }
}