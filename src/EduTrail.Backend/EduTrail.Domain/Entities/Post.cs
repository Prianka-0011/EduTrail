using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class Post : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Summary { get; set; } = string.Empty;

        public string? Details { get; set; }

        [Required]
        public Guid PostTypeId { get; set; }

        [ForeignKey(nameof(PostTypeId))]
        public PostType PostType { get; set; } = null!;

        [Required]
        public EditorType EditorType { get; set; }

        [Required]
        public bool IsAnnouncement { get; set; }

        [Required]
        public bool SendEmailImmediately { get; set; }

        [Required]
        public bool IsScheduled { get; set; }

        public DateTimeOffset? ScheduledAt { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

    public enum EditorType
    {
        RichText = 1,
        PlainText = 2,
        Markdown = 3
    }
}