using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.QuestionTypes;

namespace EduTrail.Infrastructure.Repositories
{
    public class QuestionTypeRepository : IQuestionTypeRepository
    {
        private readonly AppDbContext _context;

        public QuestionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionType> CreateAsync(QuestionType questionType)
        {
            await _context.QuestionTypes.AddAsync(questionType);
            await _context.SaveChangesAsync();
            return questionType;
        }

        public async Task<IEnumerable<QuestionType>> GetAllAsync()
        {
            return await _context.QuestionTypes.ToListAsync();
        }

        public async Task<QuestionType> GetByIdAsync(Guid id)
        {
            return await _context.QuestionTypes.FindAsync(id);
        }

        public async Task<QuestionType> UpdateAsync(QuestionType questionType)
        {
            _context.QuestionTypes.Update(questionType);
            await _context.SaveChangesAsync();
            return questionType;
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
