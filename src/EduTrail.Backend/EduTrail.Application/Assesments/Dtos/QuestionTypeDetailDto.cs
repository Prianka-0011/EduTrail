namespace EduTrail.Application.QuestionTypes
{
    public class QuestionTypeDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? AvailableCredit { get; set; } = 100;
        public int? MaxScore { get; set; } = 0;
        public Guid? CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTimeOffset OpenDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}