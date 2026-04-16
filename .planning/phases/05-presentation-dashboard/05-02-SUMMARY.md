---
phase: 05-presentation-dashboard
plan: 02
subsystem: ui
tags: [dashboard, component, breakdown, lessons]

# Dependency graph
requires:
  - phase: 04-calculation-engine
    provides: Calculation service that computes subject-level lessons
provides:
  - SubjectBreakdownWidget component with expandable UI
  - Real-time lessons vs total percentage progress bar
  - Dashboard integration showing subject breakdown
affects: [05-presentation-dashboard]

# Tech tracking
tech-stack:
  added: []
  patterns: [expandable accordion cards, responsive progress bars]

key-files:
  created: [SchoolTimeCalc/Components/Widgets/SubjectBreakdownWidget.razor]
  modified:
    - SchoolTimeCalc/Components/Pages/Dashboard.razor
    - SchoolTimeCalc/Services/CalculationService.cs
    - SchoolTimeCalc/Models/SubjectRemainingLessons.cs

key-decisions:
  - "Added TotalLessons and CanceledLessons to the CalculationService model to accurately support the breakdown requirements instead of mocking progress bars."

patterns-established: []

requirements-completed: [UI-02, UI-04]

# Metrics
duration: 4m
completed: 2026-04-16T12:06:29Z
---

# Phase 05 Plan 02: Subject Breakdown UI Summary

**Subject Breakdown widget with expandable accordions and progress bars integrated into the Dashboard.**

## Performance

- **Duration:** 4 min
- **Started:** 2026-04-16T12:06:29Z
- **Completed:** 2026-04-16T12:06:29Z
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments
- Created responsive SubjectBreakdownWidget with progress bars
- Implemented expandable cards to show Total and Canceled lessons per subject
- Added `TotalLessons` and `CanceledLessons` calculations to `CalculationService`
- Integrated widget into main Dashboard page

## Task Commits

Each task was committed atomically:

1. **Task 1: Create SubjectBreakdownWidget Component** - `ac7dcb9` (feat)
2. **Task 2: Integrate SubjectBreakdownWidget into Dashboard** - `bfffa8b` (feat)

## Files Created/Modified
- `SchoolTimeCalc/Components/Widgets/SubjectBreakdownWidget.razor` - Widget component displaying accordion subject list
- `SchoolTimeCalc/Components/Pages/Dashboard.razor` - Added the new widget to the view
- `SchoolTimeCalc/Models/SubjectRemainingLessons.cs` - Added Total/Canceled counts
- `SchoolTimeCalc/Services/CalculationService.cs` - Computed Total and Canceled lessons over all fetched Untis lessons

## Decisions Made
- Added `TotalLessons` and `CanceledLessons` to the CalculationService model to accurately support the breakdown requirements instead of mocking progress bars.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] Calculate total and canceled lessons for subjects**
- **Found during:** Task 1
- **Issue:** SubjectRemainingLessons model only had `RemainingLessons`, making it impossible to render "lessons vs total" and "canceled lessons" accurately.
- **Fix:** Added `TotalLessons` and `CanceledLessons` properties to `SubjectRemainingLessons.cs` and updated `CalculationService.cs` to calculate these counts over the full parsed Untis payload.
- **Files modified:** `SchoolTimeCalc/Models/SubjectRemainingLessons.cs`, `SchoolTimeCalc/Services/CalculationService.cs`
- **Verification:** Properties used in UI build without errors.
- **Committed in:** `ac7dcb9` (part of Task 1 commit)

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** Essential for accuracy of the dashboard visualization. No scope creep.

## Issues Encountered
None

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
Dashboard is populated with macro and subject breakdown widgets. Ready to proceed to final dashboard refinements or deployment.
