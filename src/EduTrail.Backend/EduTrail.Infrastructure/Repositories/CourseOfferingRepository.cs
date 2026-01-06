using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class CourseOfferingRepository : ICourseOfferingRepository
    {
        private readonly AppDbContext _context;
        public CourseOfferingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CourseOffering> CreateAsync(CourseOffering courseOffering)
        {
            await _context.CourseOfferings.AddAsync(courseOffering);
            await _context.SaveChangesAsync();
            return courseOffering;
        }

        public async Task<List<CourseOffering>> GetAllAsync()
        {
            return await _context.CourseOfferings
            .Include(c => c.Course)
            .Include(c => c.Assessments)
            .Include(c => c.Instructor)
            .Include(c => c.Term).ToListAsync();
        }

        public async Task<CourseOffering> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.CourseOfferings.Where(c => c.InstructorId == userId).FirstOrDefaultAsync();
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

    }
}