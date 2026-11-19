using EduTrail.Application.Shared;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Generates a new instance of a question: runs server.py, stores the correct
    // answers server-side, and returns the rendered question.html.
    public class CreateQuestionInstanceCommand : IRequest<QuestionInstanceDto?>
    {
        public string Slug { get; set; } = string.Empty;

        public class Handler : IRequestHandler<CreateQuestionInstanceCommand, QuestionInstanceDto?>
        {
            private readonly IQuestionCatalogReader _reader;
            private readonly IPythonQuestionClient _python;
            private readonly IQuestionInstanceStore _store;
            private readonly IQuestionScoreStore _scores;
            private readonly ICurrentUserService _currentUser;

            public Handler(
                IQuestionCatalogReader reader,
                IPythonQuestionClient python,
                IQuestionInstanceStore store,
                IQuestionScoreStore scores,
                ICurrentUserService currentUser)
            {
                _reader = reader;
                _python = python;
                _store = store;
                _scores = scores;
                _currentUser = currentUser;
            }

            public async Task<QuestionInstanceDto?> Handle(CreateQuestionInstanceCommand request, CancellationToken cancellationToken)
            {
                var files = await _reader.GetQuestionFilesAsync(request.Slug);
                if (files is null) return null;

                var generated = await _python.GenerateAsync(files.ServerPy);

                var instance = _store.Save(request.Slug, generated.Params, generated.CorrectAnswers);
                var renderedHtml = ParamSubstitution.Substitute(files.QuestionHtml, generated.Params);

                // Surface the user's previous best for this question so the player can show it.
                var bestScore = await _scores.GetBestScoreAsync(
                    _currentUser.GetUserId(), request.Slug, cancellationToken);

                return new QuestionInstanceDto(instance.Id, renderedHtml, generated.Params, ShowCorrectAnswer: true, BestScore: bestScore);
            }
        }
    }
}
