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
    // BestScore is the signed-in user's stored best for this question (null if none yet).
    public record QuestionInstanceDto(
        Guid InstanceId,
        string RenderedHtml,
        JsonElement Params,
        bool ShowCorrectAnswer,
        float? BestScore
    );

    // Per-field grading feedback.
    public record FieldFeedbackDto(bool IsCorrect, float Score, string Message);

    // Result of grading a submission. BestScore is the user's stored best after this
    // attempt is recorded (>= Score; unchanged if this attempt didn't beat the old best).
    public record SubmissionResultDto(
        Guid SubmissionId,
        float Score,
        Dictionary<string, FieldFeedbackDto> Feedback,
        float? BestScore
    );
}
