using System.Text;
using System.Text.Json;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // HTTP client for the code-runner service. The shared-secret header is attached
    // at registration time (see Program.cs), so this class only shapes the payloads.
    public class CodeRunnerClient : ICodeRunnerClient
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public CodeRunnerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<RunCodeResult> RunAsync(string language, string code, string stdin)
            => PostAsync<RunCodeResult>("/run-code", new { language, code, stdin });

        public Task<GradeCodeResult> GradeAsync(string language, string code, IReadOnlyList<CodeTestCase> tests)
            => PostAsync<GradeCodeResult>("/grade-code", new
            {
                language,
                code,
                tests = tests.Select(t => new { input = t.Input, expected = t.Expected })
            });

        private async Task<T> PostAsync<T>(string path, object payload)
        {
            var content = new StringContent(JsonSerializer.Serialize(payload, Json), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(path, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Code runner error ({(int)response.StatusCode}): {error}");
            }

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, Json)
                ?? throw new InvalidOperationException("Empty response from code runner");
        }
    }
}
