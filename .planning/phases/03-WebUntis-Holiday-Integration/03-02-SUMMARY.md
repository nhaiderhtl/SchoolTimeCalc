---
phase: 03-WebUntis-Holiday-Integration
plan: 02
subsystem: api
tags: [webuntis, holidays, sync, refit, postgres, efcore, background-service]

requires:
  - phase: 02-webuntis-integration
    provides: [WebUntis authentication flow, base API clients]
  - phase: 03-WebUntis-Holiday-Integration
    provides: [Holiday DTOs and Data Models]
provides:
  - Holiday sync service interface and implementation
  - Background service for periodic holiday synchronization
  - Upsert logic for holiday data to PostgreSQL database
affects: [03-03-PLAN.md, 04-calculation-engine]

tech-stack:
  added: [Microsoft.Extensions.Hosting]
  patterns: [BackgroundService, Upsert]

key-files:
  created: [SchoolTimeCalc/Services/IHolidaySyncService.cs, SchoolTimeCalc/Services/WebUntisHolidaySyncService.cs, SchoolTimeCalc/Services/HolidaySyncBackgroundService.cs, SchoolTimeCalc.Tests/HolidaySyncServiceTests.cs]
  modified: [SchoolTimeCalc/Program.cs]

key-decisions:
  - "Decided to implement IHostedService as a BackgroundService instead of Hangfire for the MVP to reduce infrastructure complexity."
  - "Used IHttpClientFactory in combination with Refit RestService.For to dynamically construct the WebUntis client for arbitrary school server addresses."

patterns-established:
  - "Daily background synchronization loop via BackgroundService."
  - "Upsert strategy for list data by fetching current state, mapping to dictionary, and checking keys."

requirements-completed: [SYNC-03]

duration: 10 min
completed: 2026-04-16T08:26:00Z
---

# Phase 03 Plan 02: Holiday Sync Service Summary

**WebUntis holiday data fetching, mapping, and database upsert operations via a background service.**

## Performance

- **Duration:** 10 min
- **Started:** 2026-04-16T08:20:00Z
- **Completed:** 2026-04-16T08:26:00Z
- **Tasks:** 4
- **Files modified:** 5

## Accomplishments
- Defined the IHolidaySyncService interface for school holiday synchronization.
- Implemented WebUntisHolidaySyncService that authenticates, retrieves holiday data via Refit, and upserts it to the database.
- Created HolidaySyncBackgroundService to run a daily periodic sync across all active users.
- Integrated all required dependencies into the DI container in Program.cs.

## Task Commits

Each task was committed atomically:

1. **Task 1-4: Implement holiday sync service** - `514a9a8` (feat)

## Files Created/Modified
- `SchoolTimeCalc/Services/IHolidaySyncService.cs` - Interface definition
- `SchoolTimeCalc/Services/WebUntisHolidaySyncService.cs` - Business logic for holiday fetching and saving
- `SchoolTimeCalc/Services/HolidaySyncBackgroundService.cs` - Periodic task trigger
- `SchoolTimeCalc/Program.cs` - Registered DI services
- `SchoolTimeCalc.Tests/HolidaySyncServiceTests.cs` - Stub for integration tests

## Decisions Made
- Used IHostedService instead of Hangfire. It is much easier to test out the initial background scheduling mechanism using Microsoft's built-in BackgroundService. Can be upgraded to Hangfire/Quartz later for robustness if needed.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered
None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- Ready for plan 03-03 (Integration & Settings UI Updates).
