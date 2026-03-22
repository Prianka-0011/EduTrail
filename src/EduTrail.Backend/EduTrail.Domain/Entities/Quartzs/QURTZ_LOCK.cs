using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{
    [PrimaryKey("SCHED_NAME", "LOCK_NAME")]
    [Table("QURTZ_LOCKS")]
    public partial class QURTZ_LOCK
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(40)]
        public string LOCK_NAME { get; set; } = null!;
    }
}