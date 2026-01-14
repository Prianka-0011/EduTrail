using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Questions
{
    public class QuestionDto
    {
        public QuestionDetailDto Details { get; set; }
        public List<DropdownItemDto> Types { get; set; }
         public List<DropdownItemDto> Assesments { get; set; }
    }

    public class QuestionDetailDto
    {
        public Guid? Id { get; set; }
        public string? Template { get; set; }
        public string? Language { get; set; }
        public Guid QuestionTypeId { get; set; }
        public Guid AssessmentId { get; set; }
        public string? Title { get; set; }
        public List<VariationRuleDto>? VariationRules { get; set; }
    }
}
