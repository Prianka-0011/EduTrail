using EduTrail.Domain.Entities;

namespace EduTrail.Application.QuestionTypes
{
    public interface IQuestionTypeRepository
    {
        Task<QuestionType> CreateAsync(QuestionType questionType);
        Task<IEnumerable<QuestionType>> GetAllAsync();
        Task<QuestionType> GetByIdAsync(Guid id);
        Task<QuestionType> UpdateAsync(QuestionType questionType);
        Task<bool> DeleteAsync(Guid id);
    }
}