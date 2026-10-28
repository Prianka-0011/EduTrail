namespace EduTrail.Application.QuestionCatalog
{
    // Result of compiling and running one program.
    public record RunCodeResult(
        bool Compiled,
        string CompileOutput,
        string Stdout,
        string Stderr,
        bool TimedOut,
        int ExitCode);

    // One grading test case (stdin -> expected stdout).
    public record CodeTestCase(string Input, string Expected);

    // Outcome of one graded case.
    public record CodeCaseResult(bool Passed, string Stdout);

    // Result of grading code against a set of cases.
    public record GradeCodeResult(
        bool Compiled,
        string CompileOutput,
        List<CodeCaseResult> Results);

    // Talks to the isolated code-runner service.
    public interface ICodeRunnerClient
    {
        Task<RunCodeResult> RunAsync(string language, string code, string stdin);
        Task<GradeCodeResult> GradeAsync(string language, string code, IReadOnlyList<CodeTestCase> tests);
    }
}
