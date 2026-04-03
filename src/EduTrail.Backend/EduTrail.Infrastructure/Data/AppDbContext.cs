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
        public DbSet<ChatMessage> ChatMessages { get; set; }
        
        #region QUARTZ

        public virtual DbSet<QURTZ_BLOB_TRIGGER> QRTZ_BLOB_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_CALENDAR> QRTZ_CALENDARS { get; set; }

        public virtual DbSet<QURTZ_CRON_TRIGGER> QRTZ_CRON_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_FIRED_TRIGGER> QRTZ_FIRED_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_JOB_DETAIL> QRTZ_JOB_DETAILS { get; set; }

        public virtual DbSet<QURTZ_LOCK> QRTZ_LOCKS { get; set; }

        public virtual DbSet<QURTZ_PAUSED_TRIGGER_GRP> QRTZ_PAUSED_TRIGGER_GRPS { get; set; }

        public virtual DbSet<QURTZ_SCHEDULER_STATE> QRTZ_SCHEDULER_STATES { get; set; }

        public virtual DbSet<QURTZ_SIMPLE_TRIGGER> QRTZ_SIMPLE_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_SIMPROP_TRIGGER> QRTZ_SIMPROP_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_TRIGGER> QRTZ_TRIGGERS { get; set; }

        #endregion
    }
}