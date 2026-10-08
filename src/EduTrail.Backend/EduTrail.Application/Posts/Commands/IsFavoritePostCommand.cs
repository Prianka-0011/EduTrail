using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class IsFavoritePostCommand : IRequest<PostDetailDto>
    {
        public Guid PostId { get; set; }
        public bool IsFavorite { get; set; }
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<IsFavoritePostCommand, PostDetailDto>
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
                IsFavoritePostCommand request,
                CancellationToken cancellationToken)
            {
                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.CourseOfferingId);


                var post = await _repository.GetByIdAsync(request.PostId);

                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found.");
                }

                
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
                        IsFavorite = request.IsFavorite,
                        ReadDate = DateTime.UtcNow
                    };

                    await _repository.AddPostUserActionAsync(postUserAction);
                }
                else
                {
                    postUserAction.IsFavorite = request.IsFavorite;
                    await _repository.UpdatePostUserActionAsync(postUserAction);
                }

                return _mapper.Map<PostDetailDto>(post);
            }
        }
    }
}