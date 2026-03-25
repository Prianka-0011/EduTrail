using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.UserDashboards
{
    public class CurrentUserMappingsProfile : Profile
    {
        public CurrentUserMappingsProfile()
        {
          CreateMap<CurrentLoginUserDto, User>()
                .ForMember(d => d.Roles, o => o.Ignore());

            CreateMap<User, CurrentLoginUserDto>()
                .ForMember(d => d.Roles,
                           o => o.MapFrom(s => s.Roles.Select(r => new DropdownItemDto
                           {
                               Id = r.Id,
                               Name = r.Name
                           })))
                           .ForMember(d => d.FullName, o => o.MapFrom(s => s.FirstName+" "+s.LastName));
        }
    }
}