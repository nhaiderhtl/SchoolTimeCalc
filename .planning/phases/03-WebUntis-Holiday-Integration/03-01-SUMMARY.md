---
phase: 03-WebUntis-Holiday-Integration
plan: 01
subsystem: data
tags: [efcore, postgres, xunit]

# Dependency graph
requires:
  - phase: 02-webuntis-integration
    provides: [WebUntis sync foundation]
provides:
  - Holiday entity and database schema
  - WebUntis holiday JSON-RPC client definitions
  - Serialization tests for holiday DTOs
affects: [03-WebUntis-Holiday-Integration]

# Tech tracking
tech-stack:
  added: [xunit]
  patterns: [DTO serialization testing]

key-files:
  created: 
    - SchoolTimeCalc.Tests/HolidayDtoTests.cs
    - SchoolTimeCalc/Migrations/20260416081652_AddHolidays.cs
  modified:
    - SchoolTimeCalc/Models/Holiday.cs
    - SchoolTimeCalc/Data/ApplicationDbContext.cs
    - SchoolTimeCalc/Services/IWebUntisClient.cs
    - SchoolTimeCalc/Services/UntisRpcModels.cs

key-decisions:
  - "Updated existing Holiday entity instead of creating a new one to avoid redundancy."
  - "Refactored existing Holiday services (National and School) to match the updated entity."

patterns-established:
  - "Using dedicated test project for validating JSON serialization logic."

requirements-completed: [SYNC-03]

# Metrics
duration: 8 min
completed: 2026-04-16
---

# Phase 03 Plan 01: Holiday Data Models & API Client Expansion Summary

**Added EF Core Holiday schema and expanded WebUntis client to support holiday fetching**

## Performance

- **Duration:** 8 min
- **Started:** 2026-04-16T08:12:00Z
- **Completed:** 2026-04-16T08:20:00Z
- **Tasks:** 3
- **Files modified:** 15

## Accomplishments
- Upgraded `Holiday` entity with `StartDate`, `EndDate`, and `SchoolId`.
- Configured Entity Framework Core with unique indexes and generated migrations.
- Expanded `IWebUntisClient` to support `getHolidays`.
- Created xUnit test project `SchoolTimeCalc.Tests` and verified DTO JSON serialization.

## Task Commits

Each task was committed atomically:

1. **Task 1: Data Models** - `7601a10` (feat(03-01): create Holiday entity and EF migration)
2. **Task 2: API Client Expansion** - `218ba73` (feat(03-01): expand WebUntis API client for holidays)
3. **Task 3: Unit Tests** - `218ba73` (feat(03-01): expand WebUntis API client for holidays)

## Files Created/Modified
- `SchoolTimeCalc/Models/Holiday.cs` - Updated schema
- `SchoolTimeCalc/Data/ApplicationDbContext.cs` - Unique constraint
- `SchoolTimeCalc/Services/IWebUntisClient.cs` - Added GetHolidaysAsync
- `SchoolTimeCalc/Services/UntisRpcModels.cs` - Added UntisHolidayDto
- `SchoolTimeCalc.Tests/HolidayDtoTests.cs` - New serialization tests

## Decisions Made
Updated existing `Holiday` entity instead of creating a separate model.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 1 - Bug] Build errors due to modified Holiday entity**
- **Found during:** Task 1 (Data Models)
- **Issue:** Changing `Date`, `Type` and `Bundesland` to `StartDate`, `EndDate` and `SchoolId` broke `NationalHolidayService` and `SchoolHolidayService`.
- **Fix:** Updated field assignments in both services to match new `Holiday` properties.
- **Files modified:** SchoolTimeCalc/Services/NationalHolidayService.cs, SchoolTimeCalc/Services/SchoolHolidayService.cs
- **Verification:** Project builds successfully.
- **Committed in:** `7601a10` (part of task commit)

**2. [Rule 3 - Blocking] dotnet executable not found in PATH**
- **Found during:** Task 1 (EF Migrations)
- **Issue:** Standard `dotnet` command could not be resolved.
- **Fix:** Used `/home/nico/.dotnet/dotnet` and updated `DOTNET_ROOT` / `PATH` environment variables.
- **Files modified:** None
- **Verification:** Successfully generated EF migrations and ran tests.
- **Committed in:** `7601a10`

---

**Total deviations:** 2 auto-fixed (1 bug, 1 blocking)
**Impact on plan:** None. 

## Issues Encountered
None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
Holiday data models and basic DTOs are in place. Ready to implement the background syncing service.

---
*Phase: 03-WebUntis-Holiday-Integration*
*Completed: 2026-04-16*
