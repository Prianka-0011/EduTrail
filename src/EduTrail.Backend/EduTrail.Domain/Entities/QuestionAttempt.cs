using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionAttempt : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }

        public int AttemptNumber { get; set; }

        public string GeneratedParametersJson { get; set; } // a=3, b=7
        public string CorrectAnswerJson { get; set; }        // sum=10

        public string? StudentAnswerJson { get; set; }

        public decimal Score { get; set; }  // 0.0 – 1.0
        public int CorrectScorePercentage { get; set; }

        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

}