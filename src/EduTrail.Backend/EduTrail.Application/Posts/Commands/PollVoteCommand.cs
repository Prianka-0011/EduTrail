using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;
using EduTrail.Application.Shared;
using EduTrail.Application.UserDashboards;

namespace EduTrail.Application.Posts
{
    public class PollVoteCommand : IRequest<PollVoteDto>
    {
        public PollVoteDto PollVoteDto { get; set; } = new();


        public class Handler : IRequestHandler<PollVoteCommand, PollVoteDto>
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


            public async Task<PollVoteDto> Handle(
                PollVoteCommand request,
                CancellationToken cancellationToken)
            {
                var dto = request.PollVoteDto;

                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolement = await _courseRepository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.PollVoteDto.CourseOfferingId);

                // Check poll option exists
                var option =
                    await _repository.GetPollOptionByIdAsync(
                        dto.PollOptionId);


                if (option == null)
                {
                    throw new Exception(
                        "Poll option not found");
                }

                // Check duplicate vote
                
                var existingVote =
                    await _repository
                    .GetPollVoteByEnrollmentAsync(
                        enrolement.Id,
                        option.PollId);


                if (existingVote != null)
                {
                    throw new Exception(
                        "You already voted on this poll");
                }


                var vote = new PollVote
                {
                    // Id = Guid.NewGuid(),

                    PollOptionId = dto.PollOptionId,

                    EnrollmentId = enrolement.Id,

                    VotedAt = DateTimeOffset.UtcNow
                };


                // Increase option vote count
                option.VoteCount++;


                await _repository.AddPollVoteAsync(vote);


                await _repository.UpdatePollOptionAsync(option);


                return _mapper.Map<PollVoteDto>(vote);
            }
        }
    }
}