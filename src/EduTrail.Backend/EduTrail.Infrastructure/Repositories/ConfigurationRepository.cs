using System.Collections;
using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.LabRequests;
using EduTrail.Application.Shared;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using EduTrail.Shared;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly AppDbContext _context;
        public ConfigurationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AutoGenerateNumber> CreateAutoGenerate(AutoGenerateNumber autoGenerateNumber)
        {
            _context.AutoGenerateNumbers.Add(autoGenerateNumber);
            await _context.SaveChangesAsync();
            return autoGenerateNumber;
        }
        public async Task<AutoGenerateNumber> GetAutoGenerateNumberByPrefixAsync(string prefix)
        {
            int currentYear = DateTime.Now.Year;
            var entity = await _context.AutoGenerateNumbers.Where(c => c.Prefix == prefix && c.Year == currentYear)
            .FirstOrDefaultAsync();
            return entity;
        }
        public async Task<AutoGenerateNumber> UpdateAutoGenerate(AutoGenerateNumber autoGenerateNumber)
        {
            _context.AutoGenerateNumbers.Update(autoGenerateNumber);
            await _context.SaveChangesAsync();
            return autoGenerateNumber;
        }
    }
}