using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class GetAllLabRequestQuery : IRequest<HelpRequestDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllLabRequestQuery, HelpRequestDto>
        {
            private readonly ICommonService _service;
            private readonly ILabRequestRepository _repository;
            public Handler(ILabRequestRepository repository, ICommonService service)
            {
                _repository = repository;
                _service = service;
            }
            public async Task<HelpRequestDto> Handle(GetAllLabRequestQuery request, CancellationToken cancellationToken)
            {
                var res = await _repository.GetAllLabRequestByCourseOfferingAsync(request.CourseOfferingId);
                var dtos = _service._Mapper.Map<List<HelpRequestDetailDto>>(res);
                var statusList = await _service._ConfigurationRepository.GetStatusListByTypeId(CustomCategory.StatusType.HelpRequestStatusType);

                return new HelpRequestDto
                {
                    DetailsListDto = dtos,
                    DetailsDto = null,
                    StatusList = statusList.Select(c=> new DropdownItemDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                };
            }
        }
    }
}