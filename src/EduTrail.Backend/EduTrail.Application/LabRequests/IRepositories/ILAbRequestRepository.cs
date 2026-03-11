using EduTrail.Domain.Entities;

namespace EduTrail.Application.LabRequests
{
    public interface ILabRequestRepository
    {

        Task<LabRequest> GetHelRequestByIdAsync();
  
        Task<LabRequest> CreateHelpRequestAsync(LabRequest course);
        Task<IEnumerable<LabRequest>> GetAllLabRequestByCourseOfferingAsync(Guid courseOfferingId);
    }
}