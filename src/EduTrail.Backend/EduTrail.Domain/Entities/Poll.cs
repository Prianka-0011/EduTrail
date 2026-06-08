using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class Poll : IAuditable
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

    }
}
