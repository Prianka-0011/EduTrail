using AutoMapper;
using EduTrail.Application.Enrolements;
using EduTrail.Domain.Entities;
using EduTrail.Application.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace EduTrail.Application.UserDashboards
{
    public class EnrollementByUserMappingProfile : Profile
    {
        public EnrollementByUserMappingProfile()
        {
            // Entity → DTO
            CreateMap<Enrollment, UserEnrollementDetailsDto>()
                .ForMember(d => d.UserName,
                    o => o.MapFrom(s => s.User != null 
                        ? s.User.FirstName + " " + s.User.LastName 
                        : null))
                .ForMember(d => d.TermTypeId, o => o.MapFrom(s => s.CourseOffering.Term.TermTypeId))
                .ForMember(d => d.IsTa, o => o.Ignore()) // You can set manually later
                .ForMember(d => d.Months, o => o.MapFrom(s => s.TALabMonths))
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.User != null 
                    ? s.User.Roles.Select(r => new DropdownItemDto { Id = r.Id, Name = r.Name }) 
                    : new List<DropdownItemDto>()));

            // DTO → Entity
            CreateMap<UserEnrollementDetailsDto, Enrollment>()
                .ForMember(d => d.CourseOffering, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore()) // Roles must be set manually
                .ForMember(d => d.TALabMonths, o => o.Ignore());

            // TALab mapping
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
