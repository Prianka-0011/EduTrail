using EduTrail.Application.Courses;
using EduTrail.Application.Enrolements;
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
        
        

    }
}