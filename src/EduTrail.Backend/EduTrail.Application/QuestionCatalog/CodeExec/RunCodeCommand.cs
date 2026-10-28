using EduTrail.Application.Shared;
using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Compile + run a snippet once (the Run button), returning its output.
    public class RunCodeCommand : IRequest<RunCodeResult>
    {
        public string Language { get; set; } = "cpp";
        public string Code { get; set; } = string.Empty;
        public string Stdin { get; set; } = string.Empty;

        public class Handler : IRequestHandler<RunCodeCommand, RunCodeResult>
        {
            private readonly ICodeRunnerClient _runner;
            private readonly ICodeRunConcurrencyLimiter _limiter;
            private readonly ICurrentUserService _currentUser;

            public Handler(ICodeRunnerClient runner, ICodeRunConcurrencyLimiter limiter, ICurrentUserService currentUser)
            {
                _runner = runner;
                _limiter = limiter;
                _currentUser = currentUser;
            }

            public Task<RunCodeResult> Handle(RunCodeCommand request, CancellationToken cancellationToken)
                => _limiter.RunAsync(
                    _currentUser.GetUserId(),
                    () => _runner.RunAsync(request.Language, request.Code, request.Stdin));
        }
    }
}
