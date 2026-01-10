namespace EduTrail.Application.Questions
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Template { get; set; }
        public string Language { get; set; }

        public string Title { get; set; }
        public List<VariationRuleDto>? VariationRules { get; set; }
    }
}
