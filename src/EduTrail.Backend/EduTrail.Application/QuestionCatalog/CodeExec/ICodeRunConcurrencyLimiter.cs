namespace EduTrail.Application.QuestionCatalog
{
    // Thrown when a user has too many code executions already in flight.
    public class CodeRunLimitExceededException : Exception
    {
        public CodeRunLimitExceededException(string message) : base(message) { }
    }

    // Bounds concurrent code executions: a global cap across all users and a
    // smaller per-user cap (over the per-user cap throws CodeRunLimitExceededException).
    public interface ICodeRunConcurrencyLimiter
    {
        Task<T> RunAsync<T>(Guid userId, Func<Task<T>> action);
    }
}
