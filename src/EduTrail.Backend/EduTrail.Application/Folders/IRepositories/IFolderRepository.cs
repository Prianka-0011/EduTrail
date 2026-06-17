using EduTrail.Domain.Entities;

namespace EduTrail.Application.Folders
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllAsync(Guid? courseOfferingId = null);
    }
}