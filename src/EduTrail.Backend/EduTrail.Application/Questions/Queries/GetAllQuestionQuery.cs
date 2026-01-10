using AutoMapper;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class GetAllQuestionQuery : IRequest<List<QuestionDto>>
    {
        public class Handler : IRequestHandler<GetAllQuestionQuery, List<QuestionDto>>
        {
            private IQuestionRepository _repository;
            private IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<QuestionDto>> Handle(GetAllQuestionQuery request, CancellationToken cancellationToken)
            {
                var questions = await _repository.GetAllAsync();
                var questionDtos = _mapper.Map<List<QuestionDto>>(questions);
                return questionDtos;
            }
        }
    }
}