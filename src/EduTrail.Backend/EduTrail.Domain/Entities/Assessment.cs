using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Assessment : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public int Credit	 { get; set; } = 0;
        public int MaxScore	 { get; set; } = 0;

        [Required]
        public Guid CourseId { get; set; }

        public Course Course { get; set; }

        [Required]
        public DateTimeOffset OpenDate { get; set; }

        [Required]
        public DateTimeOffset DueDate { get; set; }
        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

        public ICollection<Question> Questions { get; set; }
    }

}