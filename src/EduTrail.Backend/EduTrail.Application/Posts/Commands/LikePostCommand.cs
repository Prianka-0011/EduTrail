using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class LikePostCommand : IRequest<PostDetailDto>
    {
        public Guid PostId { get; set; }

        public bool IsLiked { get; set; }

        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<LikePostCommand, PostDetailDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;
            private readonly ICommonService _commonService;
            private readonly IUserRepository _userRepository;
            private readonly IUserCourseOfferingRepository _courseRepository;

            public Handler(
                IPostRepository repository,
                IMapper mapper,
                ICommonService commonService,
                IUserRepository userRepository,
                IUserCourseOfferingRepository courseRepository)
            {
                _repository = repository;
                _mapper = mapper;
                _commonService = commonService;
                _userRepository = userRepository;
                _courseRepository = courseRepository;
            }

            public async Task<PostDetailDto> Handle(
                LikePostCommand request,
                CancellationToken cancellationToken)
            {
                var userId = _commonService._CurrentUserService.GetUserId();

                var enrollment = await _courseRepository
                    .GetEnrollmentByUserIdAsync(
                        userId,
                        request.CourseOfferingId);

                if (enrollment == null)
                {
                    throw new KeyNotFoundException("Enrollment not found.");
                }

                var post = await _repository.GetByIdAsync(request.PostId);

                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found.");
                }

                var postUserAction =
                    await _repository.GetPostUserActionAsync(
                        request.PostId,
                        enrollment.Id);

                if (postUserAction == null)
                {
                    postUserAction = new PostUserAction
                    {
                        Id = Guid.NewGuid(),
                        PostId = request.PostId,
                        EnrollmentId = enrollment.Id,
                        IsLiked = request.IsLiked,
                        LikedDate = request.IsLiked
                            ? DateTime.UtcNow
                            : null
                    };

                    await _repository.AddPostUserActionAsync(postUserAction);
                }
                else
                {
                    postUserAction.IsLiked = request.IsLiked;
                    postUserAction.LikedDate = request.IsLiked
                        ? DateTime.UtcNow
                        : null;

                    await _repository.UpdatePostUserActionAsync(postUserAction);
                }

                // Reload post so UserActions reflect latest data
                post = await _repository.GetByIdAsync(request.PostId);

                var dto = _mapper.Map<PostDetailDto>(post);

                dto.IsLiked = postUserAction.IsLiked;

                dto.LikeCount = post.UserActions.Count(x => x.IsLiked);

                return dto;
            }
        }
    }
}