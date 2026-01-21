using AutoMapper;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAllAssessmentQuery : IRequest<List<AssessmentDetailDto>>
    {
        public class Handler : IRequestHandler<GetAllAssessmentQuery, List<AssessmentDetailDto>>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<AssessmentDetailDto>> Handle(GetAllAssessmentQuery request, CancellationToken cancellationToken)
            {
                var entities = await _repository.GetAllAsync();
                var results = _mapper.Map<List<AssessmentDetailDto>>(entities);
                return results;
            }
        }
    }
}