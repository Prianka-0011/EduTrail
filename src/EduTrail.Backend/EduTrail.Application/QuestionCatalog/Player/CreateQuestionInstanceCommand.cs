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

            public Handler(IQuestionCatalogReader reader, IPythonQuestionClient python, IQuestionInstanceStore store)
            {
                _reader = reader;
                _python = python;
                _store = store;
            }

            public async Task<QuestionInstanceDto?> Handle(CreateQuestionInstanceCommand request, CancellationToken cancellationToken)
            {
                var files = await _reader.GetQuestionFilesAsync(request.Slug);
                if (files is null) return null;

                var generated = await _python.GenerateAsync(files.ServerPy);

                var instance = _store.Save(request.Slug, generated.Params, generated.CorrectAnswers);
                var renderedHtml = ParamSubstitution.Substitute(files.QuestionHtml, generated.Params);

                return new QuestionInstanceDto(instance.Id, renderedHtml, generated.Params, ShowCorrectAnswer: true);
            }
        }
    }
}
