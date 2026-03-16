using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "TRIGGER_GROUP")]
    [Table("PMTS_QURTZ_PAUSED_TRIGGER_GRPS")]
    public partial class PMTS_QURTZ_PAUSED_TRIGGER_GRP
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(150)]
        public string TRIGGER_GROUP { get; set; } = null!;
    }
}
