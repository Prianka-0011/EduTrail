namespace EduTrail.Application.Courses
{
    public class CourseDto
    {
        public Guid? Id { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? Institute { get; set; }
        public string? TimeZone { get; set; }
    }
}