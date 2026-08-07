using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class ArchivePostCommand : IRequest<PostDetailDto>
    {
        public Guid PostId { get; set; }
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<ArchivePostCommand, PostDetailDto>
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
                ArchivePostCommand request,
                CancellationToken cancellationToken)
            {


                var post = await _repository.GetByIdAsync(request.PostId);

                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found.");
                }

                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.CourseOfferingId);


                if (enrolement == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                var user = await _userRepository.GetByIdAsync(currentLoginUserId);
                var isInstructor = user.Roles.Any(r => r.Id == CustomCategory.RoleType.Instructor);

                var postUserAction = await _repository.GetPostUserActionAsync(
                    request.PostId,
                    enrolement.Id);

                if (postUserAction == null)
                {
                    postUserAction = new PostUserAction
                    {
                        Id = Guid.NewGuid(),
                        PostId = request.PostId,
                        EnrollmentId = enrolement.Id,
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