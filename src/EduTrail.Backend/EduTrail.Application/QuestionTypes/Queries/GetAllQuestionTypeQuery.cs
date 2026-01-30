using AutoMapper;
using MediatR;
using Microsoft.VisualBasic;

namespace EduTrail.Application.QuestionTypes
{
    public class GetAllQuestionTypeQuery : IRequest<List<QuestionTypeDetailDto>>
    {
        public class Handler : IRequestHandler<GetAllQuestionTypeQuery, List<QuestionTypeDetailDto>>
        {
            private readonly IQuestionTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<QuestionTypeDetailDto>> Handle(GetAllQuestionTypeQuery request, CancellationToken cancellationToken)
            {
                var entities = await _repository.GetAllAsync();
                var list =_mapper.Map<List<QuestionTypeDetailDto>>(entities);
                return list;

            }
        }
    }
}