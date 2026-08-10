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
        Task<bool> DeleteAsync(
               Guid postId,
               CancellationToken cancellationToken = default);
        void RemovePoll(Poll poll);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseOfferingAsync(Guid courseOfferingId);

        // Add this
        Task<List<Enrollment>> GetEnrollmentsByIdsAsync(List<Guid> enrollmentIds);
        Task<IEnumerable<Folder>> GetFoldersByCourseOfferingAsync(Guid courseOfferingId);
        Task<List<Folder>> GetFoldersByIdsAsync(List<Guid> folderIds);

        Task<PollOption?> GetPollOptionByIdAsync(Guid id);
        Task<PollVote?> GetPollVoteByEnrollmentAsync(Guid enrollmentId, Guid pollId);
        Task<Poll?> GetPollByIdAsync(Guid pollId);
        Task AddPollVoteAsync(PollVote vote);
        Task UpdatePollOptionAsync(PollOption option);

        Task<PostDiscussion> CreateDiscussionAsync(PostDiscussion discussion);
        Task<PostDiscussion?> GetDiscussionByIdAsync(Guid id);

        Task<PostDiscussion> UpdateDiscussionAsync(
            PostDiscussion discussion);
        Task<Post> ArchivePostAsync(Guid id);

        Task<PostUserAction?> GetPostUserActionAsync(Guid postId, Guid enrollementId);

        Task<PostUserAction> AddPostUserActionAsync(
            PostUserAction action);

        Task<PostUserAction> UpdatePostUserActionAsync(
            PostUserAction action);
    }
}