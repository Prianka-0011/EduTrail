using System;
using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public enum LabMode
    {
        InPerson = 1,
        Remote = 2,
        Hybrid = 3
    }

    public class TALabHour : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }  // Day of the week

        [Required]
        public TimeSpan StartTime { get; set; }   // Start time for this session

        [Required]
        public TimeSpan EndTime { get; set; }     // End time for this session

        [Required]
        public LabMode Mode { get; set; }         // Remote, InPerson, or Hybrid

        public Guid? LabId { get; set; }          // Optional lab location
        public Lab? Lab { get; set; }

        public string? RemoteLink { get; set; }   // Optional link if Mode is Remote or Hybrid

        public bool IsActive { get; set; } = true;

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
