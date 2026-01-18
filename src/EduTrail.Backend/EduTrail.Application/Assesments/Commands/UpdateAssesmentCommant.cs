using AutoMapper;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class UpdateAssesmentCommant : IRequest<AssesmentDto>
    {
        public class Handler : IRequestHandler<UpdateAssesmentCommant, AssesmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public Task<AssesmentDto> Handle(UpdateAssesmentCommant request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}