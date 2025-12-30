using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Course : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CourseCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string CourseName { get; set; }
        public DateTimeOffset? TimeZone { get; set; }
        public string? Institute { get; set; }

        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public ICollection<CourseOffering> CourseOfferings { get; set; }
        
    }

}