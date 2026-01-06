namespace EduTrail.Application.CourseOfferings
{
    public class CourseOfferingDto
    {
        public Guid? Id { get; set; }
        public Guid? CourseId { get; set; }
        public string? CourseName { get; set; }
        public Guid? TermId { get; set; }    
        public string? TermName { get; set; }
        public Guid? InstructorId { get; set; }
        public string? InstructorName { get; set; }
       
        
    }
}