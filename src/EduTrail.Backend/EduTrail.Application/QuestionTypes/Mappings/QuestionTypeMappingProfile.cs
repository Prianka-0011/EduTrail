using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Application.QuestionTypes;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.QuestionTypes
{
    public class QuestionTypeMappingProfile : Profile
    {
        public QuestionTypeMappingProfile()
        {

            CreateMap<QuestionTypeDetailDto, QuestionType>();
            CreateMap<QuestionType, QuestionTypeDetailDto>();
        }
    }

}