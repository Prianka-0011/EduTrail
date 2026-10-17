import random
import math
import threading
import numpy as np
from pl_compat import pl_compat

def _make_safe_import():
    import types
    _pl_module = types.SimpleNamespace(
        to_json=pl_compat.to_json,
        from_json=pl_compat.from_json,
    )
    _allowed = {
        'random': random,
        'math': math,
        'numpy': np,
    }

    def safe_import(name, globals=None, locals=None, fromlist=(), level=0):
        if name == 'prairielearn':
            return _pl_module
        if name in _allowed:
            return _allowed[name]
        raise ImportError(f"Import of '{name}' is not permitted in question server.py")

    return safe_import

_SAFE_BUILTINS = {
    '__import__': _make_safe_import(),
    '__build_class__': __build_class__,
    'range': range,
    'len': len,
    'sorted': sorted,
    'list': list,
    'dict': dict,
    'set': set,
    'tuple': tuple,
    'int': int,
    'float': float,
    'str': str,
    'bool': bool,
    'abs': abs,
    'round': round,
    'min': min,
    'max': max,
    'sum': sum,
    'enumerate': enumerate,
    'zip': zip,
    'print': print,
    'isinstance': isinstance,
    'repr': repr,
    'ValueError': ValueError,
    'TypeError': TypeError,
    'SystemExit': SystemExit,
    'KeyboardInterrupt': KeyboardInterrupt,
    'None': None,
    'True': True,
    'False': False,
}

def execute_server_py(server_py: str, timeout: float = 5.0) -> dict:
    data: dict = {'params': {}, 'correct_answers': {}}

    safe_globals = {
        '__builtins__': dict(_SAFE_BUILTINS),
        '__name__': 'server',
        'np': np,
        'pl': _make_safe_import()('prairielearn'),
    }

    result_box: dict = {}

    def run():
        try:
            exec(server_py, safe_globals)  # noqa: S102
            if 'generate' not in safe_globals:
                raise ValueError("server.py must define a generate(data) function")
            safe_globals['generate'](data)
        except BaseException as exc:  # noqa: BLE001
            result_box['exc'] = exc
        finally:
            result_box['done'] = True
            done_event.set()

    done_event = threading.Event()
    # Run as a daemon thread so a runaway server.py (e.g. an infinite loop)
    # cannot block process/interpreter shutdown — we just stop waiting on it.
    worker = threading.Thread(target=run, daemon=True)
    worker.start()

    if not done_event.wait(timeout=timeout):
        raise TimeoutError(f"server.py execution exceeded {timeout}s timeout")

    if 'exc' in result_box:
        exc = result_box['exc']
        if isinstance(exc, (SystemExit, KeyboardInterrupt)):
            raise RuntimeError("server.py attempted to exit the interpreter") from exc
        raise exc

    return data
