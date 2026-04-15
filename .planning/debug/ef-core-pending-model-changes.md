---
status: awaiting_human_verify
trigger: "Investigate issue: ef-core-pending-model-changes"
created: 2026-04-15T00:00:00Z
updated: 2026-04-15T12:05:00Z
---

## Current Focus
hypothesis: "Pending model changes exist in ApplicationDbContext but no migration was created to apply them to the DB"
test: "Run dotnet ef migrations add InitialCreate"
expecting: "Migration created successfully and subsequent dotnet run starts app without error."
next_action: "Wait for human verification"

## Symptoms
expected: The app boots up, applies migrations successfully, and starts the web server.
actual: Startup crashes with `System.InvalidOperationException: The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database.` followed by `Npgsql.PostgresException: 42P01: relation "Users" does not exist`.
errors: Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning ... Npgsql.PostgresException: 42P01: relation "Users" does not exist.
reproduction: 1. Start postgres container (`docker compose up -d`). 2. Run `dotnet run` in SchoolTimeCalc directory.
started: Started happening immediately after fixing the previous DB connection refused error. The app now connects to the DB but fails during the startup migration phase.

## Eliminated

## Evidence
- timestamp: 2026-04-15T12:02:00Z
  checked: SchoolTimeCalc/Migrations folder
  found: The folder did not exist. There were no migrations.
  implication: EF Core sees the entity models in `ApplicationDbContext` but since there is no matching migration, it treats this as a pending model change. `dbContext.Database.Migrate()` automatically attempts to apply existing migrations, and since there were none for the current models, it threw a warning which EF Core 9+ treats as an exception.

- timestamp: 2026-04-15T12:04:00Z
  checked: `dotnet run` output after creating migration
  found: Applied migration '20260415101451_InitialCreate', tables (like Users) were created, and app started successfully.
  implication: Creating the initial migration resolved the issue.

## Resolution
root_cause: The models in `ApplicationDbContext` did not have corresponding Entity Framework migrations generated. Because EF Core 9+ throws on `PendingModelChangesWarning` when `dbContext.Database.Migrate()` is called, the app crashed on startup.
fix: Installed the `dotnet-ef` global tool and generated an initial migration by running `dotnet ef migrations add InitialCreate`. This allowed `Migrate()` to successfully create the missing database tables.
verification: Ran `dotnet run` which correctly applied the migration and started listening on port 5241 without crashing.
files_changed: [SchoolTimeCalc/Migrations/20260415101451_InitialCreate.cs, SchoolTimeCalc/Migrations/20260415101451_InitialCreate.Designer.cs, SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs]