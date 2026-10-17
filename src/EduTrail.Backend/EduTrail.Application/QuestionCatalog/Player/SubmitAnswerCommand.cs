using System.Text.Json;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Grades a submitted set of answers against a stored instance's correct answers.
    public class SubmitAnswerCommand : IRequest<SubmissionResultDto?>
    {
        public Guid InstanceId { get; set; }
        public JsonElement Answers { get; set; }

        public class Handler : IRequestHandler<SubmitAnswerCommand, SubmissionResultDto?>
        {
            private readonly IQuestionInstanceStore _store;
            private readonly IQuestionCatalogReader _reader;
            private readonly IQuestionGrader _grader;

            public Handler(IQuestionInstanceStore store, IQuestionCatalogReader reader, IQuestionGrader grader)
            {
                _store = store;
                _reader = reader;
                _grader = grader;
            }

            public async Task<SubmissionResultDto?> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
            {
                var instance = _store.Get(request.InstanceId);
                if (instance is null) return null;

                var files = await _reader.GetQuestionFilesAsync(instance.Slug);
                if (files is null) return null;

                var feedback = _grader.Grade(files.QuestionHtml, request.Answers, instance.CorrectAnswers);

                var overallScore = feedback.Count > 0
                    ? feedback.Values.Average(f => f.Score)
                    : 0f;

                return new SubmissionResultDto(Guid.NewGuid(), overallScore, feedback);
            }
        }
    }
}
