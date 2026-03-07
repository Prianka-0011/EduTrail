using AutoMapper;
using EduTrail.Application.Enrolements;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.UserDashboards
{
    public class EnrollementByUserMappingProfile : Profile
    {
        public EnrollementByUserMappingProfile()
        {
            CreateMap<Enrollment, UserEnrollementDetailsDto>()
                .ForMember(d => d.StudentName,
                    o => o.MapFrom(s =>
                        s.Student != null
                            ? s.Student.FirstName + " " + s.Student.LastName
                            : null))
                .ForMember(d => d.TermTypeId, o => o.MapFrom(s => s.CourseOffering.Term.TermTypeId))
                .ForMember(d => d.IsTa, o => o.Ignore())
                .ForMember(d => d.Months, o => o.MapFrom(s => s.TALabMonths));

            CreateMap<UserEnrollementDetailsDto, Enrollment>()
                .ForMember(d => d.CourseOffering, o => o.Ignore())
                .ForMember(d => d.Student, o => o.Ignore())
                .ForMember(d => d.TALabMonths, o => o.Ignore());
            
            CreateMap<TALabMonth, TALabMonthDto>()
                .ForMember(d => d.Weeks, o => o.MapFrom(s => s.Weeks));

            CreateMap<TALabWeek, TALabWeekDto>()
                .ForMember(d => d.Days, o => o.MapFrom(s => s.Days));

            CreateMap<TALabDay, TALabDayDto>()
                .ForMember(d => d.Slots, o => o.MapFrom(s => s.Slots));

            CreateMap<TALabSlot, TALabSlotDto>();
        }
    }

}