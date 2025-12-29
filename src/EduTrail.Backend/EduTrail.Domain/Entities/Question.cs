using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Question : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Template { get; set; } // "What is {a} + {b}?"

        [Required]
        public Guid QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }

        
        public Guid AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

}