namespace EduTrail.Application.Questions
{
     public class GeneratedQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string DisplayText { get; set; }
        public List<GeneratedCodeLineDto> Lines { get; set; }
        public Dictionary<string, string> Parameters { get; set; } // What values were substituted
    }
}