namespace EduTrail.Application.Assesments
{
    public class AssessmentDetailDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
    }
}