namespace EduTrail.Application.QuestionCatalog
{
    // Persists each user's best (maximum) score per catalog question (keyed by slug).
    public interface IQuestionScoreStore
    {
        // The user's stored best score for a question, or null if they've never scored it.
        Task<float?> GetBestScoreAsync(Guid userId, string slug, CancellationToken cancellationToken = default);

        // Records a score if it beats the stored best; returns the resulting best score.
        Task<float> RecordBestScoreAsync(Guid userId, string slug, float score, CancellationToken cancellationToken = default);
    }
}
