using EduTrail.Domain.Entities;
namespace EduTrail.Application.Courses
{

    public interface ICourseRepository
    {
        Task<Course> CreateAsync(Course course);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(Guid id);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(Guid id);
    }
}