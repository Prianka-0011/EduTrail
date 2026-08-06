using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class LikePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
        public Guid DiscussionId { get; set; }
        public Guid CourseOfferingId { get; set; }


        public class Handler
            : IRequestHandler<
                LikePostDiscussionCommand,
                PostDiscussionDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;
            private ICommonService _commonService;
            private readonly IUserCourseOfferingRepository _courseRepository;

            public Handler(
                IPostRepository repository,
                IMapper mapper,
                ICommonService commonService,
                IUserCourseOfferingRepository courseRepository
                )
            {
                _repository = repository;
                _mapper = mapper;
                _commonService = commonService;
                _courseRepository = courseRepository;
            }

            public async Task<PostDiscussionDto> Handle(
                LikePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var discussion =
                    await _repository.GetDiscussionByIdAsync(
                        request.DiscussionId);

                if (discussion == null)
                {
                    throw new KeyNotFoundException(
                        "Discussion not found.");
                }

                discussion.Likes =
                    discussion.Likes += 1;

                // var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                // var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(discussion?.Enrollment.UserId??Guid.Empty, request.CourseOfferingId);


                var result =
                    await _repository.UpdateDiscussionAsync(
                        discussion);

                var test = _mapper.Map<PostDiscussionDto>(
                    result);
                test.AuthorName = discussion.Enrollment?.User?.FirstName + " " + discussion.Enrollment?.User?.LastName;

                return test;
            }
        }
    }
}