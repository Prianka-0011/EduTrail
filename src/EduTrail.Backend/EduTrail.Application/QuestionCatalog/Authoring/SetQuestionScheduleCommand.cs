using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Outcome of assigning a question's week/day.
    public enum SetQuestionScheduleOutcome { Updated, NotFound, Forbidden, Invalid }

    // Sets which week/day a question belongs to. Instructor-only, matching the other
    // authoring actions; the role check lives in the handler because roles aren't in the JWT.
    public class SetQuestionScheduleCommand : IRequest<SetQuestionScheduleOutcome>
    {
        public string Slug { get; set; } = string.Empty;
        public int? Week { get; set; }
        public int? Day { get; set; }

        public class Handler : IRequestHandler<SetQuestionScheduleCommand, SetQuestionScheduleOutcome>
        {
            private readonly IQuestionCatalogWriter _writer;
            private readonly IUserRepository _userRepository;
            private readonly ICurrentUserService _currentUser;

            public Handler(IQuestionCatalogWriter writer, IUserRepository userRepository, ICurrentUserService currentUser)
            {
                _writer = writer;
                _userRepository = userRepository;
                _currentUser = currentUser;
            }

            public async Task<SetQuestionScheduleOutcome> Handle(SetQuestionScheduleCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(_currentUser.GetUserId());
                var isInstructor = user?.Roles?.Any(r => r.Id == CustomCategory.RoleType.Instructor) == true;
                if (!isInstructor)
                    return SetQuestionScheduleOutcome.Forbidden;

                // Week/day are 1-based when present; null clears the assignment.
                if ((request.Week is < 1) || (request.Day is < 1))
                    return SetQuestionScheduleOutcome.Invalid;

                return await _writer.SetScheduleAsync(request.Slug, request.Week, request.Day)
                    ? SetQuestionScheduleOutcome.Updated
                    : SetQuestionScheduleOutcome.NotFound;
            }
        }
    }
}
