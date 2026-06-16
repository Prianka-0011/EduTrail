using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrail.Domain.Entities
{
    public class PollVote
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PollOptionId { get; set; }

        [ForeignKey(nameof(PollOptionId))]
        public PollOption PollOption { get; set; } = null!;

        [Required]
        public Guid EnrollmentId { get; set; }

        [ForeignKey(nameof(EnrollmentId))]
        public Enrollment Enrollment { get; set; } = null!;
        public DateTimeOffset VotedAt { get; set; }
    }
}