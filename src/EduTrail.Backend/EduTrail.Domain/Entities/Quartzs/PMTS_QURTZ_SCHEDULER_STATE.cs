using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "INSTANCE_NAME")]
    [Table("PMTS_QURTZ_SCHEDULER_STATE")]
    public partial class PMTS_QURTZ_SCHEDULER_STATE
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(200)]
        public string INSTANCE_NAME { get; set; } = null!;

        public long LAST_CHECKIN_TIME { get; set; }

        public long CHECKIN_INTERVAL { get; set; }
    }
}
