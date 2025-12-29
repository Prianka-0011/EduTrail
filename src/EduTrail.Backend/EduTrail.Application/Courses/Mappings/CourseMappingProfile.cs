using AutoMapper;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Courses
{
   public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        CreateMap<CourseDto, Course>();
            // .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 

        CreateMap<Course, CourseDto>();
    }
}

}