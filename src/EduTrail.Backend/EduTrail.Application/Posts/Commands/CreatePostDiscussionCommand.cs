using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;

namespace EduTrail.Application.Posts
{
    public class CreatePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
        public CreatePostDiscussionDto DiscussionDto { get; set; } = new();


        public class Handler
            : IRequestHandler<
                CreatePostDiscussionCommand,
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
                IUserCourseOfferingRepository courseRepository)
            {
                _repository = repository;
                _mapper = mapper;
                _commonService = commonService;
                _courseRepository = courseRepository;
            }

            public async Task<PostDiscussionDto> Handle(
                CreatePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var dto = request.DiscussionDto;

                var discussion =
                    _mapper.Map<PostDiscussion>(dto);

                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.DiscussionDto.CourseOfferingId);


                discussion.CreatedDate =
                    DateTimeOffset.UtcNow;
                discussion.CreatedById = currentLoginUserId;
                discussion.EnrollmentId = enrolement.Id;

                discussion.IsDeleted = false;

                discussion.IsResolved = false;

                var result =
                    await _repository.CreateDiscussionAsync(
                        discussion);
                var res = _mapper.Map<PostDiscussionDto>(
                    result);
                res.AuthorName = enrolement?.User?.FirstName + " " + enrolement?.User?.LastName;
                return res;
            }
        }
    }
}