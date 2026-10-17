using System.Text;
using System.Text.Json;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Calls the Python sandbox service to run a question's server.py generate().
    public class PythonQuestionClient : IPythonQuestionClient
    {
        private readonly HttpClient _httpClient;

        public PythonQuestionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GenerateResult> GenerateAsync(string serverPy)
        {
            var payload = JsonSerializer.Serialize(new { server_py = serverPy });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/generate", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Python service error: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GenerateResult>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Empty response from Python service");
        }
    }
}
