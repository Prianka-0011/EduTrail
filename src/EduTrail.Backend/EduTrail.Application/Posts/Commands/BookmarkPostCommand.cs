using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class BookmarkPostCommand : IRequest<PostDetailDto>
    {
        public Guid PostId { get; set; }

        public bool IsBookmarked { get; set; }

        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<BookmarkPostCommand, PostDetailDto>
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
                BookmarkPostCommand request,
                CancellationToken cancellationToken)
            {
                var userId = _commonService._CurrentUserService.GetUserId();

                var enrollment =
                    await _courseRepository.GetEnrollmentByUserIdAsync(
                        userId,
                        request.CourseOfferingId);

                var post = await _repository.GetByIdAsync(request.PostId);

                if (post == null)
                    throw new KeyNotFoundException("Post not found.");

                var action =
                    await _repository.GetPostUserActionAsync(
                        request.PostId,
                        enrollment.Id);

                if (action == null)
                {
                    action = new PostUserAction
                    {
                        Id = Guid.NewGuid(),
                        PostId = request.PostId,
                        EnrollmentId = enrollment.Id,
                        IsBookmarked = request.IsBookmarked,
                        BookmarkedDate = request.IsBookmarked
                            ? DateTime.UtcNow
                            : null
                    };

                    await _repository.AddPostUserActionAsync(action);
                }
                else
                {
                    action.IsBookmarked = request.IsBookmarked;
                    action.BookmarkedDate = request.IsBookmarked
                        ? DateTime.UtcNow
                        : null;

                    await _repository.UpdatePostUserActionAsync(action);
                }

                return _mapper.Map<PostDetailDto>(post);
            }
        }
    }
}