using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class GetRequestByIdQuery : IRequest<HelpRequestDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetRequestByIdQuery, HelpRequestDto>
        {
            private readonly ICommonService _service;
            private readonly ILabRequestRepository _repository;
            public Handler(ILabRequestRepository repository, ICommonService service)
            {
                _repository = repository;
                _service = service;
            }
            public async Task<HelpRequestDto> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
            {
                var res = await _repository.GetLabRequestById(request.Id);
                var dto = _service._Mapper.Map<HelpRequestDetailDto>(res);
                var statusList = await _service._ConfigurationRepository.GetStatusListByTypeId(CustomCategory.StatusType.HelpRequestStatusType);

                return new HelpRequestDto
                {
                    DetailsListDto = null,
                    DetailsDto = dto,
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