using EduTrail.Application.Enrolements;
using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.UserDashboards
{
    public class UserEnrollementDto
    {
        public List<DropdownItemDtoInt>? DropdownMonths { get; set; } = new List<DropdownItemDtoInt>();
        public UserEnrollementDetailsDto DetailsDto { get; set; }
        public List<UserEnrollementDetailsDto>? DetailsListDto { get; set; }
        public int Year { get; set; }
    }

    public class UserEnrollementDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CourseOfferingId { get; set; }
        public Guid TermTypeId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTimeOffset EnrolledDate { get; set; }
        public decimal? TotalWorkHoursPerWeek { get; set; } = 10;
        public bool? IsActive { get; set; }
        public bool? IsTa { get; set; }
        public List<TALabMonthDto> Months { get; set; } = new List<TALabMonthDto>();
    }
}