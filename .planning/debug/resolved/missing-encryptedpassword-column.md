---
status: resolved
trigger: "Investigate issue: missing-encryptedpassword-column"
created: 2026-04-16T00:00:00.000Z
updated: 2026-04-16T00:00:00.000Z
---

## Current Focus
hypothesis: Pending EF Core migration for WebUntisData's new EncryptedPassword column needs to be created and applied.
test: Check EF Core migrations and model.
expecting: Creating a new migration and applying it will resolve the issue.
next_action: Request human verification for the fix.

## Symptoms
expected: The application starts and queries the database successfully.
actual: A PostgresException occurs stating the column "EncryptedPassword" does not exist, and there is a warning about pending model changes for ApplicationDbContext.
errors: fail: Program[0] An error occurred while migrating the database. System.InvalidOperationException: An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database. Npgsql.PostgresException (0x80004005): 42703: column w.EncryptedPassword does not exist
reproduction: dotnet run --project SchoolTimeCalc/
started: Occurred after fixing the DI resolution issue and previous WebUntisHolidaySyncService updates in phase 03 wave 2.

## Eliminated

## Evidence
- timestamp: 2026-04-16T00:00:00.000Z
  checked: Application execution via dotnet run
  found: The `dotnet run` triggered the EF migrations warning and the exception.
  implication: WebUntisData has been updated in the DbContext with a new EncryptedPassword column, but the migration was not created.
- timestamp: 2026-04-16T00:05:00.000Z
  checked: Run `dotnet ef migrations add AddEncryptedPassword`
  found: The migration was generated successfully.
  implication: The DbContext was successfully analyzed and changes detected.
- timestamp: 2026-04-16T00:06:00.000Z
  checked: Application execution after migration via `dotnet run`
  found: The application applied the new migration and started up successfully without exceptions.
  implication: The issue is fixed by adding the missing migration.

## Resolution
root_cause: The WebUntisData entity model was modified to include the EncryptedPassword, Server, and LastHolidaySync properties, but an EF Core migration was never generated for these pending model changes.
fix: Generated the missing EF Core migration (`AddEncryptedPassword`) using `dotnet ef migrations add`. The application automatically applies this migration on startup, resolving the schema mismatch.
verification: Executed `dotnet run --project SchoolTimeCalc` and observed that the migration was applied, tables were updated, and the service started successfully without any database exceptions.
files_changed: 
- SchoolTimeCalc/Migrations/20260416085319_AddEncryptedPassword.cs (generated)
- SchoolTimeCalc/Migrations/20260416085319_AddEncryptedPassword.Designer.cs (generated)
- SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs (updated)
