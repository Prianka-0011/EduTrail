using System.Collections.Concurrent;
using System.Text.Json;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Holds generated question instances in memory. Instances are lost on restart,
    // which is acceptable for the practice/browse player. Registered as a singleton.
    public class InMemoryQuestionInstanceStore : IQuestionInstanceStore
    {
        private readonly ConcurrentDictionary<Guid, StoredQuestionInstance> _instances = new();

        public StoredQuestionInstance Save(string slug, JsonElement @params, JsonElement correctAnswers)
        {
            var instance = new StoredQuestionInstance
            {
                Id = Guid.NewGuid(),
                Slug = slug,
                // Clone so the values stay valid after the originating JsonDocument is disposed.
                Params = @params.Clone(),
                CorrectAnswers = correctAnswers.Clone()
            };
            _instances[instance.Id] = instance;
            return instance;
        }

        public StoredQuestionInstance? Get(Guid id)
            => _instances.TryGetValue(id, out var instance) ? instance : null;
    }
}
