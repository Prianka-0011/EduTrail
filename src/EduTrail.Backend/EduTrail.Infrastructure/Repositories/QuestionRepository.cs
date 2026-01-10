using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.Questions;

namespace EduTrail.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> GetByIdAsync(Guid id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task<Question> UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
