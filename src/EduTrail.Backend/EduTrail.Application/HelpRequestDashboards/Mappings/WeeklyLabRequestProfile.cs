using AutoMapper;
using EduTrail.Application.HelpRequestDashboards;

public class WeeklyLabRequestProfile : Profile
{
    public WeeklyLabRequestProfile()
    {
        CreateMap<WeeklyLabRequestDetailDto, WeeklyLabRequestDetailDto>();
    }
}