"""code-runner service — compiles and runs untrusted C/C++ in isolated Docker
containers. Intended to run on 127.0.0.1:8002 and be called only by the EduTrail
backend, authenticated with a shared secret.

Run (dev):
    CODE_RUNNER_SECRET=<secret> python -m uvicorn main:app --port 8002
uvicorn's default host is 127.0.0.1 — do NOT pass --host 0.0.0.0.
"""

import hmac
import os

from fastapi import Depends, FastAPI, Header, HTTPException
from pydantic import BaseModel

from runner import run_code, grade_code

app = FastAPI(title="EduTrail code-runner")


def verify_secret(x_runner_secret: str = Header(default="")) -> None:
    expected = os.environ.get("CODE_RUNNER_SECRET", "")
    # A missing/empty configured secret rejects everything, rather than
    # silently allowing unauthenticated access. compare_digest is constant-time.
    if not expected or not hmac.compare_digest(x_runner_secret, expected):
        raise HTTPException(status_code=401, detail="invalid or missing runner secret")


class RunRequest(BaseModel):
    language: str
    code: str
    stdin: str = ""


class TestCase(BaseModel):
    input: str = ""
    expected: str = ""


class GradeRequest(BaseModel):
    language: str
    code: str
    tests: list[TestCase] = []


@app.post("/run-code")
def run_endpoint(req: RunRequest, _: None = Depends(verify_secret)):
    try:
        return run_code(req.language, req.code, req.stdin)
    except ValueError as e:
        raise HTTPException(status_code=422, detail=str(e))


@app.post("/grade-code")
def grade_endpoint(req: GradeRequest, _: None = Depends(verify_secret)):
    try:
        return grade_code(req.language, req.code, [t.model_dump() for t in req.tests])
    except ValueError as e:
        raise HTTPException(status_code=422, detail=str(e))
