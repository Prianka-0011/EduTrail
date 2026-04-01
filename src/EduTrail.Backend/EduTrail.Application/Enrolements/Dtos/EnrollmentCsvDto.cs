using Microsoft.AspNetCore.Http;

namespace EduTrail.Application.Enrolements
{
     public class EnrollmentBulkCsvDto
    {
        public Guid CourseOfferingId { get; set; }
        public IFormFile File { get; set; }
        public bool IsTa { get; set; }
    }
}

public class EnrollmentCsvDto
{
    public string FullName { get; set; }
    public string CanvasUserId { get; set; }
    public string Email { get; set; }
    public string SISId { get; set; }
}
