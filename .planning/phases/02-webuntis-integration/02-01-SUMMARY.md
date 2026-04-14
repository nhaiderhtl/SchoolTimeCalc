---
phase: 02-webuntis-integration
plan: 01
subsystem: api
tags: [refit, polly, efcore, postgres, webuntis]

# Dependency graph
requires:
  - phase: 01-foundation
    provides: [database context, user model]
provides:
  - ef core entity for storing JSONB Untis data
  - refit client interface
  - polly retry policies configured
affects: [02-webuntis-integration]

# Tech tracking
tech-stack:
  added: [Refit.HttpClientFactory, Microsoft.Extensions.Http.Polly]
  patterns: [Refit API definitions, Polly retry policies, JSONB EF Core columns]

key-files:
  created: 
    - SchoolTimeCalc/Models/WebUntisData.cs
    - SchoolTimeCalc/Services/UntisRpcModels.cs
    - SchoolTimeCalc/Services/IWebUntisClient.cs
  modified: 
    - SchoolTimeCalc/SchoolTimeCalc.csproj
    - SchoolTimeCalc/Models/ApplicationUser.cs
    - SchoolTimeCalc/Data/ApplicationDbContext.cs
    - SchoolTimeCalc/Program.cs

key-decisions:
  - "Configured WebUntis JSON responses to be stored in PostgreSQL JSONB columns for flexibility."
  - "Used Refit for declarative WebUntis JSON-RPC client definitions."
  - "Added Polly WaitAndRetryAsync policy to handle transient HTTP errors."

patterns-established:
  - "JSONB columns in EF Core for unstructured JSON data"
  - "Refit interface for external API calls"

requirements-completed: [SYNC-01, SYNC-02]

# Metrics
duration: 2 min
completed: 2026-04-14T12:00:00Z
---

# Phase 02 Plan 01: WebUntis Integration Models and API Client Summary

**Configured EF Core WebUntisData model with JSONB columns and established Refit JSON-RPC client with Polly retries**

## Performance

- **Duration:** 2 min
- **Started:** 2026-04-14T12:00:00Z
- **Completed:** 2026-04-14T12:02:00Z
- **Tasks:** 3
- **Files modified:** 7

## Accomplishments
- Added WebUntisData model with one-to-one relationship to ApplicationUser.
- Configured ApplicationDbContext to use JSONB columns for WebUntisData.
- Defined Refit client and DTOs for the WebUntis JSON-RPC API.
- Configured Dependency Injection with Polly retry policies for the WebUntis client.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add Packages and Database Models** - `b2c22c0` (feat)
2. **Task 2: Define Refit Client and DTOs** - `f3ffcb0` (feat)
3. **Task 3: Configure DI and Polly Policies** - `929b3d0` (feat)

## Files Created/Modified
- `SchoolTimeCalc/Models/WebUntisData.cs` - Database model for WebUntis JSON data.
- `SchoolTimeCalc/Services/UntisRpcModels.cs` - Request and response DTOs for WebUntis JSON-RPC API.
- `SchoolTimeCalc/Services/IWebUntisClient.cs` - Refit client interface for WebUntis API.
- `SchoolTimeCalc/SchoolTimeCalc.csproj` - Added Refit and Polly NuGet packages.
- `SchoolTimeCalc/Models/ApplicationUser.cs` - Added navigation property to WebUntisData.
- `SchoolTimeCalc/Data/ApplicationDbContext.cs` - Configured WebUntisData DbSet and JSONB columns.
- `SchoolTimeCalc/Program.cs` - Registered Refit client and Polly retry policy.

## Decisions Made
- Configured WebUntis JSON responses to be stored in PostgreSQL JSONB columns for flexibility.
- Used Refit for declarative WebUntis JSON-RPC client definitions.
- Added Polly WaitAndRetryAsync policy to handle transient HTTP errors.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered
None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
WebUntis API client foundation is complete, ready for actual syncing logic.

---
*Phase: 02-webuntis-integration*
*Completed: 2026-04-14*
