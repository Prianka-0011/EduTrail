using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.Enrolements;
using EduTrail.Application.HelpRequestDashboards;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class HelpRequestDashboardRepository : IHelpRequestDashboardRepository
    {
        private readonly AppDbContext _context;
        public HelpRequestDashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LabRequest>> GetAllAsync()
        {
            return await _context.LabRequests
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<LabRequest>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.LabRequests
                .Where(x => x.RequestedDate >= start && x.RequestedDate <= end)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}