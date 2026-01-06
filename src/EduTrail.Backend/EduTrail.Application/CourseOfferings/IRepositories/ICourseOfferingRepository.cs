using EduTrail.Domain.Entities;

namespace EduTrail.Application.CourseOfferings
{
    public interface ICourseOfferingRepository
    {
         Task<List<CourseOffering>> GetAllAsync();
        Task<CourseOffering> GetAllByUserIdAsync(Guid userId);
        Task<CourseOffering> CreateAsync(CourseOffering courseOffering);
        Task<CourseOffering> UpdateAsync(CourseOffering courseOffering);
        Task<CourseOffering> GetCourseOfferingById(Guid Id);
    }
}