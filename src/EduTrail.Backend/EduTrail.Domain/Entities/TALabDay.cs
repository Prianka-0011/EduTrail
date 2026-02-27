using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrail.Domain.Entities
{
    public class TALabDay
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("TALabWeekId")]
        public Guid TALabWeekId { get; set; }
        public TALabWeek TALabWeek { get; set; }

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