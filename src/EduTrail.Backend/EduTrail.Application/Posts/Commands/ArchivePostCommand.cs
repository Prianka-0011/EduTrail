using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class ArchivePostCommand : IRequest<PostDetailDto>
    {
        public Guid PostId { get; set; }

        public class Handler : IRequestHandler<ArchivePostCommand, PostDetailDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;
            private readonly ICommonService _commonService;
            private readonly IUserRepository _userRepository;

            public Handler(
                IPostRepository repository,
                IMapper mapper,
                ICommonService commonService,
                IUserRepository userRepository)
            {
                _repository = repository;
                _mapper = mapper;
                _commonService = commonService;
                _userRepository = userRepository;
            }

            public async Task<PostDetailDto> Handle(
                ArchivePostCommand request,
                CancellationToken cancellationToken)
            {
                var userId = _commonService._CurrentUserService.GetUserId();

                var post = await _repository.GetByIdAsync(request.PostId);

                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found.");
                }

                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }

                var isInstructor = user.Roles.Any(r => r.Id == CustomCategory.RoleType.Instructor);

                var postUserAction = await _repository.GetPostUserActionAsync(
                    request.PostId,
                    userId);

                if (postUserAction == null)
                {
                    postUserAction = new PostUserAction
                    {
                        Id = Guid.NewGuid(),
                        PostId = request.PostId,
                        UserId = userId,
                        IsArchived = true,
                        ArchivedDate = DateTime.UtcNow
                    };

                    await _repository.AddPostUserActionAsync(postUserAction);
                }
                else
                {
                    postUserAction.IsArchived = true;
                    postUserAction.ArchivedDate = DateTime.UtcNow;

                    await _repository.UpdatePostUserActionAsync(postUserAction);
                }

                // Only instructors archive the post globally
                if (isInstructor)
                {
                    post.IsArchived = true;
                    await _repository.UpdateAsync(post);
                }

                return _mapper.Map<PostDetailDto>(post);
            }
        }
    }
}