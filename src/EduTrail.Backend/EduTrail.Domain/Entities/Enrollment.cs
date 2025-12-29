using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{

    public class Enrollment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        public User Student { get; set; }

        public DateTimeOffset EnrolledDate { get; set; } 

        // Optional statuses (can extend later)
        public bool IsActive { get; set; } = true;
        // Audit fields
        
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

    }
}