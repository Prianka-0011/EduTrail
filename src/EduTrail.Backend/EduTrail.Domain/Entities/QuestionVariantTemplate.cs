using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionVariantTemplate : IAuditable
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        [Required, MaxLength(50)]
        public string Language { get; set; } // Example: "C++", "Python"

        [Required]
        public string Template { get; set; } // Contains placeholders like {{DECLARATION}}
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

}