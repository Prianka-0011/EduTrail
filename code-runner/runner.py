"""Compile and run untrusted C/C++ in a locked-down, throwaway Docker container.

Every execution runs in a fresh `gcc:13` container with no network, a read-only
root filesystem, a small writable tmpfs at /work, strict memory/cpu/pid limits,
a non-root user, dropped capabilities, and a hard wall-clock timeout. The source
is passed base64-encoded in an env var (so the container's stdin stays free for
the program's own input).
"""

import base64
import shutil
import subprocess
import uuid

# Limits (see the plan's Global Constraints). The compile and run time limits are
# enforced INSIDE the container with `timeout` so the container self-terminates;
# the Python-level timeout is only a backstop if the daemon itself wedges.
COMPILE_TIMEOUT = 5      # seconds for g++
RUN_TIMEOUT = 3          # seconds for the compiled program
PY_BACKSTOP = 25         # seconds, outer safety net around `docker run`
OUTPUT_CAP = 64 * 1024   # bytes of stdout/stderr kept
COMPILE_FAIL_EXIT = 99   # sentinel exit code the shell uses when g++ fails
TIMEOUT_EXITS = {124, 137}  # `timeout`/SIGKILL exit codes → run exceeded its limit

_DOCKER_FLAGS = [
    "--rm", "-i",
    "--network", "none",
    "--memory=256m", "--memory-swap=256m",
    "--cpus=1",
    "--pids-limit=64",
    "--read-only",
    "--tmpfs", "/work:size=32m,exec,mode=1777",
    "--workdir", "/work",
    "--user", "1000:1000",
    "--cap-drop", "ALL",
    "--security-opt", "no-new-privileges",
]

# Decode the source, compile it; on failure emit the compiler output and exit
# with the sentinel; otherwise exec the program (its stdin is the container's).
_SHELL = (
    'export TMPDIR=/work; '
    'echo "$SRC_B64" | base64 -d > /work/main.cpp; '
    'timeout -s KILL %d g++ /work/main.cpp -o /work/a.out 2>/work/cerr || '
    '{ cat /work/cerr >&2; exit %d; }; '
    'exec timeout -s KILL %d /work/a.out'
) % (COMPILE_TIMEOUT, COMPILE_FAIL_EXIT, RUN_TIMEOUT)


def run_code(language: str, code: str, stdin: str) -> dict:
    if language != "cpp":
        raise ValueError(f"unsupported language: {language!r}")

    src_b64 = base64.b64encode(code.encode("utf-8")).decode("ascii")
    name = "runcpp-" + uuid.uuid4().hex[:12]

    args = [
        "docker", "run", *_DOCKER_FLAGS,
        "--name", name,
        "-e", "SRC_B64=" + src_b64,
        "gcc:13", "sh", "-c", _SHELL,
    ]

    try:
        proc = subprocess.run(
            args,
            input=stdin.encode("utf-8"),
            capture_output=True,
            timeout=PY_BACKSTOP,
        )
    except subprocess.TimeoutExpired:
        subprocess.run(["docker", "kill", name], capture_output=True)
        return {
            "compiled": True,
            "compileOutput": "",
            "stdout": "",
            "stderr": "",
            "timedOut": True,
            "exitCode": -1,
        }

    stdout = proc.stdout.decode("utf-8", "replace")[:OUTPUT_CAP]
    stderr = proc.stderr.decode("utf-8", "replace")[:OUTPUT_CAP]
    compiled = proc.returncode != COMPILE_FAIL_EXIT
    timed_out = compiled and proc.returncode in TIMEOUT_EXITS

    return {
        "compiled": compiled,
        "compileOutput": "" if compiled else stderr,
        "stdout": stdout if compiled else "",
        "stderr": stderr if compiled else "",
        "timedOut": timed_out,
        "exitCode": proc.returncode,
    }
