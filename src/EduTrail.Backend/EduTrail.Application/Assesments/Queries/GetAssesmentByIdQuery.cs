using AutoMapper;
using EduTrail.Application.CourseOfferings;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAssesmentByIdQuery : IRequest<AssesmentDto>
    {
        private Guid Id {get; set;}
        public class Handler : IRequestHandler<GetAssesmentByIdQuery, AssesmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
            }

            public async Task<AssesmentDto> Handle(GetAssesmentByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var dto = _mapper.Map<AssesmentDto>(entity);
                return dto;
            }
        }
    }
}