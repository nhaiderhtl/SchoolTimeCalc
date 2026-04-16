---
phase: 04-calculation-engine
plan: 01
subsystem: api
tags: [calculation, WebUntis, C#]

# Dependency graph
requires:
  - phase: 03-webuntis-holiday-integration
    provides: Holiday data and WebUntis timetable payload
provides:
  - CalculationService that accurately computes total and subject-specific remaining lessons
  - Filter logic that excludes holidays and weekends
affects: [05-presentation]

# Tech tracking
tech-stack:
  added: []
  patterns: [Dependency Injection, calculation-as-a-service]

key-files:
  created: [SchoolTimeCalc/Services/CalculationService.cs, SchoolTimeCalc/Models/CalculationResult.cs]
  modified: [SchoolTimeCalc/Program.cs]

key-decisions:
  - "Extract JSON serialization logic into local protected classes inside CalculationService to simplify domain models"
  - "Process JSON objects from Untis directly by computing valid days against cached holidays"

patterns-established:
  - "JSON parsing localized in CalculationService"

requirements-completed: [CALC-01, CALC-02, CALC-03, CALC-04]

# Metrics
duration: 10 min
completed: 2026-04-16T11:45:00Z
---

# Phase 04 Plan 01: Core Calculation Logic & Service Summary

**Implemented CalculationService to accurately compute exact remaining time and specific subject lessons from WebUntis timetable data**

## Performance

- **Duration:** 10 min
- **Started:** 2026-04-16T11:35:00Z
- **Completed:** 2026-04-16T11:45:00Z
- **Tasks:** 2
- **Files modified:** 5

## Accomplishments
- Created CalculationResult and SubjectRemainingLessons models.
- Created and registered ICalculationService that queries WebUntisData JSON blobs.
- Implemented calculation logic excluding weekends, holidays, and cancelled lessons to derive a dynamic end date and subject-level insights.

## Task Commits

Each task was committed atomically:

1. **Task 1: Create Calculation Models** - `599a647` (feat)
2. **Task 2: Implement Calculation Service** - `c5ed3cd` (feat)

## Files Created/Modified
- `SchoolTimeCalc/Models/CalculationResult.cs` - Model holding output metrics.
- `SchoolTimeCalc/Models/SubjectRemainingLessons.cs` - Model holding specific subject counts.
- `SchoolTimeCalc/Services/ICalculationService.cs` - Interface.
- `SchoolTimeCalc/Services/CalculationService.cs` - Implementation counting down lessons.
- `SchoolTimeCalc/Program.cs` - Registered service in DI.

## Decisions Made
- Extracted JSON serialization logic into local protected classes inside CalculationService to simplify domain models

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered
None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
Core logic is established. Need unit tests (Plan 04-02).

---
*Phase: 04-calculation-engine*
*Completed: 2026-04-16*
