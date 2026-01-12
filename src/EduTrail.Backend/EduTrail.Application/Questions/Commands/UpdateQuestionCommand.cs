using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class UpdateQuestionCommand : IRequest<QuestionDetailDto>
    {
        public QuestionDetailDto questionDetailDto { get; set; }
        public class Handler : IRequestHandler<UpdateQuestionCommand, QuestionDetailDto>
        {
            private readonly IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<QuestionDetailDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Question>(request.questionDetailDto);
                var result = await _repository.UpdateAsync(entity);
                var questionDetailDto = _mapper.Map<QuestionDetailDto>(result);
                return questionDetailDto;
            }
        }
    }
}