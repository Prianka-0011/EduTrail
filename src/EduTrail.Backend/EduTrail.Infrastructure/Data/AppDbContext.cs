using Microsoft.EntityFrameworkCore;
using EduTrail.Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Dynamic;
using EduTrail.Domain.Entities.Quartzs;
namespace EduTrail.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public Guid currentUserId { get; set; } = Guid.Empty;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditInterceptor(currentUserId));
        }
        public DbSet<AutoGenerateNumber> AutoGenerateNumbers { get; set; } = null!;
        public DbSet<AuditEntry> AuditEntries { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        public DbSet<QuestionVariantTemplate> QuestionVariantTemplates { get; set; } = null!;
        public DbSet<QuestionVariationRule> QuestionVariationRules { get; set; } = null!;

        public DbSet<QuestionLine> QuestionLines { get; set; } = null!;
        public DbSet<Assessment> Assessments { get; set; } = null!;
        public DbSet<Submission> Submissions { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<CourseOffering> CourseOfferings { get; set; } = null!;
        public DbSet<Term> Terms { get; set; }
        public DbSet<TermType> TermTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabRequest> LabRequests { get; set; }
        public DbSet<TALabMonth> TALabMonths { get; set; }
        public DbSet<TALabWeek> TALabWeeks { get; set; }
        public DbSet<TALabDay> TALabDays { get; set; }
        public DbSet<TALabSlot> TALabSlots { get; set; }
        
        #region QUARTZ

        public virtual DbSet<PMTS_QURTZ_BLOB_TRIGGER> PMTS_QURTZ_BLOB_TRIGGERs { get; set; }

        public virtual DbSet<PMTS_QURTZ_CALENDAR> PMTS_QURTZ_CALENDARs { get; set; }

        public virtual DbSet<PMTS_QURTZ_CRON_TRIGGER> PMTS_QURTZ_CRON_TRIGGERs { get; set; }

        public virtual DbSet<PMTS_QURTZ_FIRED_TRIGGER> PMTS_QURTZ_FIRED_TRIGGERs { get; set; }

        public virtual DbSet<PMTS_QURTZ_JOB_DETAIL> PMTS_QURTZ_JOB_DETAILs { get; set; }

        public virtual DbSet<PMTS_QURTZ_LOCK> PMTS_QURTZ_LOCKs { get; set; }

        public virtual DbSet<PMTS_QURTZ_PAUSED_TRIGGER_GRP> PMTS_QURTZ_PAUSED_TRIGGER_GRPs { get; set; }

        public virtual DbSet<PMTS_QURTZ_SCHEDULER_STATE> PMTS_QURTZ_SCHEDULER_STATEs { get; set; }

        public virtual DbSet<PMTS_QURTZ_SIMPLE_TRIGGER> PMTS_QURTZ_SIMPLE_TRIGGERs { get; set; }

        public virtual DbSet<PMTS_QURTZ_SIMPROP_TRIGGER> PMTS_QURTZ_SIMPROP_TRIGGERs { get; set; }

        public virtual DbSet<PMTS_QURTZ_TRIGGER> PMTS_QURTZ_TRIGGERs { get; set; }

        #endregion
    }
}