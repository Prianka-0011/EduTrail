using EduTrail.Domain.Interfaces;

namespace EduTrail.Application.Assesments
{
    public class AssesmentDto
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CourseId { get; set; }
        public DateTimeOffset OpenDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}
