using AutoMapper;
using MediatR;

namespace EduTrail.Application.Assessments
{
    public class GetAllAssessmentQuery : IRequest<List<AssessmentDetailDto>>
    {
        public class Handler : IRequestHandler<GetAllAssessmentQuery, List<AssessmentDetailDto>>
        {
            private readonly IAssessmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssessmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<AssessmentDetailDto>> Handle(GetAllAssessmentQuery request, CancellationToken cancellationToken)
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<List<AssessmentDetailDto>>(entities);

            }
        }
    }
}