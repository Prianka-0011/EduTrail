using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Enrolements
{
    public class EnrolementMappingsProfile : Profile
    {
        public EnrolementMappingsProfile()
        {
            CreateMap<EnrolementDetailsDto, Enrollment>()
            // .ForMember(d=>d.Id, o=>o.Ignore())
            .ForMember(d=>d.CourseOffering, o=>o.Ignore())
            .ForMember(d=>d.Student, o=>o.Ignore());

            CreateMap<Enrollment, EnrolementDetailsDto>()
            .ForMember(d=>d.CourseOfferingId, o=>o.MapFrom(s=>s.CourseOfferingId))
            .ForMember(d=>d.StudentId, o=>o.MapFrom(s=>s.StudentId))
            .ForMember(d=>d.StudentName, o=>o.MapFrom(s=>s.Student != null ? s.Student.FirstName + " " + s.Student.LastName : null));
        }
    }
}