using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{
    [PrimaryKey("SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP")]
    [Table("QURTZ_CRON_TRIGGERS")]
    public partial class QURTZ_CRON_TRIGGER
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

        [StringLength(120)]
        public string CRON_EXPRESSION { get; set; } = null!;

        [StringLength(80)]
        public string? TIME_ZONE_ID { get; set; }

        [ForeignKey("SCHED_NAME, TRIGGER_NAME, TRIGGER_GROUP")]
        [InverseProperty("QURTZ_CRON_TRIGGER")]
        public virtual QURTZ_TRIGGER QURTZ_TRIGGER { get; set; } = null!;
    }
}