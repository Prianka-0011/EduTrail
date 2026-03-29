using EduTrail.Domain.Entities;
namespace EduTrail.Application.HelpRequestDashboards
{

    public interface IHelpRequestDashboardRepository
    {
        Task<List<LabRequest>> GetAllAsync();
        Task<List<LabRequest>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}