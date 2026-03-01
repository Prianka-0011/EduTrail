using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Terms
{
    public class TermMappingsProfile : Profile
    {
        public TermMappingsProfile()
        {
            CreateMap<TermDetailDto, Term>()
                .ForMember(dest => dest.TermTypeId, opt => opt.MapFrom(src => src.TermTypeId));
            CreateMap<Term, TermDetailDto>()
                .ForMember(dest => dest.TermTypeId, opt => opt.MapFrom(src => src.TermTypeId));
        }
    }
}