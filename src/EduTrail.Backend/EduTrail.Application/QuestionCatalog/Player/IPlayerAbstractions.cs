using System.Text.Json;

namespace EduTrail.Application.QuestionCatalog
{
    // The on-disk files that make up a question.
    public record QuestionFiles(string ServerPy, string QuestionHtml);

    // A generated instance held server-side between creation and grading.
    public class StoredQuestionInstance
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public JsonElement Params { get; set; }
        public JsonElement CorrectAnswers { get; set; }
    }

    // Runs a question's server.py in the Python sandbox service.
    public interface IPythonQuestionClient
    {
        Task<GenerateResult> GenerateAsync(string serverPy);
    }

    // Holds generated instances so a later submission can be graded against them.
    public interface IQuestionInstanceStore
    {
        StoredQuestionInstance Save(string slug, JsonElement @params, JsonElement correctAnswers);
        StoredQuestionInstance? Get(Guid id);
    }

    // Grades a submission against the stored correct answers for a question.
    public interface IQuestionGrader
    {
        Dictionary<string, FieldFeedbackDto> Grade(string questionHtml, JsonElement submittedAnswers, JsonElement correctAnswers);
    }
}
