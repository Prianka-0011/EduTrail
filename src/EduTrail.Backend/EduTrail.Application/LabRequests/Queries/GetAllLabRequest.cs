using AutoMapper;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class GetAllLabRequest : IRequest<HelpRequestDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllLabRequest, HelpRequestDto>
        {
            private readonly IMapper _mapper;
            private readonly ILabRequestRepository _repository;
            public Handler(ILabRequestRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<HelpRequestDto> Handle(GetAllLabRequest request, CancellationToken cancellationToken)
            {
                var res = await _repository.GetAllLabRequestByCourseOfferingAsync(request.CourseOfferingId);
                var dtos = _mapper.Map<List<HelpRequestDetailDto>>(res);
                return new HelpRequestDto { DetailsListDto = dtos, DetailsDto = null };
            }
        }
    }
}