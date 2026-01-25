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
                IEnumerable<Domain.Entities.Assessment> entities = new List<Domain.Entities.Assessment>();
                try
                {
                    entities = await _repository.GetAllAsync();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
                var results = _mapper.Map<List<AssessmentDetailDto>>(entities);
                return results;
            }
        }
    }
}