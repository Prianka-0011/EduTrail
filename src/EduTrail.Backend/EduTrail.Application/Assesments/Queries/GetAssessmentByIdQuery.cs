using AutoMapper;
using EduTrail.Application.CourseOfferings;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAssessmentByIdQuery : IRequest<AssessmentDto>
    {
        private Guid Id {get; set;}
        public class Handler : IRequestHandler<GetAssessmentByIdQuery, AssessmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
            }

            public async Task<AssessmentDto> Handle(GetAssessmentByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var dto = _mapper.Map<AssessmentDto>(entity);
                return dto;
            }
        }
    }
}