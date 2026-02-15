using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class CourseOffering : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public Guid TermId { get; set; }
        public Term Term { get; set; }

      
        public Guid? InstructorId { get; set; }
        public User? Instructor { get; set; }
        public ICollection<Assessment> Assessments { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }


    }

}