# Code Question Type (C/C++) — Design

Date: 2026-10-28
Status: Proposed (awaiting approval)

## Intent & why

Add a new question type where a student **writes C/C++ code in an in-browser
editor, runs it to see the output, and submits it to be graded** against test
cases. First concrete example: "Read two numbers and print their sum."

This differs from every existing question type: instead of selecting/entering an
answer that is compared, the student's *code is executed on the server*. Because
the app will be used by **many students through separate logins**, that execution
must be genuinely isolated — untrusted code is the normal case here, not an edge
case.

## Goals

- A new `pl-code-editor` renderer component: a real code editor (highlighting,
  line numbers), a language set per question (C/C++ first), an editable input
  box, a **Run** button, and an **output** panel.
- **Run**: compile + execute the student's code once with an editable sample
  input; show compile errors / stdout / stderr. No grading.
- **Submit**: compile once, run against all test cases (stdin → expected
  stdout); score = fraction of cases passed; a compile error scores 0.
- Every compile and run happens inside a **short-lived, locked-down Docker
  container**: no network, CPU/memory/process/output limits, non-root,
  read-only filesystem, hard timeout, destroyed after the run.
- The author defines the task text, optional starter code, sample input, and
  test cases in the question's `server.py`, so inputs/expected outputs can be
  randomized per attempt (consistent with the other question types).

## Non-goals (v1)

- Languages other than C/C++ (the editor and runner are built to add more later,
  but only C/C++ ships now).
- Interactive stdin while running, multi-file projects, third-party libraries
  beyond the standard library.
- Persisting submissions/history (instances stay in-memory, like the rest).
- A rich autograder DSL — test cases are plain input/expected-output pairs with a
  trimmed string comparison.

## Anti-goals (would be a failure even if it "works")

- Student code able to read app secrets (DB password, JWT key, SMTP creds),
  reach the database, touch other students' data, access the network, or crash
  the host. **Isolation is the whole point.** A version that compiles/runs
  student C/C++ directly on the host is a failure, not an MVP.

## Constraints

- Reuse the existing architecture: question folders
  (`info.json` / `question.html` / `server.py`), request-time catalog scan,
  in-memory instances, MediatR + controllers, the dynamic renderer + component
  registry, and the existing create-instance / submit / run flow.
- Docker is available on the host (v25) and is the isolation mechanism.
- WebSpa uses Angular SSR, so the editor must initialize **client-only** (lazy,
  guarded against `window`/`document` on the server). This is why CodeMirror is
  chosen over Monaco.

## Decisions already made

| Decision | Choice | Source |
|---|---|---|
| Editor | **CodeMirror 6**, language extension per question | my call (SSR-friendly, light, clean multi-language growth) |
| Run input | **Editable sample** — defaults to a sample case's input, student can edit before running | user |
| Grading | Compile once; run each test case; compare **trimmed** stdout to expected; score = passed/total; per-case pass/fail feedback | my call (partial credit, trimmed compare) |
| Test cases | Defined in the question's `server.py` (`correct_answers`) | my call / user default |
| Isolation | **Self-hosted, Docker-per-run** | user |
| Runner location | The existing sandbox service (`python-service`) gains C/C++ endpoints that orchestrate `docker run`; the .NET backend forwards to it | my call (keeps all code execution behind one service boundary) |
| Grader | A new `code` grader type that *executes* via the runner (unlike the pure-comparison graders) | my call |

## Architecture

### 1. Runner Docker image
Use a compiler image with `g++` (e.g. `gcc:13`), or a slim custom image built
from it. The container compiles `main.cpp` and runs the resulting binary with a
supplied stdin.

### 2. Sandbox service endpoints (`python-service`, FastAPI)
- `POST /run-code` `{ language, code, stdin }` →
  `{ compiled, compileOutput, stdout, stderr, timedOut, exitCode }`.
  Writes the code to a temp dir and launches one container (flags below) that
  compiles then runs, piping `stdin`, with a wall-clock timeout and capped
  output.
- `POST /grade-code` `{ language, code, tests: [{input, expected}] }` →
  `{ compiled, compileOutput, results: [{passed, stdout}] }`.
  **Compiles once**, then runs each test case in the sandbox. (Preferred over
  calling `/run-code` per case so compilation isn't repeated.)

Container flags (the heart of the isolation):
`docker run --rm --network none --memory=256m --memory-swap=256m --cpus=1
--pids-limit=64 --read-only --tmpfs /work:size=32m,exec --user <non-root>
--cap-drop ALL --security-opt no-new-privileges`, host-enforced timeout
(e.g. 5 s compile + 3 s run), stdout/stderr truncated (e.g. 64 KB).

### 3. Backend (.NET)
- `POST /api/QuestionCatalog/run-code` `{ language, code, stdin }` → forwards to
  the sandbox, returns the output. `[Authorize]`.
- Submit path: a new `code` grader reads `correct_answers.test_cases` and the
  submitted code, calls the sandbox `/grade-code`, and computes the score. The
  grading engine dispatches to it for the `pl-code-editor` component. Because
  grading now does I/O (an HTTP call), `IQuestionGrader.Grade` becomes async (or
  a parallel async grading path is added for code questions).

### 4. Frontend
- `pl-code-editor` component: CodeMirror editor bound to the code, `language`
  and `starter` inputs, an editable stdin box (seeded from a sample input),
  a **Run** button that calls `run-code` and renders compile errors / stdout /
  stderr, and it emits the code via `answerChange` so Submit grades it.
  Registered in the component registry; CodeMirror is initialized lazily in the
  browser only.
- `QuestionCatalogService.runCode(...)` service method.

### 5. Sample question (`sum-two-numbers`)
`server.py` sets `params.prompt`, optional `params.starter_code`,
`params.sample_input`, and `correct_answers.test_cases` (several random
`a b` → `a+b` cases). `question.html`:
```
<pl-question-panel><p>{{params.prompt}}</p></pl-question-panel>
<pl-code-editor answers-name="code" language="cpp"></pl-code-editor>
```

### Data shapes
- `generate()` → `params: { prompt, starter_code?, sample_input }`,
  `correct_answers: { test_cases: [{ input, expected, visible? }] }`.
- Grader: compile once; for each case, run with `input`; pass if
  `trim(stdout) == trim(expected)`; `score = passed / total`.

## Residual security notes (documented, not solved by v1)

- The sandbox service can launch containers, which on most setups is
  **root-equivalent on the host**. Keep that service internal, authenticated
  (shared secret from the backend), and least-privileged. This is a real caveat
  to revisit before production, separate from per-run isolation.
- Windows/Docker Desktop resource-limit enforcement differs from Linux; verify
  limits actually bite on the deployment host.

## Left to the builder

- Exact image tag / whether to prebuild a slim image vs use `gcc:13`.
- Exact limits (memory, cpus, pids, timeouts, output cap) — start from the values
  above and tune.
- CodeMirror Angular wiring (lazy client-only init; SSR guard).
- `IQuestionGrader` async refactor vs a separate async code-grading path.
- Whether test cases can be marked hidden (expected output not shown); default is
  to show pass/fail per case without revealing expected for hidden ones.
- Error/empty-state copy.
