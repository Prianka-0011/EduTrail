using EduTrail.Domain.Entities;

namespace EduTrail.Application.Folders
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllAsync(Guid? courseOfferingId = null);

        Task<IEnumerable<Folder>> GetAllByParentIdAsync(Guid? parentId = null);
        Task<List<Folder>> GetSubByParentIdsAsync(List<Guid> parentIds);

        Task<List<Folder>> GetByIdsAsync(List<Guid> folderIds);

        Task<Folder?> GetByIdAsync(Guid id);

        Task<List<Folder>> CreateRangeAsync(List<Folder> folders);

        Task<Folder> UpdateAsync(Folder folder);

        Task DeleteRangeAsync(List<Folder> folders);
    }
}