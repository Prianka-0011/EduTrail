using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP")]
    [Table("PMTS_QURTZ_SIMPROP_TRIGGERS")]
    public partial class PMTS_QURTZ_SIMPROP_TRIGGER
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

        [StringLength(512)]
        public string? STR_PROP_1 { get; set; }

        [StringLength(512)]
        public string? STR_PROP_2 { get; set; }

        [StringLength(512)]
        public string? STR_PROP_3 { get; set; }

        public int? INT_PROP_1 { get; set; }

        public int? INT_PROP_2 { get; set; }

        public long? LONG_PROP_1 { get; set; }

        public long? LONG_PROP_2 { get; set; }

        [Column(TypeName = "numeric(13, 4)")]
        public decimal? DEC_PROP_1 { get; set; }

        [Column(TypeName = "numeric(13, 4)")]
        public decimal? DEC_PROP_2 { get; set; }

        public bool? BOOL_PROP_1 { get; set; }

        public bool? BOOL_PROP_2 { get; set; }

        [StringLength(80)]
        public string? TIME_ZONE_ID { get; set; }

        [ForeignKey("SCHED_NAME, TRIGGER_NAME, TRIGGER_GROUP")]
        [InverseProperty("PMTS_QURTZ_SIMPROP_TRIGGER")]
        public virtual PMTS_QURTZ_TRIGGER PMTS_QURTZ_TRIGGER { get; set; } = null!;
    }
}
