using AutoMapper;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class GetAllEnrolementsQuery : IRequest<EnrolementDto>
    {
        public Guid? CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllEnrolementsQuery, EnrolementDto>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<EnrolementDto> Handle(GetAllEnrolementsQuery request, CancellationToken cancellationToken)
            {
                var enrolements = await _repository.GetAllAsync(request.CourseOfferingId);
                var enrolementDtos = _mapper.Map<List<EnrolementDetailsDto>>(enrolements);
                return new EnrolementDto { DetailsDtoList = enrolementDtos };
            }
        }
    }
}