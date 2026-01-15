using EduTrail.Domain.Entities;

namespace EduTrail.Application.Assesments
{
    public interface IAssesmentRepository
    {
         Task<Assessment> CreateAsync(Assessment Assessment);
        Task<IEnumerable<Assessment>> GetAllAsync();
        Task<Assessment> GetByIdAsync(Guid id);
        Task<Assessment> UpdateAsync(Assessment Assessment);
        Task<bool> DeleteAsync(Guid id);
    }
}