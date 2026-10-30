# How to Run EduTrail

EduTrail has **four parts** that run together:

| Part | Tech | URL | Needed for |
|------|------|-----|------------|
| Database | PostgreSQL (Docker) | `localhost:5432` | everything |
| Backend API | ASP.NET Core (.NET 10) | `https://localhost:7238` | everything |
| Python sandbox | FastAPI / Uvicorn | `http://localhost:8001` | playing & creating questions |
| Frontend | Angular 19 | `https://localhost:4200` | the UI |

All folder paths below are relative to the **repo root**, which is the folder that
contains `EduTrail.sln` (e.g. `E:\Projects\eduTrail\EduTrail`).

---

## Prerequisites (install once)

- **.NET 10 SDK** — `dotnet --version`
- **Node.js 18+** — `node -v`
- **Python 3.11+** — `python --version`
- **Docker Desktop** — `docker --version`

First time only, trust the local HTTPS dev certificate:

```powershell
dotnet dev-certs https --trust
```

---

## 1. Database — PostgreSQL (run from the repo root)

```powershell
docker compose up -d database-edu-trail
```

This starts PostgreSQL on `localhost:5432` with database `EduTrailDb`
(user `postgres`, password `EduTrail123!`). The backend's defaults already match,
so no extra configuration is needed.

> **Port 5432 already in use?** If you also have PostgreSQL installed directly on
> Windows, it occupies 5432 and the backend will fail with
> `password authentication failed`. Stop the Windows service (Administrator
> PowerShell): `Stop-Service postgresql-x64-17` — then restart the container:
> `docker compose restart database-edu-trail`.

Check it: `docker ps` should list `edu-trail-postgres` as `Up`.

---

## 2. Python sandbox (run from `python-service/`)

First time only — create a virtual environment and install dependencies:

```powershell
cd python-service
python -m venv .venv
.\.venv\Scripts\python.exe -m pip install -r requirements.txt
```

Every time — start the service:

```powershell
cd python-service
.\.venv\Scripts\python.exe -m uvicorn main:app --port 8001
```

Leave this terminal running. It executes each question's `server.py` and must be up
before you open or create a question.

---

## 3. Backend API (run from the repo root)

```powershell
dotnet run --project src/EduTrail.Backend/EduTrail.API --launch-profile https
```

- Runs on `https://localhost:7238` (Swagger at `https://localhost:7238/swagger`).
- Applies database migrations automatically on startup.
- Leave this terminal running.

---

## 4. Frontend (run from `src/WebSpa/`)

First time only — install packages:

```powershell
cd src/WebSpa
npm install
```

Every time — start the dev server:

```powershell
cd src/WebSpa
npm start
```

Open **https://localhost:4200**.

> If you get an OpenSSL error, run `$env:NODE_OPTIONS="--openssl-legacy-provider"`
> in the same terminal first, then `npm start` again.

---

## Start order (quick reference)

Run each in **its own terminal**, in this order:

```powershell
# Terminal 1 — repo root
docker compose up -d database-edu-trail

# Terminal 2 — python-service/
.\.venv\Scripts\python.exe -m uvicorn main:app --port 8001

# Terminal 3 — repo root
dotnet run --project src/EduTrail.Backend/EduTrail.API --launch-profile https

# Terminal 4 — src/WebSpa/
npm start
```

Then browse to **https://localhost:4200**.

---

## Sign in

- **Email:** `adminpower@yopmail.com`
- **Password:** `Admin@123`

This is the seeded **Instructor** account (can Browse, Add, and Delete questions).
The **Start Exam** menu item only appears for **Student** accounts.

---

## Stopping

- Frontend / backend / Python: press `Ctrl+C` in each terminal.
- Database: `docker compose stop database-edu-trail` (data is kept),
  or `docker compose down` to remove the container.
