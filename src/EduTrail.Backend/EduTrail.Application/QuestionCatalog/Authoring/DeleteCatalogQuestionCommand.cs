using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Outcome of a delete attempt.
    public enum DeleteCatalogQuestionOutcome { Deleted, NotFound, Forbidden }

    // Deletes a file-based question. Instructor-only, matching Add Question.
    public class DeleteCatalogQuestionCommand : IRequest<DeleteCatalogQuestionOutcome>
    {
        public string Slug { get; set; } = string.Empty;

        public class Handler : IRequestHandler<DeleteCatalogQuestionCommand, DeleteCatalogQuestionOutcome>
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

            public async Task<DeleteCatalogQuestionOutcome> Handle(DeleteCatalogQuestionCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(_currentUser.GetUserId());
                var isInstructor = user?.Roles?.Any(r => r.Id == CustomCategory.RoleType.Instructor) == true;
                if (!isInstructor)
                    return DeleteCatalogQuestionOutcome.Forbidden;

                return _writer.DeleteQuestion(request.Slug)
                    ? DeleteCatalogQuestionOutcome.Deleted
                    : DeleteCatalogQuestionOutcome.NotFound;
            }
        }
    }
}
