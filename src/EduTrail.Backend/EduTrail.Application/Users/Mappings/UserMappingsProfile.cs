using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Users
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
          CreateMap<UserDetailDto, User>()
                .ForMember(d => d.Roles, o => o.Ignore());

            CreateMap<User, UserDetailDto>()
                .ForMember(d => d.SelectedRoleList,
                           o => o.MapFrom(s => s.Roles.Select(r => new DropdownItemDto
                           {
                               Id = r.Id,
                               Name = r.Name
                           })));
        }
    }
}