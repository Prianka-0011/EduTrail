using AutoMapper;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAllAssesmentQuery : IRequest<List<AssesmentDetailDto>>
    {
        public class Handler : IRequestHandler<GetAllAssesmentQuery, List<AssesmentDetailDto>>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<AssesmentDetailDto>> Handle(GetAllAssesmentQuery request, CancellationToken cancellationToken)
            {
                var entities = await _repository.GetAllAsync();
                var results = _mapper.Map<List<AssesmentDetailDto>>(entities);
                return results;
            }
        }
    }
}