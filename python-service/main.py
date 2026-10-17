from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from sandbox import execute_server_py

app = FastAPI(title="PrairieLearn Question Generator")


class GenerateRequest(BaseModel):
    server_py: str


@app.post("/generate")
def generate(request: GenerateRequest):
    try:
        result = execute_server_py(request.server_py)
        return result
    except TimeoutError as e:
        raise HTTPException(status_code=422, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=422, detail=str(e))
