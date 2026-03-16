using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "JOB_NAME", "JOB_GROUP")]
    [Table("PMTS_QURTZ_JOB_DETAILS")]
    public partial class PMTS_QURTZ_JOB_DETAIL
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(150)]
        public string JOB_NAME { get; set; } = null!;

        [Key]
        [StringLength(150)]
        public string JOB_GROUP { get; set; } = null!;

        [StringLength(250)]
        public string? DESCRIPTION { get; set; }

        [StringLength(250)]
        public string JOB_CLASS_NAME { get; set; } = null!;

        public bool IS_DURABLE { get; set; }

        public bool IS_NONCONCURRENT { get; set; }

        public bool IS_UPDATE_DATA { get; set; }

        public bool REQUESTS_RECOVERY { get; set; }

        public byte[]? JOB_DATA { get; set; }

        [InverseProperty("PMTS_QURTZ_JOB_DETAIL")]
        public virtual ICollection<PMTS_QURTZ_TRIGGER> PMTS_QURTZ_TRIGGERs { get; set; } = new List<PMTS_QURTZ_TRIGGER>();
    }
}
