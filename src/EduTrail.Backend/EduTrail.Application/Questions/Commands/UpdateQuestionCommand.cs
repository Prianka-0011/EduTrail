using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class UpdateQuestionCommand : IRequest<QuestionDto>
    {
        public QuestionDto questionDto { get; set; }
        public class Handler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
        {
            private readonly IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Question>(request.questionDto);
                var result = await _repository.UpdateAsync(entity);
                var questionDto = _mapper.Map<QuestionDto>(result);
                return questionDto;
            }
        }
    }
}