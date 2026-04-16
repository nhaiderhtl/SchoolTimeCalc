---
status: resolved
trigger: "[verbatim user input]"
created: 2026-04-16T00:00:00Z
updated: 2026-04-16T11:40:00Z
---

## Current Focus
hypothesis: The snapshot was out of sync with migrations, so `dotnet ef migrations add` generated an empty migration. Using `dotnet ef migrations remove` fixed the snapshot, allowing us to generate the proper `Username` column migration.
test: Apply the new `AddUntisUsername` migration to the database using `dotnet ef database update`.
expecting: The `Username` column is added to `WebUntisData` table and the app runs without crashing.
next_action: Ask the user to verify the app runs without the PostgreSQL exception.

## Symptoms
expected: Application runs and queries WebUntisData without errors.
actual: Application crashes during Home page load and Background sync.
errors: fail: Microsoft.EntityFrameworkCore.Query[10100] ... Npgsql.PostgresException (0x80004005): 42703: column w.Username does not exist
reproduction: Run the app, background sync triggers or navigate to Home page.
started: Started after building/running recent changes (Phase 03-05 added Server and Username credentials persistence).

## Eliminated

## Evidence
- timestamp: 2026-04-16T11:25:00Z
  checked: Database schema vs Model.
  found: `WebUntisData` entity has `Username` and `Server` but no migration existed.
  implication: Missing EF Core Migration caused schema mismatch.
- timestamp: 2026-04-16T11:40:00Z
  checked: ApplicationDbContextModelSnapshot.cs
  found: The snapshot had already added the `Username` column from a previously manual/broken migration, causing EF Core to generate an empty Up() method.
  implication: Using `dotnet ef migrations remove` removed the empty migration, reverted the snapshot, and let us generate the correct `AddUntisUsername` migration. The DB update successfully ran `ALTER TABLE "WebUntisData" ADD "Username" text NOT NULL DEFAULT '';`.

## Resolution
root_cause: The `ApplicationDbContextModelSnapshot.cs` snapshot thought `Username` was already tracked on `WebUntisData` due to a previously broken/manual migration that was never properly applied to the database. This caused `dotnet ef migrations add` to generate an empty migration script (`Up()` was empty), meaning `dotnet ef database update` did nothing.
fix: Ran `dotnet ef migrations remove` to undo the broken snapshot. Then regenerated `AddUntisUsername` which correctly created `ALTER TABLE` scripts for `Username` and applied it to the database with `dotnet ef database update`.
verification: Need user to confirm that app no longer crashes with the PostgreSQL `42703: column w.Username does not exist` exception.
files_changed: 
  - SchoolTimeCalc/Migrations/20260416093817_AddUntisUsername.cs
  - SchoolTimeCalc/Migrations/20260416093817_AddUntisUsername.Designer.cs
  - SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs
