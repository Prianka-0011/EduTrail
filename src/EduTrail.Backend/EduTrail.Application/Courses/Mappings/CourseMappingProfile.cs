using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Courses
{
   public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<QuestionDto, Question>();
            // .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 

        CreateMap<Question, QuestionDto>();
    }
}

}