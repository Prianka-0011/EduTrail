using AutoMapper;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class GetQuestionByIdQuery : IRequest<QuestionDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
        {
            private readonly  IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var questionDto = _mapper.Map<QuestionDto>(entity);
                return questionDto;
            }
        }
    }
}