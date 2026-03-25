using EduTrail.Domain.Entities;

namespace EduTrail.Application.LabRequests
{
    public interface ILabRequestRepository
    {

        // Task<LabRequest> GetHelRequestByIdAsync();
        Task<LabRequest> CreateHelpRequestAsync(LabRequest course);
        Task<LabRequest> UpdateHelpRequestAsync(LabRequest course);
        Task<IEnumerable<LabRequest>> GetAllLabRequestByCourseOfferingAsync(Guid courseOfferingId);
        Task<LabRequest> GetLabRequestById(Guid id);
        Task<Enrollment> GetEnrollementByUserId(Guid userId);
        Task<IEnumerable<LabRequest>> GetAllLabRequestByCourseOfferingAsyncByLoginUser(Guid courseOfferingId, Guid enrollmentId);

    }
}