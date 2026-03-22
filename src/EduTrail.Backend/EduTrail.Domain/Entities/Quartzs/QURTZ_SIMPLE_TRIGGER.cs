using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP")]
    [Table("QURTZ_SIMPLE_TRIGGERS")]
    public partial class QURTZ_SIMPLE_TRIGGER
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(150)]
        public string TRIGGER_NAME { get; set; } = null!;

        [Key]
        [StringLength(150)]
        public string TRIGGER_GROUP { get; set; } = null!;

        public int REPEAT_COUNT { get; set; }

        public long REPEAT_INTERVAL { get; set; }

        public int TIMES_TRIGGERED { get; set; }

        [ForeignKey("SCHED_NAME, TRIGGER_NAME, TRIGGER_GROUP")]
        [InverseProperty("QURTZ_SIMPLE_TRIGGER")]
        public virtual QURTZ_TRIGGER QURTZ_TRIGGER { get; set; } = null!;
    }
}