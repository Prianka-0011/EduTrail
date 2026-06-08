using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class PollOption
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PollId { get; set; }

        [ForeignKey(nameof(PollId))]
        public Poll Poll { get; set; } = null!;

        [Required]
        public string OptionText { get; set; } = string.Empty;

        public int VoteCount { get; set; }

        public ICollection<PollVote> Votes { get; set; } = new List<PollVote>();
    }
}