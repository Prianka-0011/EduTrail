// namespace EduTrail.Application.Questions
// {
//     using MediatR;
//     using EduTrail.Application.Questions;
//     public class CreateQuestionCommand : IRequest<QuestionDto>
//     {
//         public QuestionDto questionDto { get; set; }
//         public class Handler : IRequestHandler<CreateQuestionCommand, QuestionDto>
//         {
//             private readonly IQuestionRepository _repository;
//             public Handler(IQuestionRepository repository)
//             {
//                 _repository = repository;
//             }
//             public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
//             {
//                 // var question = new Question();

//                 // question.Text = request.questionDto.Text;

//                 // question.Type = request.questionDto.Type;

//                 // question.Options = request.questionDto.Options;

//                 // question.CorrectAnswer = request.questionDto.CorrectAnswer;

//                 // var result = await _repository.Create(question);
                
//                 // var questionDto = new QuestionDto
//                 // {
//                 //     Id = result.Id,
//                 //     Text = result.Text,
//                 //     Type = result.Type,
//                 //     Options = result.Options,
//                 //     CorrectAnswer = result.CorrectAnswer
//                 // };

//                 // return questionDto;
//                 return new QuestionDto();
//             }
//         }
//     }
// }   