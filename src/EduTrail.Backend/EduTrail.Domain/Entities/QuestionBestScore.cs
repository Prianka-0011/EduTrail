using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    // A user's best (maximum) score on a file-based catalog question, keyed by the
    // question's slug. One row per (user, question); only updated when a later attempt
    // beats the stored score.
    public class QuestionBestScore : IAuditable
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        [MaxLength(200)]
        public string QuestionSlug { get; set; } = string.Empty;

        // Fraction from 0.0 to 1.0.
        public float Score { get; set; }

        // Audit fields (set by AuditInterceptor).
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
