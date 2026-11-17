# ScamCheck

ScamCheck is a South African localized cybersecurity self-check site. It uses an ASP.NET Core Minimal API, SQLite, and a Blazor WebAssembly PWA. There are no user accounts or authentication in this first version.

## Run

From the solution directory, run:

```powershell
dotnet run --project ScamCheck.Api
```

The API serves the Blazor app from the same local host. Check `GET /health` for a health response. The SQLite database is `scamcheck.db` in the API process working directory and is created and seeded on first start. WAL mode and NORMAL synchronous mode are enabled at startup.

## Backup

Stop writes to the application, then create a consistent SQLite backup with:

```sql
VACUUM INTO 'scamcheck-backup.db';
```

Do not place the SQLite database or its WAL files on a network share. Keep the database on local storage and copy backups to an appropriate protected backup destination.

## Tests

Run the `ScamCheck.Tests` project from Visual Studio Test Explorer or with the standard .NET test runner.
