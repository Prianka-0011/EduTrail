using System.Text.Json;
using Microsoft.Extensions.Configuration;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Scans the questions folder at request time and builds catalog entries from each
    // folder's info.json. A folder is only treated as a question when it has info.json,
    // question.html and server.py, matching the reference PrairieLearn layout.
    public class QuestionCatalogReader : IQuestionCatalogReader
    {
        private readonly IConfiguration _configuration;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public QuestionCatalogReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<QuestionCatalogItemDto>> GetCatalogAsync()
        {
            var items = new List<QuestionCatalogItemDto>();

            var questionsPath = ResolveQuestionsPath();
            if (questionsPath is null) return items;

            foreach (var dir in Directory.GetDirectories(questionsPath))
            {
                var infoPath = Path.Combine(dir, "info.json");
                var htmlPath = Path.Combine(dir, "question.html");
                var pyPath = Path.Combine(dir, "server.py");

                if (!File.Exists(infoPath) || !File.Exists(htmlPath) || !File.Exists(pyPath))
                    continue;

                InfoJson? info;
                try
                {
                    var json = await File.ReadAllTextAsync(infoPath);
                    info = JsonSerializer.Deserialize<InfoJson>(json, JsonOptions);
                }
                catch (JsonException)
                {
                    // Skip a question whose info.json is malformed rather than failing the whole list.
                    continue;
                }

                if (info is null) continue;

                items.Add(new QuestionCatalogItemDto
                {
                    Slug = Path.GetFileName(dir),
                    Title = string.IsNullOrWhiteSpace(info.Title) ? Path.GetFileName(dir) : info.Title,
                    Topic = info.Topic ?? string.Empty,
                    Tags = info.Tags ?? new List<string>()
                });
            }

            return items
                .OrderBy(i => i.Title, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        // Finds the questions directory. An absolute QuestionCatalog:QuestionsPath is used as-is;
        // a relative one (default "questions") is searched for by walking up from the app's
        // output directory and the current working directory, so it resolves whether the API is
        // launched from the repo root or elsewhere.
        private string? ResolveQuestionsPath()
        {
            var configured = _configuration["QuestionCatalog:QuestionsPath"];
            if (string.IsNullOrWhiteSpace(configured)) configured = "questions";

            if (Path.IsPathRooted(configured))
                return Directory.Exists(configured) ? configured : null;

            foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
            {
                var dir = new DirectoryInfo(start);
                while (dir is not null)
                {
                    var candidate = Path.Combine(dir.FullName, configured);
                    if (Directory.Exists(candidate)) return candidate;
                    dir = dir.Parent;
                }
            }

            return null;
        }

        // Shape of a question's info.json; only the fields the catalog needs.
        private sealed class InfoJson
        {
            public string? Title { get; set; }
            public string? Topic { get; set; }
            public List<string>? Tags { get; set; }
        }
    }
}
