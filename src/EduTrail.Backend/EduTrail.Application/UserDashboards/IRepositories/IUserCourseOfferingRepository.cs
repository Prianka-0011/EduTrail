using EduTrail.Domain.Entities;

namespace EduTrail.Application.UserDashboards
{
    public interface IUserCourseOfferingRepository
    {
        // Task<CourseOfferings> CreateAsync(CourseOfferings CourseOfferings);
        Task<IEnumerable<CourseOffering>> GetAllByUserIdAsync(Guid userId);
        Task<Enrollment>GetEnrollmentByUserIdAsync(Guid userId, Guid courseOfferingId);
        // Task<CourseOfferings> GetByIdAsync(Guid id);
        // Task<CourseOfferings> UpdateAsync(CourseOfferings course);
        // Task<bool> DeleteAsync(Guid id);
        // Task<IEnumerable<CourseOfferingsType>> GetAllCourseOfferingsType();
        // Task<IEnumerable<Assessment>> GetAllAssessment();
    }
}