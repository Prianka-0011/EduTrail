using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP")]
    [Table("PMTS_QURTZ_BLOB_TRIGGERS")]
    public partial class PMTS_QURTZ_BLOB_TRIGGER
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

        public byte[]? BLOB_DATA { get; set; }
    }
}
