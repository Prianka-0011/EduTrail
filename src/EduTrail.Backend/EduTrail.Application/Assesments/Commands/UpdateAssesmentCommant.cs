using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class UpdateAssesmentCommant : IRequest<AssesmentDto>
    {
        public AssesmentDto AssesmentDto { get; set; }
        public class Handler : IRequestHandler<UpdateAssesmentCommant, AssesmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<AssesmentDto> Handle(UpdateAssesmentCommant request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Assessment>(request.AssesmentDto);
                var result = await _repository.UpdateAsync(entity);
                var assesmentDto = _mapper.Map<AssesmentDto>(result);
                return assesmentDto;
            }
        }
    }
}