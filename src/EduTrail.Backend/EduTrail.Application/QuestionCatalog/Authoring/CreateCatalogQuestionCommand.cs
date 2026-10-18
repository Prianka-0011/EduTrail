using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Result of an Add Question attempt: a slug on success, an error message on bad
    // input, or Forbidden when the caller isn't an instructor.
    public record CreateCatalogQuestionResult(string? Slug, string? Error, bool Forbidden);

    // Creates a new file-based question. Restricted to instructors because the body
    // includes server.py, which the sandbox executes.
    public class CreateCatalogQuestionCommand : IRequest<CreateCatalogQuestionResult>
    {
        public string Title { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string QuestionHtml { get; set; } = string.Empty;
        public string ServerPy { get; set; } = string.Empty;

        public class Handler : IRequestHandler<CreateCatalogQuestionCommand, CreateCatalogQuestionResult>
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

            public async Task<CreateCatalogQuestionResult> Handle(CreateCatalogQuestionCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(_currentUser.GetUserId());
                var isInstructor = user?.Roles?.Any(r => r.Id == CustomCategory.RoleType.Instructor) == true;
                if (!isInstructor)
                    return new CreateCatalogQuestionResult(null, null, Forbidden: true);

                if (string.IsNullOrWhiteSpace(request.Title))
                    return new CreateCatalogQuestionResult(null, "Title is required.", false);
                if (string.IsNullOrWhiteSpace(request.QuestionHtml))
                    return new CreateCatalogQuestionResult(null, "question.html is required.", false);
                if (string.IsNullOrWhiteSpace(request.ServerPy))
                    return new CreateCatalogQuestionResult(null, "server.py is required.", false);

                var tags = request.Tags
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();

                var draft = new QuestionDraft(
                    request.Title.Trim(),
                    request.Topic.Trim(),
                    tags,
                    request.QuestionHtml,
                    request.ServerPy);

                var slug = await _writer.CreateQuestionAsync(draft);
                return slug is null
                    ? new CreateCatalogQuestionResult(null, "Could not locate the questions directory.", false)
                    : new CreateCatalogQuestionResult(slug, null, false);
            }
        }
    }
}
