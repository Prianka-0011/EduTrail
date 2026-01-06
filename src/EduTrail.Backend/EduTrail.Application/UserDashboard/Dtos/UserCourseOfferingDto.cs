using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.UserDashboard
{

    public class UserCourseOfferingDto
    {

        public UserCourseOfferingDetail CourseOfferingDetail { get; set; }
        public List<DropdownItemDto> Courses { get; set; }
        public List<DropdownItemDto> Instructors { get; set; }
        public List<DropdownItemDto> Terms { get; set; }
    }
    public class UserCourseOfferingDetail
    {
        public Guid? Id { get; set; }
        public Guid? CourseId { get; set; }
        public string? CourseName { get; set; }
        public Guid? InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public Guid? TermId { get; set; }
        public string? TermName { get; set; }
    }


}