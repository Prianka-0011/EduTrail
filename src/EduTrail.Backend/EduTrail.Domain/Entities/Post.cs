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

        public bool? IsAnnouncement { get; set; } = false;

        public bool? SendEmailImmediately { get; set; } = false;

        public bool? IsScheduled { get; set; } = false;

        public bool? IsIndividual { get; set; } = false;

        public DateTimeOffset? ScheduledAt { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }
        public DateTime? PinnedDate { get; set; }
        public ICollection<PostUserAction> UserActions { get; set; }
                = new List<PostUserAction>();

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<PostDiscussion> Discussions { get; set; } = new List<PostDiscussion>();
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public Poll? Poll { get; set; }
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    }

    public enum EditorType
    {
        RichText = 1,
        PlainText = 2,
        Markdown = 3
    }
}