using System.Collections;
using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.LabRequests;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using EduTrail.Shared;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class LabRequestRepository : ILabRequestRepository
    {
        private readonly AppDbContext _context;
        public LabRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AutoGenerateNumber> CreateAutoGenerate(AutoGenerateNumber autoGenerateNumber)
        {
            _context.AutoGenerateNumbers.Add(autoGenerateNumber);
            await _context.SaveChangesAsync();
            return autoGenerateNumber;
        }

        public async Task<LabRequest> CreateHelpRequestAsync(LabRequest request)
        {
            await _context.LabRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<LabRequest>> GetAllLabRequestByCourseOfferingAsync(Guid courseOfferingId)
        {
            return await _context.LabRequests
                .Where(c => c.CourseOfferingId == courseOfferingId)
                .OrderBy(c => c.RequestedDate)
                .ThenBy(c => c.Id)
                .Include(c => c.CourseOffering)
                    .ThenInclude(c => c.Course)
                .Include(c => c.Student)
                    .ThenInclude(c => c.User)
                .Include(c => c.Status)
                .Include(c => c.AssignedTeacher)
                    .ThenInclude(c => c.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AutoGenerateNumber> GetAutoGenerateNumberByPrefixAsync(string prefix)
        {
            int currentYear = DateTime.Now.Year;
            var entity = await _context.AutoGenerateNumbers.Where(c => c.Prefix == prefix && c.Year == currentYear)
            .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<LabRequest> GetHelRequestByIdAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.LabRequests.Where(r => r.CreatedDate >= today)
            .OrderByDescending(r => r.CreatedDate)
            .FirstOrDefaultAsync();
        }

        public async Task<AutoGenerateNumber> UpdateAutoGenerate(AutoGenerateNumber autoGenerateNumber)
        {
            _context.AutoGenerateNumbers.Update(autoGenerateNumber);
            await _context.SaveChangesAsync();
            return autoGenerateNumber;
        }
        public async Task<Enrollment> GetEnrollementByUserId(Guid userId)
        {
            var enrollement = _context.Enrollments.Where(c => c.UserId == userId).FirstOrDefault();
            await _context.SaveChangesAsync();
            return enrollement;
        }

        public async Task<LabRequest> UpdateHelpRequestAsync(LabRequest request)
        {
            _context.LabRequests.Update(request);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return request;
        }

        public async Task<IEnumerable<LabRequest>> GetAllLabRequestByCourseOfferingAsyncByLoginUser(Guid courseOfferingId, Guid enrollmentId)
        {
            return await _context.LabRequests
               .Where(c => c.CourseOfferingId == courseOfferingId && c.StudentId == enrollmentId)
               .OrderBy(c => c.RequestedDate)
               .ThenBy(c => c.Id)
               .Include(c => c.CourseOffering)
                   .ThenInclude(c => c.Course)
               .Include(c => c.Student)
                   .ThenInclude(c => c.User)
               .Include(c => c.Status)
               .Include(c => c.AssignedTeacher)
                   .ThenInclude(c => c.User)
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<LabRequest> GetLabRequestById(Guid id)
        {
            return await _context.LabRequests
            .Include(c=>c.Status)
            .Include(c=>c.Student).ThenInclude(c=>c.User)
            .Include(c=>c.CourseOffering).ThenInclude(c=>c.Course)
            .Include(c=>c.AssignedTeacher).Where(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}