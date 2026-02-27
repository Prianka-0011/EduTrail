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
        public Guid? CourseOfferingId { get; set; }
        public Guid? StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTimeOffset? EnrolledDate { get; set; }
        public decimal? TotalWorkHoursPerWeek { get; set; } = 10;
        public bool? IsActive { get; set; }
        public bool? IsTa { get; set; }
        public List<TALabMonthDto> Months { get; set; } = new List<TALabMonthDto>();
    }
    public class TALabMonthDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; } // 1–12
        public int Year { get; set; }
        public Guid EnrollmentId { get; set; }
        public List<TALabWeekDto> Weeks { get; set; } = new List<TALabWeekDto>();
    }
    public class TALabWeekDto
    {
        public Guid Id { get; set; }
        public int WeekNumber { get; set; }
          public Guid TALabMonthId { get; set; }
        public List<TALabDayDto> Days { get; set; } = new List<TALabDayDto>();
    }
    public class TALabDayDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset LabDate { get; set; }
        public Guid TALabWeekId { get; set; }
        public bool IsActive { get; set; }
        public List<TALabSlotDto> Slots { get; set; } = new List<TALabSlotDto>();
    }
    public class TALabSlotDto
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Guid TALabDayId { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string? RemoteLink { get; set; }
    }

}

