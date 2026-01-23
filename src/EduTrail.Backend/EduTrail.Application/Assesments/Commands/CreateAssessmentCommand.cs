using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Assessments
{
    public class CreateAssessmentCommand : IRequest<AssessmentDetailDto>
    {
        public AssessmentDetailDto AssessmentDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateAssessmentCommand, AssessmentDetailDto>
        {
            private readonly IAssessmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssessmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<AssessmentDetailDto> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Assessment>(request.AssessmentDetailDto);
                var result = await _repository.CreateAsync(entity);
                var assessmentDto = _mapper.Map<AssessmentDetailDto>(result);
                return assessmentDto;
            }
        }
    } 
}