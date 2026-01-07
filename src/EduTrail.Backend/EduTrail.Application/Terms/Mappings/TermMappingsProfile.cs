using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Terms
{
    public class TermMappingsProfile : Profile
    {
        public TermMappingsProfile()
        {
            CreateMap<TermDto, Term>();
            CreateMap<Term, TermDto>();
        }
    }
}