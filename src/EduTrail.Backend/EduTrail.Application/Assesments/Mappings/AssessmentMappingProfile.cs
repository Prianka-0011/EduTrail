using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Assessments
{
    public class AssessmentMappingProfile : Profile
    {
        public AssessmentMappingProfile()
        {

            CreateMap<AssessmentDetailDto, Assessment>();
            CreateMap<Assessment, AssessmentDetailDto>();
        }
    }

}