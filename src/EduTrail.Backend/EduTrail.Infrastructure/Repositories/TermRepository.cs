using EduTrail.Application.Terms;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class TermRepository : ITermRepository
    {
         private readonly AppDbContext _context;

        public TermRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Term> CreateAsync(Term term)
        {
            await _context.Terms.AddAsync(term);
            await _context.SaveChangesAsync();
            return term;
        }

        public async Task<IEnumerable<Term>> GetAllAsync()
        {
            return await _context.Terms.ToListAsync();
        }

        public async Task<Term> GetByIdAsync(Guid id)
        {
            return await _context.Terms.FindAsync(id);
        }

        public async Task<Term> UpdateAsync(Term term)
        {
            _context.Terms.Update(term);
            await _context.SaveChangesAsync();
            return term;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var term = await _context.Terms.FindAsync(id);
            if (term == null) return false;

            _context.Terms.Remove(term);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TermType>> GetTermTypesAsync()
        {
            return await _context.TermTypes.ToListAsync();
        }
    }
}