using System.Text.Json;
using System.Text.Json.Serialization;

namespace EduTrail.Application.QuestionCatalog
{
    // Raw result of running a question's server.py generate() in the Python sandbox.
    public class GenerateResult
    {
        [JsonPropertyName("params")]
        public JsonElement Params { get; set; }

        [JsonPropertyName("correct_answers")]
        public JsonElement CorrectAnswers { get; set; }
    }

    // A rendered question instance returned to the frontend player.
    public record QuestionInstanceDto(
        Guid InstanceId,
        string RenderedHtml,
        JsonElement Params,
        bool ShowCorrectAnswer
    );

    // Per-field grading feedback.
    public record FieldFeedbackDto(bool IsCorrect, float Score, string Message);

    // Result of grading a submission.
    public record SubmissionResultDto(
        Guid SubmissionId,
        float Score,
        Dictionary<string, FieldFeedbackDto> Feedback
    );
}
