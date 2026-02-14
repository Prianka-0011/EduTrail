using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.CourseOfferings
{
    public class CourseOfferingDto
    {
        public CourseOfferingDetailDto DetailDto { get; set; }
        public List<DropdownItemDto> Courses { get; set; } = new List<DropdownItemDto>();
        public List<DropdownItemDto> Terms { get; set; } = new List<DropdownItemDto>();
        public List<DropdownItemDto> Instructors { get; set; } = new List<DropdownItemDto>();
    }
    public class CourseOfferingDetailDto
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