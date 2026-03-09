using EduTrail.Domain.Entities;

namespace EduTrail.Application.UserDashboards
{
    public interface IUserCourseOfferingRepository
    {
        Task<IEnumerable<CourseOffering>> GetAllByUserIdAsync(Guid userId);
        Task<Enrollment> GetEnrollmentByUserIdAsync(Guid userId, Guid courseOfferingId);
        Task<IEnumerable<Enrollment>> GetTAByCourseOfferingAsync( Guid courseOfferingId);
        Task<Enrollment> UpdateAsync(Enrollment course);
        Task<Enrollment> GetByIdAsync(Guid id);
        Task<LabRequest> CreateHelpRequestAsync(LabRequest course);
        Task<IEnumerable<CourseOffering>> GetAllLabRequestByCourseOfferingAsync(Guid userId);
    }
}