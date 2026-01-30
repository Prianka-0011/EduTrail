using EduTrail.Domain.Interfaces;

namespace EduTrail.Application.Types
{
    public class QuestionTypeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }   // MCQ, TEXT, ORDERING
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
