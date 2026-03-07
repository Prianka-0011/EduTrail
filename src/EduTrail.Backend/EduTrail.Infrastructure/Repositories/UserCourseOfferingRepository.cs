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
                .Include(c => c.Course)
                .Include(c => c.Term)
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.Enrollments.Any(e => e.StudentId == userId))
                .ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentByUserIdAsync(Guid userId, Guid courseOfferingId)
        {
            return await _context.Enrollments.Include(c => c.Student).ThenInclude(c => c.Roles).Include(c => c.TALabMonths).ThenInclude(c => c.Weeks).ThenInclude(c => c.Days).ThenInclude(c => c.Slots).Include(c => c.CourseOffering).ThenInclude(c => c.Term).Where(c => c.StudentId == userId && c.CourseOfferingId == courseOfferingId).FirstOrDefaultAsync();
        }

        public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment> GetByIdAsync(Guid id)
        {
            return await _context.Enrollments.Include(c => c.CourseOffering).ThenInclude(c => c.Term).Include(c => c.Student).ThenInclude(c => c.Roles).Where(c => c.Id == id)
            .Include(c => c.TALabMonths).ThenInclude(c => c.Weeks).ThenInclude(c => c.Days).ThenInclude(c => c.Slots).FirstOrDefaultAsync();
        }
    }
}