namespace EduTrail.Application.QuestionCatalog
{
    // A new question's contents, as entered in the Add Question form.
    public record QuestionDraft(
        string Title,
        string Topic,
        List<string> Tags,
        string QuestionHtml,
        string ServerPy);

    // Writes a new question folder (info.json / question.html / server.py) to disk.
    public interface IQuestionCatalogWriter
    {
        // Returns the generated slug, or null if the questions directory can't be resolved.
        Task<string?> CreateQuestionAsync(QuestionDraft draft);

        // Deletes a question folder. Returns true if removed, false if the slug is
        // invalid or the folder doesn't exist.
        bool DeleteQuestion(string slug);
    }
}
