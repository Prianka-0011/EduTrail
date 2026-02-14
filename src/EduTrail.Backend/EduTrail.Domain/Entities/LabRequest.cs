using System.ComponentModel.DataAnnotations;
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
        public Guid StudentId { get; set; }
        public User Student { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateOnly RequestedDate { get; set; }
        public Guid? AssignedTeacherId { get; set; }
        public User? AssignedTeacher { get; set; }
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}