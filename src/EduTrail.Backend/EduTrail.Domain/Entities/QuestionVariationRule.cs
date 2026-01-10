using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionVariationRule : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; } // Placeholder name: "DECLARATION", "OPERATION"

        [Required]
        public string OptionsJson { get; set; } // JSON array of possible values
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}