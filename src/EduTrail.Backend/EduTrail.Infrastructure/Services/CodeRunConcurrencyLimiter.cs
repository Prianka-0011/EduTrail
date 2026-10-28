using System.Collections.Concurrent;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Caps concurrent code executions: at most GlobalMax across the whole service
    // and PerUserMax per user. Registered as a singleton so the counts are shared.
    public class CodeRunConcurrencyLimiter : ICodeRunConcurrencyLimiter
    {
        private const int GlobalMax = 8;
        private const int PerUserMax = 2;

        private readonly SemaphoreSlim _global = new(GlobalMax, GlobalMax);
        private readonly ConcurrentDictionary<Guid, int> _perUser = new();

        public async Task<T> RunAsync<T>(Guid userId, Func<Task<T>> action)
        {
            // Reserve a per-user slot (reject rather than queue when over the cap).
            var updated = _perUser.AddOrUpdate(userId, 1, (_, n) => n + 1);
            if (updated > PerUserMax)
            {
                Release(userId);
                throw new CodeRunLimitExceededException("Too many code runs in progress for this user. Please wait for the current run to finish.");
            }

            await _global.WaitAsync();
            try
            {
                return await action();
            }
            finally
            {
                _global.Release();
                Release(userId);
            }
        }

        private void Release(Guid userId)
        {
            _perUser.AddOrUpdate(userId, 0, (_, n) => n > 0 ? n - 1 : 0);
        }
    }
}
