using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrail.Domain.Entities
{
    public class TALabMonth
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int Month { get; set; } // 1–12
        [Required]
        public int Year { get; set; }
        [ForeignKey("EnrollmentId")]
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
        public List<TALabWeek> Weeks { get; set; }
    }
}