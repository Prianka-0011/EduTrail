using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Enrolements
{
    public class EnrolementDto
    {
        public EnrolementDetailsDto DetailsDto { get; set; }
        public List<EnrolementDetailsDto> DetailsDtoList { get; set; }
        public List<DropdownItemDto> Students { get; set; }
    }
    public class EnrolementDetailsDto
    {
        public Guid Id { get; set; }
        public Guid ?CourseOfferingId { get; set; }
        public Guid? StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTimeOffset? EnrolementDate { get; set; }
        public bool? IsActive { get; set; }
    }

}

