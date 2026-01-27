using AutoMapper;
using EduTrail.Application.CourseOfferings;
using MediatR;

namespace EduTrail.Application.Assessments
{
    public class GetAssessmentByIdQuery : IRequest<AssessmentDetailDto>
    {
        public Guid Id {get; set;}
        public class Handler : IRequestHandler<GetAssessmentByIdQuery, AssessmentDetailDto>
        {
            private readonly IAssessmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssessmentRepository repository, IMapper mapper)
            {
                _repository = repository;
            }

            public async Task<AssessmentDetailDto> Handle(GetAssessmentByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var dto = _mapper.Map<AssessmentDetailDto>(entity);
                return dto;
            }
        }
    }
}