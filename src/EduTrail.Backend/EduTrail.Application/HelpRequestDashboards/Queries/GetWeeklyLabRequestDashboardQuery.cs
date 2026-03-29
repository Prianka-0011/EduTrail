using AutoMapper;
using MediatR;

using System.Globalization;

namespace EduTrail.Application.HelpRequestDashboards
{
    public class GetWeeklyLabRequestDashboardQuery : IRequest<WeeklyLabRequestDto>
    {
        public class Handler : IRequestHandler<GetWeeklyLabRequestDashboardQuery, WeeklyLabRequestDto>
        {
            private readonly IHelpRequestDashboardRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IHelpRequestDashboardRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<WeeklyLabRequestDto> Handle(GetWeeklyLabRequestDashboardQuery request, CancellationToken cancellationToken)
            {
                var labRequests = await _repository.GetAllAsync();

                var weeklyData = labRequests
                    .GroupBy(x => new
                    {
                        Year = x.RequestedDate.Year,
                        Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                            x.RequestedDate.DateTime,
                            CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday)
                    })
                    .Select(g =>
                    {
                        var weekStart = g.Min(x => x.RequestedDate).Date;

                        return new WeeklyLabRequestDetailDto
                        {
                            Week = $"Week {g.Key.Week}",
                            WeekStartDate = weekStart,
                            Monday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Monday),
                            Tuesday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Tuesday),
                            Wednesday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Wednesday),
                            Thursday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Thursday),
                            Friday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Friday),
                            Saturday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Saturday),
                            Sunday = g.Count(x => x.RequestedDate.DayOfWeek == DayOfWeek.Sunday),
                            Total = g.Count()
                        };
                    })
                    .OrderBy(x => x.WeekStartDate)
                    .ToList();

                return new WeeklyLabRequestDto { WeeklyDataList = weeklyData };
            }
        }
    }
}