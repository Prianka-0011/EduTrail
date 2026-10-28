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
