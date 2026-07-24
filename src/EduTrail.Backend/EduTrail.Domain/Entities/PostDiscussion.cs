using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class PostDiscussion : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
        public Guid? ParentDiscussionId { get; set; }

         [ForeignKey(nameof(EnrollmentId))]
        public Enrollment Enrollment { get; set; } = null!;
        public Guid? EnrollmentId { get; set; }

        [ForeignKey(nameof(ParentDiscussionId))]
        public PostDiscussion? ParentDiscussion { get; set; }
        public bool? IsVisibleToInstructor { get; set; }
        public ICollection<PostDiscussion> Replies { get; set; }
            = new List<PostDiscussion>();

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public EditorType EditorType { get; set; }

        public bool IsResolved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public Guid? UpdatedById { get; set; }
    }
}