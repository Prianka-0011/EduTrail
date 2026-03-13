using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{

    public class Enrollment : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; }


        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public decimal? TotalWorkHoursPerWeek { get; set; }

        public DateTimeOffset EnrolledDate { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<TALabMonth> TALabMonths { get; set; } = new List<TALabMonth>();
        // Audit fields

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}