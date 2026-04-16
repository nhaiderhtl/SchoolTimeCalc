# GSD Debug Knowledge Base

Resolved debug sessions. Used by `gsd-debugger` to surface known-pattern hypotheses at the start of new investigations.

---

## postgres-connection-refused — App fails to start with Npgsql connection refused
- **Date:** 2026-04-15T00:00:00Z
- **Error patterns:** NpgsqlException, SocketException, Connection refused, database unavailability
- **Root cause:** No PostgreSQL instance was running locally and there was no docker-compose.yml file provided to start one for development.
- **Fix:** Created a docker-compose.yml file configured with the credentials expected by the application (postgres/postgres on port 5432) and started the database.
- **Files changed:** docker-compose.yml
---
## webuntis-connection-htl-leonding — Login fails with invalid schoolname error code -8500
- **Date:** 2026-04-15T00:00:00Z
- **Error patterns:** login fails, invalid schoolname, error code -8500, WebUntis API
- **Root cause:** Users often enter the display name of their school (e.g. "HTBLA Linz-Leonding") instead of the internal WebUntis login name (e.g. "htl-leonding"), causing the API to reject the request with code -8500.
- **Fix:** Removed the school name input field entirely from WebUntisLogin.razor, and instead derive it directly from the subdomain of the provided server URL.
- **Files changed:** SchoolTimeCalc/Components/Pages/WebUntisLogin.razor
---
## di-resolution-nationalholidayservice — DI unable to resolve INationalHolidayService in WebUntisHolidaySyncService
- **Date:** 2026-04-16T00:00:00Z
- **Error patterns:** System.AggregateException, Unable to resolve service for type, INationalHolidayService, WebUntisHolidaySyncService, Error while validating the service descriptor
- **Root cause:** `NationalHolidayService` and `SchoolHolidayService` were registered as concrete types in `Program.cs` instead of their respective interfaces (`INationalHolidayService` and `ISchoolHolidayService`). `WebUntisHolidaySyncService` expects the interfaces in its constructor.
- **Fix:** Updated `Program.cs` to register the interfaces instead: `builder.Services.AddScoped<INationalHolidayService, NationalHolidayService>();` and `builder.Services.AddScoped<ISchoolHolidayService, SchoolHolidayService>();`.
- **Files changed:** SchoolTimeCalc/Program.cs
---

## missing-encryptedpassword-column — EF Core missing migration for EncryptedPassword column
- **Date:** 2026-04-16T00:00:00.000Z
- **Error patterns:** fail, Program[0], An error occurred while migrating the database, System.InvalidOperationException, Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning, pending changes, Add a new migration before updating the database, Npgsql.PostgresException, 42703, column w.EncryptedPassword does not exist, warning about pending model changes
- **Root cause:** The WebUntisData entity model was modified to include the EncryptedPassword, Server, and LastHolidaySync properties, but an EF Core migration was never generated for these pending model changes.
- **Fix:** Generated the missing EF Core migration (`AddEncryptedPassword`) using `dotnet ef migrations add`. The application automatically applies this migration on startup, resolving the schema mismatch.
- **Files changed:** SchoolTimeCalc/Migrations/20260416085319_AddEncryptedPassword.cs, SchoolTimeCalc/Migrations/20260416085319_AddEncryptedPassword.Designer.cs, SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs
---
## missing-webuntis-username-column — Missing EF Core migration for WebUntisData Username column
- **Date:** 2026-04-16T11:40:00Z
- **Error patterns:** 42703, column w.Username does not exist, Npgsql.PostgresException, EntityFrameworkCore.Query
- **Root cause:** The ApplicationDbContextModelSnapshot.cs snapshot was out of sync due to a broken/manual migration, causing 'dotnet ef migrations add' to generate an empty migration script instead of the ADD COLUMN command.
- **Fix:** Ran 'dotnet ef migrations remove' to undo the broken snapshot, then regenerated 'AddUntisUsername' which correctly created the ALTER TABLE scripts, and applied it via 'dotnet ef database update'.
- **Files changed:** SchoolTimeCalc/Migrations/20260416093817_AddUntisUsername.cs, SchoolTimeCalc/Migrations/20260416093817_AddUntisUsername.Designer.cs, SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs
---
## cs0103-webuntisdata-not-exist — Compile error due to missing webUntisData variable
- **Date:** 2026-04-16T12:00:00Z
- **Error patterns:** CS0103, The name 'webUntisData' does not exist in the current context, build fails
- **Root cause:** A previous edit deleted the code block that fetched `webUntisData` from the database, but left the code that attempts to update `webUntisData.LastHolidaySync` intact.
- **Fix:** Added `var webUntisData = await _dbContext.Set<WebUntisData>().FirstOrDefaultAsync(w => w.SchoolName == schoolName, cancellationToken);` before its usage on line 125.
- **Files changed:** SchoolTimeCalc/Services/WebUntisHolidaySyncService.cs
---
