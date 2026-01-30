namespace EduTrail.Application.QuestionTypes
{
    public class QuestionTypeDetailDto
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }   // MCQ, TEXT, ORDERING
        public string? Name { get; set; }
        public string Description { get; set; }
    }
}