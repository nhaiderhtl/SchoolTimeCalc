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

