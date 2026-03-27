
using System.Security.Claims;
using EduTrail.Application.Assessments;
using EduTrail.Application.Auths;
using EduTrail.Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseController
    {
        public ChatController(IMediator mediator) : base(mediator)
        {
        }
       
    }
}