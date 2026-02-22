using EduTrail.Domain.Entities;

namespace EduTrail.Application.Enrolements
{
    public interface IEnrolementRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync(Guid? courseOfferingId = null);
        Task<Enrollment> GetByIdAsync(Guid id);
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<Enrollment> UpdateAsync(Enrollment enrollment);
        Task<bool> DeleteAsync(Guid id);
        Task<List<User>> GetAllUsersAsync();
    }
}