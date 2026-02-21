using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EduTrail.Infrastructure.Data;
using EduTrail.Infrastructure.Repositories;
using EduTrail.Application.Tests;
using EduTrail.Application.Courses;
using EduTrail.Application.Terms;
using EduTrail.Application.Questions;
using EduTrail.Application.Assessments;
using EduTrail.Application.QuestionTypes;
using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Users;
using EduTrail.Application.Enrolements;


namespace EduTrail.Infrastructure
{
    public static class DependencyInjection
    {
        private static string dbHost =>
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                ? "edu-trail-sql"
                : "localhost";

        private static int dbPort => int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port) ? port : 1433;
        private static string dbName => Environment.GetEnvironmentVariable("DB_NAME") ?? "EduTrailDb";
        private static string dbUser => Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
        private static string dbPass => Environment.GetEnvironmentVariable("DB_PASS") ?? "EduTrail123!";

        public static string GetConnectionString(string databaseName)
        {
            return $"Server={dbHost},{dbPort};Database={databaseName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;Encrypt=False;";
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine($"DB Connection String: {GetConnectionString(dbName)}");

            // Add EF Core with retry on transient failures
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    GetConnectionString(dbName),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                );
            });

            // Quartz setup
            // services.AddQuartz(q =>
            // {
            //     q.SchedulerName = "EduTrail-Scheduler";
            //     q.SchedulerId = "AUTO";

            //     q.UseSimpleTypeLoader();

            //     q.UsePersistentStore(s =>
            //     {
            //         s.UseClustering();
            //         s.UseSqlServer(c =>
            //         {
            //             c.ConnectionString = GetConnectionString(dbName);
            //             c.TablePrefix = "EMOTIA_QRTZ_";
            //         });
            //         s.UseJsonSerializer();
            //     });

            //     q.UseDefaultThreadPool(tp =>
            //     {
            //         tp.MaxConcurrency = 10;
            //     });
            // });

            // services.AddQuartzHostedService(options =>
            // {
            //     options.WaitForJobsToComplete = true;
            // });

            // Register other services, repositories, etc.
            // services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ITermRepository, TermRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<IQuestionTypeRepository, QuestionTypeRepository>();
            services.AddScoped<ICourseOfferingRepository, CourseOfferingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEnrolementRepository, EnrolementRepository>();

            return services;
        }
    }
}
