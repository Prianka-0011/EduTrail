using EduTrail.Domain.Entities;

namespace EduTrail.Application.Questions
{
    public interface IQuestionRepository
    {
         Task<Question> CreateAsync(Question question);
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(Guid id);
        Task<Question> UpdateAsync(Question course);
        Task<bool> DeleteAsync(Guid id);
    }
}