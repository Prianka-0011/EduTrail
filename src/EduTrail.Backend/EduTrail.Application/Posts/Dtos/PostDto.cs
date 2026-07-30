using EduTrail.Application.Shared.Dtos;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class PostDto
    {
        public PostDetailDto? DetailsDto { get; set; }
        public List<PostDetailDto>? DetailsListDto { get; set; }
        public List<DropdownItemDto>? Enrollements { get; set; } = new List<DropdownItemDto>();
        public List<DropdownItemDto>? Types { get; set; } = new List<DropdownItemDto>();
        public List<FolderTree>? Folders { get; set; } = new List<FolderTree>();
    }

    public class PostDetailDto
    {
        public Guid Id { get; set; }

        public string Summary { get; set; } = string.Empty;

        public string? Details { get; set; }
        public string? VisibilityText { get; set; }

        public Guid? PostTypeId { get; set; }

        public string? PostTypeName { get; set; }

        public EditorType EditorType { get; set; }

        public bool? IsAnnouncement { get; set; }

        public bool? SendEmailImmediately { get; set; }

        public bool? IsScheduled { get; set; }

        public bool? CourseOfferingId { get; set; }

        public DateTimeOffset? ScheduledAt { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsIndividual { get; set; } = false;


        // Folder selection
        public List<Guid> FolderIds { get; set; } = new();
        public List<Guid> EnrollmentIds { get; set; } = new();


        // Poll
        public PollDto? Poll { get; set; }

        public List<PostDiscussionDto> Discussions { get; set; } = new();
    }

    public class PollDto
    {
        public Guid Id { get; set; }
        public string? Question { get; set; }
        public List<PollOptionDto> Options { get; set; } = new();
        public List<PollVoteDto> Votes { get; set; } = new();
    }

    public class PollOptionDto
    {
        public Guid Id { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public int VoteCount { get; set; }
    }

    public class PollVoteDto
    {
        public Guid Id { get; set; }

        public Guid PollOptionId { get; set; }

        public string? OptionText { get; set; }

        public Guid? EnrollmentId { get; set; }
        public Guid CourseOfferingId { get; set; }

        public string? VoterName { get; set; }

        public string? VoterEmail { get; set; }

        public DateTimeOffset VotedAt { get; set; }
    }

    public class FolderTree
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid? ParentFolderId { get; set; }

        public ICollection<FolderTree> ChildFolders { get; set; } = new List<FolderTree>();
    }

    public class PostDiscussionDto
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public Guid? ParentDiscussionId { get; set; }

        public Guid CourseOfferingId { get; set; }

        public Guid? EnrollmentId { get; set; }

        public string? AuthorName { get; set; }

        public string? AuthorEmail { get; set; }

        public string Content { get; set; } = string.Empty;

        public EditorType EditorType { get; set; }

        public bool IsResolved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public Guid? UpdatedById { get; set; }

        public List<PostDiscussionDto> Replies { get; set; } = new();
    }


    public class CreatePostDiscussionDto
    {
        public Guid PostId { get; set; }

        public Guid CourseOfferingId { get; set; }

        public Guid? ParentDiscussionId { get; set; }

        public Guid? EnrollmentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public EditorType EditorType { get; set; }
    }
}