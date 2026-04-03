using AutoMapper;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class GetAllActiveTAsQuery : IRequest<EnrolementDto>
    {
        public Guid? CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllActiveTAsQuery, EnrolementDto>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<EnrolementDto> Handle(GetAllActiveTAsQuery request, CancellationToken cancellationToken)
            {
                var enrolements = await _repository.GetAllActiveTAsAsync(request.CourseOfferingId, CustomCategory.RoleType.TA);
                var enrolementDtos = _mapper.Map<List<EnrolementDetailsDto>>(enrolements);
                return new EnrolementDto { DetailsDtoList = enrolementDtos };
            }
        }
    }
}