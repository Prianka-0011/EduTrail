using System.Text.Json;
using EduTrail.Application.Shared;
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
            private readonly IQuestionScoreStore _scores;
            private readonly ICurrentUserService _currentUser;

            public Handler(
                IQuestionInstanceStore store,
                IQuestionCatalogReader reader,
                IQuestionGrader grader,
                IQuestionScoreStore scores,
                ICurrentUserService currentUser)
            {
                _store = store;
                _reader = reader;
                _grader = grader;
                _scores = scores;
                _currentUser = currentUser;
            }

            public async Task<SubmissionResultDto?> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
            {
                var instance = _store.Get(request.InstanceId);
                if (instance is null) return null;

                var files = await _reader.GetQuestionFilesAsync(instance.Slug);
                if (files is null) return null;

                var feedback = await _grader.GradeAsync(files.QuestionHtml, request.Answers, instance.CorrectAnswers);

                var overallScore = feedback.Count > 0
                    ? feedback.Values.Average(f => f.Score)
                    : 0f;

                // Persist only the maximum score per (user, question); returns the best so far.
                var bestScore = await _scores.RecordBestScoreAsync(
                    _currentUser.GetUserId(), instance.Slug, overallScore, cancellationToken);

                return new SubmissionResultDto(Guid.NewGuid(), overallScore, feedback, bestScore);
            }
        }
    }
}
