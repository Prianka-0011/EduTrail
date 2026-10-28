import shutil
import pytest
from runner import run_code

docker_available = shutil.which("docker") is not None
needs_docker = pytest.mark.skipif(not docker_available, reason="docker CLI not available")

SUM = "#include <iostream>\nint main(){int a,b;std::cin>>a>>b;std::cout<<a+b;return 0;}\n"


@needs_docker
def test_runs_and_returns_stdout():
    r = run_code("cpp", SUM, "3 4")
    assert r["compiled"] is True
    assert r["stdout"].strip() == "7"
    assert r["exitCode"] == 0


@needs_docker
def test_compile_error_reported():
    r = run_code("cpp", "int main(){", "")
    assert r["compiled"] is False
    assert r["compileOutput"].strip() != ""


@needs_docker
def test_timeout_flagged():
    r = run_code("cpp", "int main(){while(1){}}", "")
    assert r["timedOut"] is True


def test_unknown_language_raises():
    with pytest.raises(ValueError):
        run_code("python", "x=1", "")


from runner import grade_code

ALWAYS_ZERO = "#include <iostream>\nint main(){std::cout<<0;return 0;}\n"


@needs_docker
def test_grade_all_pass():
    r = grade_code("cpp", SUM, [{"input": "1 2", "expected": "3"}, {"input": "5 5", "expected": "10"}])
    assert r["compiled"] is True
    assert all(c["passed"] for c in r["results"])


@needs_docker
def test_grade_mixed():
    r = grade_code("cpp", ALWAYS_ZERO, [{"input": "1 2", "expected": "3"}, {"input": "0 0", "expected": "0"}])
    assert r["results"][0]["passed"] is False
    assert r["results"][1]["passed"] is True


@needs_docker
def test_grade_compile_error():
    r = grade_code("cpp", "int main(){", [{"input": "1 2", "expected": "3"}])
    assert r["compiled"] is False
    assert r["results"] == []
