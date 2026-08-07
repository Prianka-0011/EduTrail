using MediatR;
using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Application.UserDashboards;

namespace EduTrail.Application.Posts
{
    public class GetAllPostByEnrolementAndByCourseQuery : IRequest<PostDto>
    {
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<GetAllPostByEnrolementAndByCourseQuery, PostDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;
            private readonly ICommonService _commonService;
            private readonly IUserRepository _userRepository;
            private readonly IUserCourseOfferingRepository _courseRepository;
            public Handler(IPostRepository repository,
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

            public async Task<PostDto> Handle(
     GetAllPostByEnrolementAndByCourseQuery request,
     CancellationToken cancellationToken)
            {
                var posts = await _repository.GetAllAsync(request.CourseOfferingId);

                var currentUserId = _commonService._CurrentUserService.GetUserId();

                var enrollment = await _courseRepository.GetEnrollmentByUserIdAsync(currentUserId, request.CourseOfferingId);

                var postDtos = _mapper.Map<List<PostDetailDto>>(posts);

                foreach (var dto in postDtos)
                {
                    var userAction = await _repository.GetPostUserActionAsync(dto.Id, enrollment.Id);

                    // Global instructor pin takes precedence
                    if (dto.IsPinned)
                    {
                        dto.IsPinned = true;
                    }
                    else
                    {
                        dto.IsPinned = userAction?.IsPinned ?? false;
                    }

                    dto.IsFavorite = userAction?.IsFavorite ?? false;
                    dto.IsRead = userAction?.IsRead ?? false;
                }

                return new PostDto
                {
                    DetailsListDto = postDtos
                };
            }
        }
    }
}