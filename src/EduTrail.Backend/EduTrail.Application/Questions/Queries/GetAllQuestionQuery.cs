using AutoMapper;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class GetAllQuestionQuery : IRequest<List<QuestionDetailDto>>
    {
        public class Handler : IRequestHandler<GetAllQuestionQuery, List<QuestionDetailDto>>
        {
            private IQuestionRepository _repository;
            private IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<QuestionDetailDto>> Handle(GetAllQuestionQuery request, CancellationToken cancellationToken)
            {
                var questions = await _repository.GetAllAsync();
                var questionDetailDto = _mapper.Map<List<QuestionDetailDto>>(questions);
                return questionDetailDto;
            }
        }
    }
}