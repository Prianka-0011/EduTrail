using System.ComponentModel.DataAnnotations;

namespace EduTrail.Domain.Entities
{
    public class TALabDay
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        [Required]
        public DateOnly LabDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

        // Navigation property for multiple slots on this day
        public ICollection<TALabSlot> Slots { get; set; } = new List<TALabSlot>();
    }

}