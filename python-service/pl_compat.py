import types
import numpy as np

def _to_json(value):
    if isinstance(value, np.ndarray):
        return value.tolist()
    return value

def _from_json(value):
    return value

pl_compat = types.SimpleNamespace(
    to_json=_to_json,
    from_json=_from_json,
)
