using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Users;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class GetPostViewByIdQuery : IRequest<PostDto>
    {
        public Guid Id { get; set; }
        public Guid CourseOfferingId { get; set; }


        public class Handler : IRequestHandler<GetPostViewByIdQuery, PostDto>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMapper _mapper;
            private readonly IUserRepository _userRepository;


            public Handler(
                IPostRepository postRepository,
                IMapper mapper,
                IUserRepository userRepository)
            {
                _postRepository = postRepository;
                _mapper = mapper;
                _userRepository = userRepository;
            }


            public async Task<PostDto> Handle(
                GetPostViewByIdQuery request,
                CancellationToken cancellationToken)
            {
                var post = await _postRepository.GetByIdAsync(request.Id);


                if (post == null)
                {
                    return new PostDto
                    {
                        DetailsDto = null
                    };
                }


                var postDto = _mapper.Map<PostDetailDto>(post);


                postDto.EditorType = post.EditorType;

                var user = await _userRepository.GetByIdAsync(post.CreatedById ?? Guid.Empty);
                postDto.PostOwnerName = user?.FirstName + " " + user?.LastName;
                // Folders
                postDto.FolderIds = post.Folders
                    .Select(x => x.Id)
                    .ToList();


                // Enrollments
                postDto.EnrollmentIds = post.Enrollments
                    .Select(x => x.Id)
                    .ToList();


                // View page properties
                postDto.PostTypeName = post.PostType?.Name;


                postDto.IsIndividual = post.IsIndividual ?? false;

                if (post.IsIndividual == true)
                {
                    var selectedAudience = post.Enrollments
                        .Select(e => $"{e.User.FirstName} {e.User.LastName}")
                        .ToList();

                    if (selectedAudience.Count == 1)
                    {
                        postDto.VisibilityText =
                            $"Only visible to {selectedAudience[0]}";
                    }
                    else if (selectedAudience.Count > 1)
                    {
                        postDto.VisibilityText =
                            $"Only visible to {selectedAudience[0]} and {selectedAudience.Count - 1} others";
                    }
                    else
                    {
                        postDto.VisibilityText =
                            "Only visible to selected students";
                    }
                }
                else
                {
                    postDto.VisibilityText =
                        "Visible to all students";
                }

                postDto.CreatedDate = post.CreatedDate;


                postDto.UpdateDate = post.UpdatedDate;


                // Discussions
                var discussionUserIds = post.Discussions
                .SelectMany(x => new[] { x }.Concat(x.Replies))
                .Where(x => x.CreatedById.HasValue)
                .Select(x => x.CreatedById!.Value)
                .Distinct()
                .ToList();

                var users = await _userRepository.GetByIdsAsync(discussionUserIds);

                var userDictionary = users.ToDictionary(
                    x => x.Id,
                    x => $"{x.FirstName} {x.LastName}"
                );


                postDto.Discussions = post.Discussions
                    .Where(x => x.ParentDiscussionId == null)
                    .Select(x => new PostDiscussionDto
                    {
                        Id = x.Id,
                        Content = x.Content,
                        CreatedDate = x.CreatedDate,
                        IsResolved = x.IsResolved,
                        PostId = x.PostId,
                        AuthorName = x.CreatedById.HasValue &&
                                     userDictionary.TryGetValue(x.CreatedById.Value, out var author)
                                     ? author
                                     : "Unknown",
                        Likes = x.Likes,
                        Replies = x.Replies
                            .Select(r => new PostDiscussionDto
                            {
                                Id = r.Id,
                                Content = r.Content,
                                CreatedDate = r.CreatedDate,
                                IsResolved = r.IsResolved,
                                PostId = x.PostId,
                                AuthorName = r.CreatedById.HasValue &&
                                             userDictionary.TryGetValue(r.CreatedById.Value, out var replyAuthor)
                                             ? replyAuthor
                                             : "Unknown",
                                Likes = r.Likes,
                            })
                            .ToList()
                    })
                    .ToList();


                return new PostDto
                {
                    DetailsDto = postDto
                };
            }
        }
    }
}