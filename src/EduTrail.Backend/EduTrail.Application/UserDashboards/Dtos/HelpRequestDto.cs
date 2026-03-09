using EduTrail.Application.Enrolements;
using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.UserDashboards
{
    public class HelpRequestDto
    {
        public HelpRequestDetailDto DetailsDto { get; set; }
        public List<HelpRequestDetailDto>? DetailsListDto { get; set; }
    }

    public class HelpRequestDetailDto
    {
        public Guid Id { get; set; }
        public string? RequestNumber { get; set; } = "";
        public string? ZoomLink { get; set; }
        public string? IssueTitle { get; set; }
        public string? IssueDescription { get; set; }
        public string? TrySofar { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? StudentName { get; set; }
        public Guid? CourseOfferingId { get; set; }
        public Guid? CourseOfferingName { get; set; }
        public DateTimeOffset? RequestedDate { get; set; }
        public Guid? AssignedTeacherId { get; set; }
        public Guid? AssignedTeacherName { get; set; }
    }
}