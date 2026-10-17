using System.Text.Json;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Returns the stored correct answers for an instance (for "Show Correct Answer").
    public class GetCorrectAnswersQuery : IRequest<JsonElement?>
    {
        public Guid InstanceId { get; set; }

        public class Handler : IRequestHandler<GetCorrectAnswersQuery, JsonElement?>
        {
            private readonly IQuestionInstanceStore _store;

            public Handler(IQuestionInstanceStore store)
            {
                _store = store;
            }

            public Task<JsonElement?> Handle(GetCorrectAnswersQuery request, CancellationToken cancellationToken)
            {
                var instance = _store.Get(request.InstanceId);
                return Task.FromResult<JsonElement?>(instance?.CorrectAnswers);
            }
        }
    }
}
