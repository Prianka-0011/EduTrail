using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class GetAllLabRequestByCurrentUserQuery : IRequest<HelpRequestDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllLabRequestByCurrentUserQuery, HelpRequestDto>
        {
            private readonly ICommonService _service;
            private readonly ILabRequestRepository _repository;
            public Handler(ILabRequestRepository repository, ICommonService service)
            {
                _repository = repository;
                _service = service;
            }
            public async Task<HelpRequestDto> Handle(GetAllLabRequestByCurrentUserQuery request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = _service._CurrentUserService.GetUserId();

                var enrolement = await _repository.GetEnrollementByUserId(currentLoginUserId);

                var res = await _repository
                    .GetAllLabRequestByCourseOfferingAsync(request.CourseOfferingId);
                var completedStatusId = CustomCategory.HelpRequestStatus.Completed;
                var grouped = res
                            .GroupBy(x => x.RequestedDate.Date)
                            .ToList();

                var resultList = new List<HelpRequestDetailDto>();

                foreach (var group in grouped)
                {
                    var ordered = group
                        .OrderBy(x => x.RequestedDate)
                        .ThenBy(x => x.Id)
                        .ToList();

                    int counter = 1;

                    foreach (var item in ordered)
                    {
                        var dto = _service._Mapper.Map<HelpRequestDetailDto>(item);

                        dto.DailyNumber = item.StatusId == completedStatusId ? 0 : counter++;

                        resultList.Add(dto);
                    }
                }
                resultList = resultList.Where(c => c.StudentId == enrolement.Id).ToList();

                return new HelpRequestDto
                {
                    DetailsListDto = resultList,
                    DetailsDto = null
                };
            }

        }
    }
}