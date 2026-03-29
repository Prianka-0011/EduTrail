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

        public virtual DbSet<QURTZ_BLOB_TRIGGER> QURTZ_BLOB_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_CALENDAR> QURTZ_CALENDARS { get; set; }

        public virtual DbSet<QURTZ_CRON_TRIGGER> QURTZ_CRON_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_FIRED_TRIGGER> QURTZ_FIRED_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_JOB_DETAIL> QURTZ_JOB_DETAILS { get; set; }

        public virtual DbSet<QURTZ_LOCK> QURTZ_LOCKS { get; set; }

        public virtual DbSet<QURTZ_PAUSED_TRIGGER_GRP> QURTZ_PAUSED_TRIGGER_GRPS { get; set; }

        public virtual DbSet<QURTZ_SCHEDULER_STATE> QURTZ_SCHEDULER_STATES { get; set; }

        public virtual DbSet<QURTZ_SIMPLE_TRIGGER> QURTZ_SIMPLE_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_SIMPROP_TRIGGER> QURTZ_SIMPROP_TRIGGERS { get; set; }

        public virtual DbSet<QURTZ_TRIGGER> QURTZ_TRIGGERS { get; set; }

        #endregion
    }
}