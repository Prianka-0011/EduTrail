using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Returns the stored correct answers for an instance (for "Show Correct Answer").
    // Code questions (pl-code-editor) are stripped: their "correct answers" are the
    // grading test cases, which must never be exposed to the student (grading oracle).
    public class GetCorrectAnswersQuery : IRequest<JsonElement?>
    {
        public Guid InstanceId { get; set; }

        public class Handler : IRequestHandler<GetCorrectAnswersQuery, JsonElement?>
        {
            private static readonly Regex CodeEditorName = new(
                @"<pl-code-editor\b[^>]*\banswers-name\s*=\s*""([^""]+)""",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            private readonly IQuestionInstanceStore _store;
            private readonly IQuestionCatalogReader _reader;

            public Handler(IQuestionInstanceStore store, IQuestionCatalogReader reader)
            {
                _store = store;
                _reader = reader;
            }

            public async Task<JsonElement?> Handle(GetCorrectAnswersQuery request, CancellationToken cancellationToken)
            {
                var instance = _store.Get(request.InstanceId);
                if (instance is null) return null;

                var files = await _reader.GetQuestionFilesAsync(instance.Slug);
                var codeNames = files is null
                    ? new HashSet<string>()
                    : CodeEditorName.Matches(files.QuestionHtml)
                        .Select(m => m.Groups[1].Value)
                        .ToHashSet();

                // Nothing to hide — return as-is.
                if (codeNames.Count == 0 || instance.CorrectAnswers.ValueKind != JsonValueKind.Object)
                    return instance.CorrectAnswers;

                var node = JsonNode.Parse(instance.CorrectAnswers.GetRawText())!.AsObject();
                foreach (var name in codeNames)
                    node.Remove(name);

                return JsonSerializer.Deserialize<JsonElement>(node.ToJsonString());
            }
        }
    }
}
