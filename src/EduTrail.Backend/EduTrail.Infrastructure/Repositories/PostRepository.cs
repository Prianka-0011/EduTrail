using Microsoft.EntityFrameworkCore;
using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.Posts;

namespace EduTrail.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await _context.Posts.AddAsync(post);

            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<IEnumerable<Post>> GetAllAsync(Guid courseOfferingId)
        {
            return await _context.Posts
                .Include(x => x.Folders)
                .Include(x => x.Enrollments)
                .Include(c => c.PostType)
                .Include(x => x.Poll)
                .ThenInclude(x => x!.Options)
                .Where(x => x.Folders.Any(f =>
                    f.CourseOfferingId == courseOfferingId))
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(x => x.Folders)
                .Include(x => x.Enrollments)
                .ThenInclude(c => c.User)
                .Include(x => x.Poll)
                    .ThenInclude(x => x.Options)
                .Include(x => x.PostType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public void RemovePollOption(PollOption option)
        {
            _context.PollOptions.Remove(option);
        }
        public async Task<Post> UpdateAsync(Post post)
        {
            await _context.SaveChangesAsync();

            return post;
        }
        public void RemovePoll(Poll poll)
        {
            _context.Polls.Remove(poll);
        }
        public void RemovePollOptions(IEnumerable<PollOption> options)
        {
            _context.PollOptions.RemoveRange(options);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return false;

            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<PostType>> GetAllTypeAsync()
        {
            return await _context.PostTypes.ToListAsync();
        }
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseOfferingAsync(
            Guid courseOfferingId)
        {
            return await _context.Enrollments
                .Include(x => x.User)
                .Where(x => x.CourseOfferingId == courseOfferingId)
                .ToListAsync();
        }
        public async Task<List<Enrollment>> GetEnrollmentsByIdsAsync(
            List<Guid> enrollmentIds)
        {
            return await _context.Enrollments
                .Include(x => x.User)
                .Where(x => enrollmentIds.Contains(x.Id))
                .ToListAsync();
        }
        public async Task<IEnumerable<Folder>> GetFoldersByCourseOfferingAsync(
            Guid courseOfferingId)
        {
            return await _context.Folders
                .Where(x => x.CourseOfferingId == courseOfferingId)
                .ToListAsync();
        }
        public async Task<List<Folder>> GetFoldersByIdsAsync(
            List<Guid> folderIds)
        {
            return await _context.Folders
                .Where(x => folderIds.Contains(x.Id))
                .ToListAsync();
        }
        public async Task<PollOption?> GetPollOptionByIdAsync(Guid id)
        {
            return await _context.PollOptions
                .Include(x => x.Poll)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PollVote?> GetPollVoteByEnrollmentAsync(
            Guid enrollmentId,
            Guid pollId)
        {
            return await _context.PollVotes
                .Include(x => x.PollOption)
                .FirstOrDefaultAsync(x =>
                    x.EnrollmentId == enrollmentId &&
                    x.PollOption.PollId == pollId);
        }
        public async Task<Poll?> GetPollByIdAsync(Guid pollId)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == pollId);
        }
        public async Task AddPollVoteAsync(PollVote vote)
        {
            await _context.PollVotes.AddAsync(vote);
        }
        public async Task UpdatePollOptionAsync(PollOption option)
        {
            _context.PollOptions.Update(option);
            await _context.SaveChangesAsync();
        }
    }
}