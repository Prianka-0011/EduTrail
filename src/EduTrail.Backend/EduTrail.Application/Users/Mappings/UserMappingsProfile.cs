using AutoMapper;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Users
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            CreateMap<UserDetailDto, User>();
            CreateMap<User, UserDetailDto>();
        }
    }
}