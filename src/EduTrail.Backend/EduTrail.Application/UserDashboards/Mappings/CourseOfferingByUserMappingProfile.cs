using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Application.QuestionTypes;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.UserDashboards
{
    public class CourseOfferingByUserMappingProfile : Profile
    {
        public CourseOfferingByUserMappingProfile()
        {

              CreateMap<UserCourseOfferingDetail, CourseOffering>()
                // .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Course, o => o.Ignore())
                .ForMember(d => d.Term, o => o.Ignore())
                .ForMember(d => d.Instructor, o => o.Ignore())
                .ForMember(d => d.Assessments, o => o.Ignore())
                .ForMember(d => d.Enrollments, o => o.Ignore());

            CreateMap<CourseOffering, UserCourseOfferingDetail>()
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName + " (" + s.Course.CourseCode + ")"))
                .ForMember(d => d.TermName, o => o.MapFrom(s => s.Term.Name + " " + s.Term.Year))
                .ForMember(d => d.InstructorName, o => o.MapFrom(s => s.Instructor != null ? s.Instructor.FirstName + " " + s.Instructor.LastName : null));
        }
    }

}