using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduTrail.Domain.Entities
{
    public class TALabWeek
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int WeekNumber { get; set; }
        [ForeignKey("TALabMonthId")]
        public Guid TALabMonthId { get; set; }
        public TALabMonth TALabMonth { get; set; }
        public List<TALabDay> Days { get; set; }
    }
}