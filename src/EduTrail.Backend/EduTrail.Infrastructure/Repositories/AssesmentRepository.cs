using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.Courses;
using EduTrail.Application.Assessments;

namespace EduTrail.Infrastructure.Repositories
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly AppDbContext _context;

        public AssessmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Assessment> CreateAsync(Assessment assessment)
        {
            await _context.Assessments.AddAsync(assessment);
            await _context.SaveChangesAsync();
            return assessment;
        }

        public async Task<IEnumerable<Assessment>> GetAllAsync()
        {
            try
            {
                var test = _context.Assessments.ToListAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            // return await _context.Assessments.ToListAsync();
        }

        public async Task<Assessment> GetByIdAsync(Guid id)
        {
            return await _context.Assessments.FindAsync(id);
        }

        public async Task<Assessment> UpdateAsync(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            await _context.SaveChangesAsync();
            return assessment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null) return false;

            _context.Assessments.Remove(assessment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
