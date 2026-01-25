using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Courses
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {

            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();
        }
    }

}