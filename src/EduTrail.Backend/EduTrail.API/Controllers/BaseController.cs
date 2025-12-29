using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace EduTrail.API.Controllers
{
     [ApiController]
    public abstract class BaseController: ControllerBase
    {
        protected readonly IMediator _mediator;
        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}