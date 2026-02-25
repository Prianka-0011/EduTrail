using System.ComponentModel.DataAnnotations;

namespace EduTrail.Domain.Entities
{
    public class TALabSlot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TALabDayId { get; set; }   // FK to TALabDay
        public TALabDay TALabDay { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public LabMode Mode { get; set; }

        public string? RemoteLink { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

}
public enum LabMode
{
    InPerson = 1,
    Remote = 2,
    Hybrid = 3
}
