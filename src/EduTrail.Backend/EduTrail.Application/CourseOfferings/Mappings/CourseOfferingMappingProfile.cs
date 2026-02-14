using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Application.QuestionTypes;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.CourseOfferings
{
    public class CourseOfferingMappingProfile : Profile
    {
        public CourseOfferingMappingProfile()
        {

            CreateMap<CourseOfferingDetailDto, CourseOffering>();
            CreateMap<QuestionType, QuestionTypeDetailDto>();
        }
    }

}