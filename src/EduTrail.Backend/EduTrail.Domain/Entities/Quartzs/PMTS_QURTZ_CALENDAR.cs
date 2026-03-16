using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Domain.Entities.Quartzs
{

    [PrimaryKey("SCHED_NAME", "CALENDAR_NAME")]
    [Table("PMTS_QURTZ_CALENDARS")]
    public partial class PMTS_QURTZ_CALENDAR
    {
        [Key]
        [StringLength(120)]
        public string SCHED_NAME { get; set; } = null!;

        [Key]
        [StringLength(200)]
        public string CALENDAR_NAME { get; set; } = null!;

        public byte[] CALENDAR { get; set; } = null!;
    }
}