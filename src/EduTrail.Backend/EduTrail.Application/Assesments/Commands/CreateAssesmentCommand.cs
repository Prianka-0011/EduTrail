using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class CreateAssesmentCommand : IRequest<AssesmentDto>
    {
        public AssesmentDto AssesmentDto { get; set; }
        public class Handler : IRequestHandler<CreateAssesmentCommand, AssesmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<AssesmentDto> Handle(CreateAssesmentCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Assessment>(request.AssesmentDto);
                var result = await _repository.CreateAsync(entity);
                var assesmentDto = _mapper.Map<AssesmentDto>(result);
                return assesmentDto;
            }
        }
    } 
}