using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class LabRequest : IAuditable
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string RequestNumber { get; set; } = null!;

        public string? ZoomLink { get; set; }
        public string? IssueTitle { get; set; }
        public string? IssueDescription { get; set; }
        public string? TrySofar { get; set; }
        [ForeignKey("StudentId")]
        public Guid StudentId { get; set; }
        public Enrollment Student { get; set; } = null!;
        [ForeignKey("CourseOfferingId")]
        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; } = null!;

        public DateTimeOffset RequestedDate { get; set; }
        [ForeignKey("AssignedTeacherId")]
        public Guid? AssignedTeacherId { get; set; }
        public Enrollment? AssignedTeacher { get; set; }
        [ForeignKey("StatusId")]
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}