namespace EduTrail.Application.Questions
{
    public class QuestionDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Template { get; set; } // "What is {a} + {b}?"
    }
}
