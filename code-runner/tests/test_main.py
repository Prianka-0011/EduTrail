import os
os.environ["CODE_RUNNER_SECRET"] = "testsecret"

import shutil
import pytest
from fastapi.testclient import TestClient
from main import app

client = TestClient(app)
docker_available = shutil.which("docker") is not None
needs_docker = pytest.mark.skipif(not docker_available, reason="docker CLI not available")

SUM = "#include <iostream>\nint main(){int a,b;std::cin>>a>>b;std::cout<<a+b;return 0;}\n"
H = {"X-Runner-Secret": "testsecret"}


def test_requires_secret():
    r = client.post("/run-code", json={"language": "cpp", "code": SUM, "stdin": "2 3"})
    assert r.status_code == 401


@needs_docker
def test_run_code_ok():
    r = client.post("/run-code", json={"language": "cpp", "code": SUM, "stdin": "2 3"}, headers=H)
    assert r.status_code == 200
    assert r.json()["stdout"].strip() == "5"


@needs_docker
def test_grade_code_ok():
    r = client.post(
        "/grade-code",
        json={"language": "cpp", "code": SUM, "tests": [{"input": "2 3", "expected": "5"}]},
        headers=H,
    )
    assert r.status_code == 200
    assert r.json()["results"][0]["passed"] is True
