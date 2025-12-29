using Microsoft.EntityFrameworkCore;
using EduTrail.Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Dynamic;
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
        public DbSet<AuditEntry> AuditEntries { get; set; } = null!;
        public DbSet<Test> Tests  {get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        public DbSet<Assessment> Assessments { get; set; } = null!;
        public DbSet<Submission> Submissions { get; set; } = null!; 
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<CourseOffering> CourseOfferings { get; set; } = null!;

    }
}