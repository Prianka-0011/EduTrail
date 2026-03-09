using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Application.QuestionTypes;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.UserDashboards
{
    public class LabRequestMappingProfile : Profile
    {
        public LabRequestMappingProfile()
        {
            CreateMap<HelpRequestDetailDto, LabRequest>();

            CreateMap<LabRequest, HelpRequestDetailDto>();
        }
    }
}