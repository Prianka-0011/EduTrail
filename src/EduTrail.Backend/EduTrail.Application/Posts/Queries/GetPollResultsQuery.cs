using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.UserDashboards;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class GetPollResultsQuery : IRequest<PollResultDto>
    {
        public Guid PollId { get; set; }
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<GetPollResultsQuery, PollResultDto>
        {
            private readonly IPostRepository _postRepository;
            private ICommonService _commonService;
            private IMapper _mapper;
            private readonly IUserCourseOfferingRepository _courseRepository;

            public Handler(
                IPostRepository postRepository,
                IMapper mapper,
                ICommonService commonService,
                IUserCourseOfferingRepository courseRepository)
            {
                _postRepository = postRepository;
               
                _mapper = mapper;
                _commonService = commonService;
                _courseRepository = courseRepository;
            }

            public async Task<PollResultDto> Handle(
                GetPollResultsQuery request,
                CancellationToken cancellationToken)
            {
                var poll = await _postRepository.GetPollByIdAsync(request.PollId);

                if (poll == null)
                {
                    return null!;
                }
                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.CourseOfferingId);


                var currentUserVote =
                    await _postRepository
                        .GetPollVoteByEnrollmentAsync(
                            enrolement.Id,
                            request.PollId);

                var totalVotes = poll.Options.Sum(o => o.VoteCount);

                var resDto = new PollResultDto
                {
                    PollId = poll.Id,
                    TotalVotes = totalVotes,
                    Options = poll.Options.Select(o => new PollOptionResultDto
                    {
                        Id = o.Id,
                        OptionText = o.OptionText,
                        VoteCount = o.VoteCount,
                        Percentage = totalVotes == 0
                            ? 0
                            : Math.Round((double)o.VoteCount * 100 / totalVotes, 2),
                            IsSelectedByCurrentUser =
                                currentUserVote != null &&
                                currentUserVote.PollOptionId == o.Id
                    }).ToList(),
                    IsCurrentUserVoted = currentUserVote!=null?true:false
                };
                return resDto;
            }
        }
    }
}