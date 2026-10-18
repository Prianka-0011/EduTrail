using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Scans the questions folder at request time and builds catalog entries from each
    // folder's info.json. A folder is only treated as a question when it has info.json,
    // question.html and server.py, matching the reference PrairieLearn layout.
    public class QuestionCatalogReader : IQuestionCatalogReader, IQuestionCatalogWriter
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

        public async Task<QuestionFiles?> GetQuestionFilesAsync(string slug)
        {
            // Only allow simple slugs; blocks path traversal (e.g. "../secrets").
            if (string.IsNullOrWhiteSpace(slug) || !Regex.IsMatch(slug, "^[a-z0-9-]+$"))
                return null;

            var questionsPath = ResolveQuestionsPath();
            if (questionsPath is null) return null;

            var dir = Path.Combine(questionsPath, slug);
            var pyPath = Path.Combine(dir, "server.py");
            var htmlPath = Path.Combine(dir, "question.html");

            if (!File.Exists(pyPath) || !File.Exists(htmlPath))
                return null;

            var serverPy = await File.ReadAllTextAsync(pyPath);
            var questionHtml = await File.ReadAllTextAsync(htmlPath);
            return new QuestionFiles(serverPy, questionHtml);
        }

        public async Task<string?> CreateQuestionAsync(QuestionDraft draft)
        {
            var questionsPath = ResolveQuestionsPath();
            if (questionsPath is null) return null;

            // Derive a filesystem-safe slug from the title; slugify only yields [a-z0-9-],
            // so the resulting path can't escape the questions directory.
            var baseSlug = Slugify(draft.Title);
            if (string.IsNullOrEmpty(baseSlug)) baseSlug = "question";

            var slug = baseSlug;
            var counter = 2;
            while (Directory.Exists(Path.Combine(questionsPath, slug)))
                slug = $"{baseSlug}-{counter++}";

            var dir = Path.Combine(questionsPath, slug);
            Directory.CreateDirectory(dir);

            var info = new
            {
                uuid = Guid.NewGuid().ToString(),
                title = draft.Title,
                topic = draft.Topic,
                tags = draft.Tags,
                type = "v3",
                showCorrectAnswer = true
            };

            var jsonOpts = new JsonSerializerOptions { WriteIndented = true };
            await File.WriteAllTextAsync(Path.Combine(dir, "info.json"), JsonSerializer.Serialize(info, jsonOpts));
            await File.WriteAllTextAsync(Path.Combine(dir, "question.html"), draft.QuestionHtml);
            await File.WriteAllTextAsync(Path.Combine(dir, "server.py"), draft.ServerPy);

            return slug;
        }

        private static string Slugify(string title) =>
            Regex.Replace(title.ToLowerInvariant().Trim(), "[^a-z0-9]+", "-").Trim('-');

        public bool DeleteQuestion(string slug)
        {
            // Reject anything other than a simple slug before touching the filesystem.
            if (string.IsNullOrWhiteSpace(slug) || !Regex.IsMatch(slug, "^[a-z0-9-]+$"))
                return false;

            var questionsPath = ResolveQuestionsPath();
            if (questionsPath is null) return false;

            var root = Path.GetFullPath(questionsPath);
            var dir = Path.GetFullPath(Path.Combine(root, slug));

            // Containment check: the resolved path must be strictly inside the questions root.
            var rootWithSep = root.EndsWith(Path.DirectorySeparatorChar) ? root : root + Path.DirectorySeparatorChar;
            if (!dir.StartsWith(rootWithSep, StringComparison.Ordinal))
                return false;

            if (!Directory.Exists(dir)) return false;

            Directory.Delete(dir, recursive: true);
            return true;
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
