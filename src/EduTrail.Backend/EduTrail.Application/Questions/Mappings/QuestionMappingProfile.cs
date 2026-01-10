using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Questions
{
    public class QuestionMappingProfile : Profile
    {
        public QuestionMappingProfile()
        {
            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionDto>();
        }
    }
}