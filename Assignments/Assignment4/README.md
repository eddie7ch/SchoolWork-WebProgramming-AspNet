# Assignment4 — Run & Database Setup

Purpose: make it easy for your instructor to run the project locally with a portable, free setup (LocalDB). This README shows the minimal steps to run the app and create the database using EF Core migrations.

Prerequisites
- .NET SDK (6/7/8) installed — run `dotnet --version`.
- SQL Server LocalDB (shipped with Visual Studio) or an alternative (see notes).

Quick run (LocalDB)
1. Clone the repository and open a terminal in the project folder:

```powershell
cd "<path-to-repo>/Assignments/Assignment4"
dotnet restore
```

2. Copy the example config to `appsettings.json` (or `appsettings.Development.json`) if you prefer:

```powershell
copy appsettings.json.example appsettings.json
```

3. Create the database from EF Core migrations (the project uses EF Core):

```powershell
dotnet tool restore
dotnet ef database update
```

4. Run the app:

```powershell
dotnet run
```

Then open the URL shown in the console (usually `https://localhost:5001` or similar).

Notes & troubleshooting
- The default connection string uses LocalDB: `Server=(localdb)\\mssqllocaldb;Database=Assignment4Db;Trusted_Connection=True;` — LocalDB starts on demand; you do not need the full `MSSQLSERVER` service.
- If `dotnet ef` is not available, install it with `dotnet tool install --global dotnet-ef` or run `dotnet tool restore` if a tool manifest is present.
- If you prefer zero-db hosting for the demo, the instructor can run the app after you seed an in-memory DB or switch to SQLite; ask me and I can help convert or add instructions.

What to include in your submission
- `appsettings.json.example` (this file)
- EF Core migrations (if present in the repo)
- A short README with the exact commands you used (this file)
- Note: do NOT commit any secrets or production connection strings.

Cleanup (avoid cloud charges)
- If you publish to a cloud VM or managed DB for a demo, stop/terminate the instance and RDS after the demo to avoid charges. Use AWS Budgets/alerts.

If you want, I can:
- Generate or run EF migrations for you locally and add a SQL export.
- Prepare a GitHub-ready submission (add example config, README, and a short commit).

---
Created to keep the demo free and reproducible on a local machine.
