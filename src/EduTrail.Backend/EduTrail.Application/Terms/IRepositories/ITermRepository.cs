using EduTrail.Domain.Entities;

namespace EduTrail.Application.Terms
{
    public interface ITermRepository
    {
        Task<Term> CreateAsync(Term term);
        Task<IEnumerable<Term>> GetAllAsync();
        Task<Term> GetByIdAsync(Guid id);
        Task<Term> UpdateAsync(Term term);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TermType>> GetTermTypesAsync();
    }
}