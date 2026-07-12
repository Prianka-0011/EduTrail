using EduTrail.Domain.Entities;
namespace EduTrail.Application.Posts
{

    public interface IPostRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<IEnumerable<Post>> GetAllAsync(Guid courseOfferingId);
        Task<IEnumerable<PostType>> GetAllTypeAsync();
        Task<Post> GetByIdAsync(Guid id);
        Task<Post> UpdateAsync(Post post);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseOfferingAsync(Guid courseOfferingId);
        Task<IEnumerable<Folder>> GetFoldersByCourseOfferingAsync(Guid courseOfferingId);
         Task<List<Folder>> GetFoldersByIdsAsync(List<Guid> folderIds);
    }
}