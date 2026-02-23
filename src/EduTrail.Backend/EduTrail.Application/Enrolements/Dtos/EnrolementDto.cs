using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Enrolements
{
    public class EnrolementDto
    {
        public EnrolementDetailsDto DetailsDto { get; set; } = new EnrolementDetailsDto();
        public List<EnrolementDetailsDto>? DetailsDtoList { get; set; } = new List<EnrolementDetailsDto>();
        public List<DropdownItemDto>? Users { get; set; } = new List<DropdownItemDto>();
    }
    public class EnrolementDetailsDto
    {
        public Guid Id { get; set; }
        public Guid ?CourseOfferingId { get; set; }
        public Guid? StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTimeOffset? EnrollmentDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTa { get; set; }
    }

}

