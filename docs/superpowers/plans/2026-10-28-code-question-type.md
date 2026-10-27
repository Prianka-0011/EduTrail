# C/C++ Code Question Type Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a question type where a student writes C/C++ in an in-browser editor, runs it to see output, and submits for grading against test cases — each compile/run isolated in a short-lived Docker container.

**Architecture:** A **separate, dedicated `code-runner` service** (FastAPI, port 8002) exposes Docker-backed `/run-code` and `/grade-code` endpoints (compile + run `g++` inside a locked-down container). It is kept distinct from `python-service` because it has the opposite trust profile — it runs *untrusted student code* and is the only component with Docker access (≈ host root), so it gets its own process, port, shared-secret auth, and rate limits. `python-service` is unchanged. The .NET backend forwards Run requests and, on Submit, grades `pl-code-editor` answers by calling the runner. The frontend adds a CodeMirror-based `pl-code-editor` component that Runs code and emits it as the answer. Instances stay in-memory.

**Tech Stack:** Python 3.12 / FastAPI, Docker (`gcc:13`), .NET 10 / MediatR, Angular 19 standalone + CodeMirror 6.

**Spec:** `docs/superpowers/specs/2026-10-28-code-question-type-design.md`

## Global Constraints

- Student code is untrusted; every compile/run uses: `--rm --network none --memory=256m --memory-swap=256m --cpus=1 --pids-limit=64 --read-only --tmpfs /work:size=32m,exec --user 1000:1000 --cap-drop ALL --security-opt no-new-privileges`, host-enforced timeouts (compile 5 s, run 3 s), stdout/stderr truncated to 64 KB each.
- Runner image: `gcc:13`. Source written to a host temp dir mounted read-only at `/src`; compilation output and binary live in the writable `/work` tmpfs.
- New backend code lives under the existing `QuestionCatalog` feature; follow MediatR command/handler + typed-HttpClient + `[Authorize]` patterns already there.
- CodeMirror must initialize **client-only** (guard with `isPlatformBrowser`); WebSpa uses Angular SSR.
- Test-case comparison is `trim(stdout) == trim(expected)`; score = passed / total; compile failure = score 0.
- The `code-runner` service is a **separate process on port 8002**, bound to `127.0.0.1`. Config: `CodeRunner:BaseUrl` (default `http://localhost:8002`), `CodeRunner:Secret`.
- **Both internal services stay localhost-only:** `code-runner` (8002) and the existing `python-service` (8001) bind to `127.0.0.1` and are never exposed off-box; only the .NET backend calls them. (uvicorn's default host is already `127.0.0.1`; don't pass `--host 0.0.0.0`.)
- **Shared-secret auth:** the backend sends header `X-Runner-Secret: <CodeRunner:Secret>` on every runner call; the runner rejects any request missing/mismatching it with HTTP 401. The secret is read from env `CODE_RUNNER_SECRET` on the runner side.
- **Rate limits (abuse control):** per authenticated user, max 10 code executions / minute and max 2 concurrent in flight (Run and Submit both count); a global cap of 8 concurrent runner calls. Over-limit → HTTP 429 with a clear message.

## Review Focus

- **Compile error** (invalid C++): Run shows the compiler message; Submit scores 0 with the message, no server error. → Task 1, Task 6.
- **Infinite loop / timeout:** container is killed at the run timeout; result marked `timedOut`, not a hung request. → Task 1.
- **Oversized output** (program prints megabytes): output truncated to 64 KB, request still returns. → Task 1.
- **Empty / whitespace-only submission:** compile fails cleanly → score 0, friendly message; no crash. → Task 6.
- **Runner/sandbox unavailable** (service down or Docker missing): Run and Submit surface a clear error, backend returns 502-style message rather than 500. → Task 4, Task 5.
- **Abuse / flooding** (a logged-in user scripts many runs): over the per-user or global limit returns HTTP 429, host stays responsive. → Task 9.
- **Direct runner access without the secret** (request to :8002 not from the backend): rejected with 401. → Task 3.

---

### Task 1: `code-runner` service — compile & run one program in Docker

**Files:**
- Create: `code-runner/runner.py`
- Create: `code-runner/requirements.txt` (`fastapi`, `uvicorn[standard]`)
- Test: `code-runner/tests/test_runner.py`

**Interfaces:**
- Produces: `run_code(language: str, code: str, stdin: str) -> dict` returning
  `{"compiled": bool, "compileOutput": str, "stdout": str, "stderr": str, "timedOut": bool, "exitCode": int}`.
  Only `language == "cpp"` supported now; others raise `ValueError`.

- [ ] **Step 1: Write failing tests** in `test_runner.py` (require Docker; skip with `pytest.mark.skipif` when `docker` CLI absent):
  - `test_runs_and_returns_stdout`: a program echoing `a+b` from stdin `"3 4"` → `stdout.strip() == "7"`, `compiled is True`, `exitCode == 0`.
  - `test_compile_error_reported`: `code="int main(){"` → `compiled is False`, `compileOutput` non-empty.
  - `test_timeout_flagged`: `int main(){while(1){}}` → `timedOut is True` within the run timeout.
- [ ] **Step 2: Run** (from `code-runner/`) `python -m pytest tests/test_runner.py -v` → FAIL (no `runner` module).
- [ ] **Step 3: Implement** `run_code` in `runner.py`: write `code` to a temp dir, `docker run` `gcc:13` with the Global-Constraints flags, mount the temp dir read-only at `/src`, `sh -c "cp /src/main.cpp /work && g++ /work/main.cpp -o /work/a.out 2> /work/cerr && /work/a.out"`, pipe `stdin`, enforce timeouts via `subprocess` `timeout=` wrapped around `docker run` plus a `docker kill`, truncate outputs to 64 KB. Map a compile failure (nonzero g++) to `compiled=False` with `compileOutput`.
- [ ] **Step 4: Run** the tests → PASS.
- [ ] **Step 5: Commit** (`git add code-runner/runner.py code-runner/tests/test_runner.py code-runner/requirements.txt`).

### Task 2: Python grader — compile once, run cases

**Files:**
- Modify: `code-runner/runner.py`
- Test: `code-runner/tests/test_runner.py`

**Interfaces:**
- Produces: `grade_code(language: str, code: str, tests: list[dict]) -> dict` where each test is `{"input": str, "expected": str}`, returning `{"compiled": bool, "compileOutput": str, "results": [{"passed": bool, "stdout": str}]}`. Compiles once, runs each case.

- [ ] **Step 1: Write failing tests**:
  - `test_grade_all_pass`: a correct sum program against `[{"input":"1 2","expected":"3"},{"input":"5 5","expected":"10"}]` → every `results[i].passed is True`.
  - `test_grade_mixed`: a program that always prints `0` → only the `expected=="0"` case passes.
  - `test_grade_compile_error`: → `compiled is False`, `results == []`.
- [ ] **Step 2: Run** → FAIL.
- [ ] **Step 3: Implement** `grade_code`: compile once in a container that keeps `/work/a.out`; for each case run the binary with the case input (reuse a single container via `docker run` per case against a cached image is acceptable for v1 — compile once by building the binary, then run cases). Compare `stdout.strip() == expected.strip()`.
- [ ] **Step 4: Run** → PASS.
- [ ] **Step 5: Commit.**

### Task 3: FastAPI endpoints

**Files:**
- Modify: `code-runner/main.py`
- Test: `code-runner/tests/test_main.py`

**Interfaces:**
- Produces: `POST /run-code` `{language, code, stdin}` → `run_code` result; `POST /grade-code` `{language, code, tests}` → `grade_code` result. `ValueError` → HTTP 422. **Both require header `X-Runner-Secret` matching env `CODE_RUNNER_SECRET`**; missing/mismatched → HTTP 401. App binds to `127.0.0.1:8002`.

- [ ] **Step 1: Write failing tests** with FastAPI `TestClient`:
  - `test_requires_secret`: POST `/run-code` **without** the header → 401 (no Docker needed).
  - (Docker-gated, skip if absent) with the correct header: sum program + stdin `"2 3"` → 200, `stdout.strip()=="5"`; `/grade-code` one passing case → `results[0].passed is True`.
- [ ] **Step 2: Run** `python -m pytest tests/test_main.py -v` → FAIL.
- [ ] **Step 3: Implement** two Pydantic request models, a FastAPI dependency that checks `X-Runner-Secret` against `CODE_RUNNER_SECRET` (401 on mismatch), and the two routes delegating to `runner`. Document the env var in `code-runner/README.md` or a comment.
- [ ] **Step 4: Run** → PASS.
- [ ] **Step 5: Commit.**

### Task 4: Backend code-runner client

**Files:**
- Create: `src/EduTrail.Backend/EduTrail.Application/QuestionCatalog/CodeExec/ICodeRunnerClient.cs` (interface + DTOs `RunCodeResult`, `GradeCodeResult`, `CodeTestCase`)
- Create: `src/EduTrail.Backend/EduTrail.Infrastructure/Services/CodeRunnerClient.cs`
- Modify: `src/EduTrail.Backend/EduTrail.API/Program.cs` (register typed `HttpClient`)
- Test: (unit) `src/EduTrail.Backend/EduTrail.Tests/CodeRunnerClientTests.cs` if a backend test project exists; otherwise cover via Task 5/8 integration and note it.

**Interfaces:**
- Produces:
  - `RunCodeResult(bool Compiled, string CompileOutput, string Stdout, string Stderr, bool TimedOut, int ExitCode)`
  - `CodeTestCase(string Input, string Expected)`, `CodeCaseResult(bool Passed, string Stdout)`, `GradeCodeResult(bool Compiled, string CompileOutput, List<CodeCaseResult> Results)`
  - `interface ICodeRunnerClient { Task<RunCodeResult> RunAsync(string language, string code, string stdin); Task<GradeCodeResult> GradeAsync(string language, string code, IReadOnlyList<CodeTestCase> tests); }`

- [ ] **Step 1:** If a test project exists, write a failing test that `CodeRunnerClient.RunAsync` posts to `/run-code` and maps the JSON (use a stub `HttpMessageHandler`); assert the mapped `Stdout`. Else skip to Step 3 and rely on Task 8.
- [ ] **Step 2:** Run the test → FAIL.
- [ ] **Step 3: Implement** `CodeRunnerClient` (JSON case-insensitive like `PythonQuestionClient`); it adds header `X-Runner-Secret: <CodeRunner:Secret>` to every request. Register in `Program.cs`:
  `builder.Services.AddHttpClient<ICodeRunnerClient, CodeRunnerClient>(c => { c.BaseAddress = new Uri(config["CodeRunner:BaseUrl"] ?? "http://localhost:8002"); c.DefaultRequestHeaders.Add("X-Runner-Secret", config["CodeRunner:Secret"] ?? ""); });`
  Add `"CodeRunner": { "BaseUrl": "http://localhost:8002", "Secret": "<dev secret>" }` to `appsettings.json`.
- [ ] **Step 4:** Run the test (if any) → PASS; else `dotnet build` the API project → success.
- [ ] **Step 5: Commit.**

### Task 5: Backend Run endpoint

**Files:**
- Create: `src/EduTrail.Backend/EduTrail.Application/QuestionCatalog/CodeExec/RunCodeCommand.cs`
- Modify: `src/EduTrail.Backend/EduTrail.API/Controllers/QuestionCatalogController.cs`

**Interfaces:**
- Consumes: `ICodeRunnerClient` (Task 4).
- Produces: `POST /api/QuestionCatalog/run-code` `{language, code, stdin}` → `RunCodeResult`. `RunCodeCommand : IRequest<RunCodeResult>` whose handler calls `ICodeRunnerClient.RunAsync`.

- [ ] **Step 1:** Add `[Authorize] [HttpPost("run-code")]` `RunCode([FromBody] RunCodeCommand command)` returning `Ok(result)`; wrap runner failure to `StatusCode(502, ...)`.
- [ ] **Step 2: Build** the API project → success.
- [ ] **Step 3: Verify** manually in Task 8 (needs the sandbox). 
- [ ] **Step 4: Commit.**

### Task 6: Code grader on Submit

**Files:**
- Modify: `src/EduTrail.Backend/EduTrail.Application/QuestionCatalog/Player/IPlayerAbstractions.cs` (make `IQuestionGrader.Grade` → `Task<Dictionary<string, FieldFeedbackDto>> GradeAsync(...)`)
- Modify: `src/EduTrail.Backend/EduTrail.Infrastructure/Services/QuestionGradingEngine.cs`
- Modify: `src/EduTrail.Backend/EduTrail.Application/QuestionCatalog/Player/SubmitAnswerCommand.cs` (await `GradeAsync`)
- Test: `src/EduTrail.Backend/EduTrail.Tests/QuestionGradingEngineTests.cs` (if test project exists)

**Interfaces:**
- Consumes: `ICodeRunnerClient` (Task 4), `correct_answers.test_cases` from the stored instance.
- Produces: grading dispatches `pl-code-editor` (grader key `code`) to a path that calls `ICodeRunnerClient.GradeAsync(language, code, tests)`; `language` read from the component's `language` attribute (default `cpp`). Score = passed / total; compile failure → `FieldFeedbackDto(false, 0, <first line of compileOutput>)`.

- [ ] **Step 1: Write failing test** (if test project): with a stubbed `ICodeRunnerClient` returning 2/3 passed, `GradeAsync` on a `pl-code-editor` answer yields `score == 2f/3f`; compile-failure stub → `score == 0` and message set. Non-code graders (relabs/exact/sequence/set) keep their existing behavior under the async signature.
- [ ] **Step 2: Run** → FAIL (or build fails on signature change).
- [ ] **Step 3: Implement**: add `"pl-code-editor" -> "code"` to the grader map; in `GradeAsync`, for `code` read `test_cases` from the instance's correct answers for that `answers-name`, call the runner, map results. Keep the pure graders synchronous internally, wrapped so the method is async. Update `SubmitAnswerCommand` to `await`.
- [ ] **Step 4: Run** tests (if any) → PASS; else build → success.
- [ ] **Step 5: Commit.**

### Task 7: Frontend `pl-code-editor` component

**Files:**
- Create: `src/WebSpa/src/app/features/learning-suite/components/questions/player/pl-code-editor.component.ts`
- Modify: `src/WebSpa/src/app/features/learning-suite/components/questions/player/component-registry.ts`
- Modify: `src/WebSpa/src/app/features/learning-suite/components/questions/services/question-catalog.service.ts` (add `runCode`)
- Modify: `src/WebSpa/src/app/features/learning-suite/components/questions/interfaces/IPlayer.ts` (add `RunCodeResult`)
- Modify: `src/WebSpa/package.json` (add CodeMirror deps)

**Interfaces:**
- Consumes: `QuestionCatalogService.runCode(language, code, stdin): Observable<RunCodeResult>` → `POST QuestionCatalog/run-code`.
- Produces: `<pl-code-editor answers-name language starter sample-input>`; `@Input() answersName/language/starter/sampleInput`; `@Output() answerChange` emits the current code string; emits once on init so an unedited submission still carries the starter code.

- [ ] **Step 1: Install** `npm i codemirror @codemirror/state @codemirror/view @codemirror/commands @codemirror/lang-cpp` in `src/WebSpa`.
- [ ] **Step 2: Implement** the component: in `ngAfterViewInit`, when `isPlatformBrowser(PLATFORM_ID)`, create a CodeMirror `EditorView` seeded with `starter`; on doc change emit `answerChange`. Render an editable stdin box seeded from `sampleInput`, a **Run** button calling `runCode(...)` and showing `compileOutput || stderr || stdout` in an output panel, and show `feedback` on grade. Add `runCode` to the service and `RunCodeResult` to `IPlayer.ts`. Register `'pl-code-editor'` in the registry.
- [ ] **Step 3: Verify** `npx ng build --configuration development` → "bundle generation complete", no errors for the new files.
- [ ] **Step 4: Commit.**

### Task 8: Abuse controls — rate limit & concurrency cap

**Files:**
- Modify: `src/EduTrail.Backend/EduTrail.API/Program.cs` (register a rate limiter)
- Modify: `src/EduTrail.Backend/EduTrail.API/Controllers/QuestionCatalogController.cs` (apply the limiter to `run-code` and `submit`)
- Create: `src/EduTrail.Backend/EduTrail.Infrastructure/Services/CodeRunConcurrencyLimiter.cs` (a `SemaphoreSlim`-based global cap, max 8)

**Interfaces:**
- Consumes: the runner client calls from Task 5/6.
- Produces: a named rate-limit policy `"code-exec"` (per-user: 10/min, queue 0) applied to the two code-executing endpoints → HTTP 429 when exceeded; `CodeRunConcurrencyLimiter.RunAsync(Func<Task<T>>)` that bounds concurrent runner calls globally (max 8) and is used by both the Run handler and the code grader; the per-user 2-concurrent cap is enforced via the limiter keyed by user id.

- [ ] **Step 1: Implement** ASP.NET rate limiting (`builder.Services.AddRateLimiter(...)`, `app.UseRateLimiter()`), a partitioned policy keyed by the authenticated user id (`NameIdentifier`): 10 permits/minute. Add `[EnableRateLimiting("code-exec")]` to the `run-code` and `submit` actions. Add `CodeRunConcurrencyLimiter` (global `SemaphoreSlim(8)` + per-user count map, cap 2) and wrap the runner calls in Task 5 and Task 6 with it (over per-user cap → 429-style result).
- [ ] **Step 2: Build** the API project → success.
- [ ] **Step 3: Verify** in Task 9: 11 rapid `run-code` calls as one user → the 11th returns 429; a parallel 3rd concurrent run → 429.
- [ ] **Step 4: Commit.**

### Task 9: Sample question + end-to-end verification

**Files:**
- Create: `questions/sum-two-numbers/info.json`, `questions/sum-two-numbers/question.html`, `questions/sum-two-numbers/server.py`
- Modify: `HOWTORUN.md` (add the `code-runner` service: port 8002, `CODE_RUNNER_SECRET`, `docker pull gcc:13`)

**Interfaces:**
- Consumes: all prior tasks.
- Produces: a working `sum-two-numbers` question. `server.py` `generate` sets `params.prompt` ("Read two integers from input and print their sum."), `params.sample_input` (e.g. `"3 4"`), optional `params.starter_code`, and the test cases **keyed by the answers-name**: `correct_answers["code"] = [{"input": "a b", "expected": str(a+b)}, ...]` (several random cases) — consistent with how the other graders read `correct_answers[answers-name]`. `question.html`: a `pl-question-panel` with `{{params.prompt}}` and `<pl-code-editor answers-name="code" language="cpp" starter="{{params.starter_code}}" sample-input="{{params.sample_input}}">`.

- [ ] **Step 1:** Write the three files.
- [ ] **Step 2: Pull** the image: `docker pull gcc:13`.
- [ ] **Step 3: Start** sandbox + backend; sign in; verify (curl, authenticated): `POST /api/QuestionCatalog/run-code` with a correct sum program + stdin `"3 4"` → `stdout` `"7"`; create an instance of `sum-two-numbers`, submit a correct program → `score == 1`; submit a wrong program → `score < 1`; submit a compile error → `score == 0` with a message.
- [ ] **Step 4:** In the browser, open the question, Run with the sample input, see `7`, Submit, see the score.
- [ ] **Step 5: Commit** the sample question.

---

## Notes for the executor

- A backend unit-test project may not exist; where a task's test step is conditional on it, the fallback is `dotnet build` + the Task 8 end-to-end checks. Do not invent a test framework not already in the repo.
- Docker-dependent Python tests should `skipif` the `docker` CLI is unavailable so the suite still runs elsewhere.
- Keep `IQuestionGrader` changes minimal: one async signature, existing pure graders wrapped, only `code` does I/O.
