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
        void RemovePollOptions(IEnumerable<PollOption> options);
        void RemovePollOption(PollOption option);
        Task<bool> DeleteAsync(Guid id);
        void RemovePoll(Poll poll);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseOfferingAsync(Guid courseOfferingId);

        // Add this
        Task<List<Enrollment>> GetEnrollmentsByIdsAsync(List<Guid> enrollmentIds);

        Task<IEnumerable<Folder>> GetFoldersByCourseOfferingAsync(Guid courseOfferingId);
        Task<List<Folder>> GetFoldersByIdsAsync(List<Guid> folderIds);
    }
}