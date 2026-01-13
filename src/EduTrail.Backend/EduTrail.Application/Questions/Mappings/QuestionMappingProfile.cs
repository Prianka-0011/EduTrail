using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Questions
{
    public class QuestionMappingProfile : Profile
    {
        public QuestionMappingProfile()
        {
             CreateMap<Question, QuestionDetailDto>()
                .ForMember(dest => dest.Template, opt => opt.MapFrom(src => src.VariantTemplates.FirstOrDefault().Template))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.VariantTemplates.FirstOrDefault().Language))
                .ForMember(dest => dest.VariationRules, opt => opt.MapFrom(src => src.VariationRules));

            // DTO → Entity
            CreateMap<QuestionDetailDto, Question>()
                .ForMember(dest => dest.VariantTemplates, opt => opt.Ignore()) // handled manually in handler
                .ForMember(dest => dest.VariationRules, opt => opt.Ignore()); 
        }
    }
}