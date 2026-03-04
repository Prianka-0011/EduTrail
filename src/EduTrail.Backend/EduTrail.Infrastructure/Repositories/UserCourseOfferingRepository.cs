using System.Collections;
using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.UserDashboards;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class UserCourseOfferingRepository : IUserCourseOfferingRepository
    {
        private readonly AppDbContext _context;
        public UserCourseOfferingRepository(AppDbContext context)
        {
            _context = context;
        }
        // public async Task<CourseOffering> CreateAsync(CourseOffering courseOffering)
        // {
        //     await _context.CourseOfferings.AddAsync(courseOffering);
        //     await _context.SaveChangesAsync();
        //     return courseOffering;
        // }
        // public async Task<List<Course>> GetAllCourses()
        // {
        //     return await _context.Courses.ToListAsync();
        // }
        // public async Task<List<Term>> GetAllTerms()
        // {
        //     return await _context.Terms.ToListAsync();
        // }
        // public async Task<List<User>> GetAllInstructors()
        // {
        //     return await _context.Users.
        //      Where(u => u.Roles.Select(r=>r.Name).Contains("Instructor"))
        //     .ToListAsync();
        // }

        // public async List<CourseOffering> GetAllByUserIdAsync(Guid userId)
        // {
        //     return await _context.CourseOfferings.Where(c => c.InstructorId == userId).ToListAsync();
        // }
        public async Task<CourseOffering> GetCourseOfferingById(Guid Id)
        {
            return await _context.CourseOfferings.Where(c => c.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<CourseOffering> UpdateAsync(CourseOffering courseOffering)
        {
            _context.CourseOfferings.Update(courseOffering);
            await _context.SaveChangesAsync();
            return courseOffering;
        }

        public async Task<IEnumerable<CourseOffering>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.CourseOfferings
                .Include(c=>c.Course)
                .Include(c=>c.Term)
                .Include(c=>c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.Enrollments.Any(e => e.StudentId == userId))
                .ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentByUserIdAsync(Guid userId)
        {
            return await _context.Enrollments.Include(c=>c.TALabMonths).Where(c=>c.StudentId == userId).FirstOrDefaultAsync();
        }

    }
}