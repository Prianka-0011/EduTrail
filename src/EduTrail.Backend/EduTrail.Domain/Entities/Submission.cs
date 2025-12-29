using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Submission : IAuditable
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid QuestionAttemptId { get; set; }
        public QuestionAttempt QuestionAttempt { get; set; }

        [Required]
        public string StudentAnswerJson { get; set; }

        [Range(0, 1)]
        public decimal Score { get; set; }

        public bool IsCorrect => Score == 1;
        public DateTimeOffset SubmittedAt { get; set; }
        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}