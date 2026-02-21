using AutoMapper;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class GetAllEnrolementsQuery : IRequest<List<EnrolementDto>>
    {
        public class Handler : IRequestHandler<GetAllEnrolementsQuery, List<EnrolementDto>>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<EnrolementDto>> Handle(GetAllEnrolementsQuery request, CancellationToken cancellationToken)
            {
                var enrolements = await _repository.GetAllAsync();
                var enrolementDtos = _mapper.Map<List<EnrolementDto>>(enrolements);
                return enrolementDtos;
            }
        }
    }
}