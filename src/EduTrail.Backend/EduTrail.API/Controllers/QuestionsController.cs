// using EduTrail.Application.Questions;
// using Microsoft.AspNetCore.Mvc;
// using MediatR;
// namespace EduTrail.API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class QuestionsController : BaseController
//     {
//         public QuestionsController(IMediator mediator) : base(mediator) { }
//         public async Task<IActionResult> Create([FromBody] QuestionDto questionDto)
//         {
//             var result = await _mediator.Send(new CreateQuestionCommand { questionDto = questionDto });
//             return Ok(result);
//         }
//     }
// }