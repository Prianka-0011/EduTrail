using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.Enrolements;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class EnrolementRepository : IEnrolementRepository
    {
        private readonly AppDbContext _context;
        public EnrolementRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Enrollment>> GetAllAsync(Guid? courseOfferingId = null)
        {
            var query = _context.Enrollments
            .Include(c => c.CourseOffering)
            .Include(c => c.Student).AsQueryable();
            if (courseOfferingId.HasValue)
            {
                query = query.Where(e => e.CourseOfferingId == courseOfferingId.Value);
            }
            return await query.ToListAsync();
        }
        public async Task<Enrollment> GetByIdAsync(Guid id)
        {
            return await _context.Enrollments.Include(c=>c.Student).ThenInclude(c=>c.Roles).Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.
             Where(u => !u.Roles.Select(r => r.Name).Contains("Instructor"))
            .ToListAsync();
        }
        public async Task<Enrollment> CreateAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }
        public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var enrollment = await GetByIdAsync(id);
            if (enrollment == null)
            {
                return false;
            }
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<Enrollment> GetByCourseOfferingIdAndStudentIdAsync(Guid courseOfferingId, Guid studentId)
        {
            return _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseOfferingId == courseOfferingId && e.StudentId == studentId);
        }

        public Task<Role> GetRoleTaAsync()
        {
            return _context.Roles.Where(r => r.Name == "TA").FirstOrDefaultAsync();
        }

        public Task<User> GetStudentByIdAsync(Guid studentId)
        {
            return _context.Users.Include(c=>c.Roles).Where(u => u.Id == studentId).FirstOrDefaultAsync();
        }
    }
}