---
phase: 03-WebUntis-Holiday-Integration
plan: 06
subsystem: Core
tags: ["holidays", "calculation", "tests"]
requires: ["03-05"]
provides: ["Holiday-integrated calculations", "Real service and integration tests"]
affects: ["CalculationEngine", "Home.razor", "HolidaySyncServiceTests"]
tech-stack:
  added: ["Moq.Protected"]
  patterns: ["Dependency Injection", "Service Stubbing"]
key-files:
  created: ["SchoolTimeCalc/Services/CalculationEngine.cs", ".planning/phases/03-WebUntis-Holiday-Integration/03-06-SUMMARY.md"]
  modified: ["SchoolTimeCalc/Program.cs", "SchoolTimeCalc/Components/Pages/Home.razor", "SchoolTimeCalc.Tests/HolidaySyncServiceTests.cs"]
decisions:
  - "Extract calculation logic into a standalone CalculationEngine to centralize calculation and inject into Razor components."
  - "Use Moq.Protected to mock HttpMessageHandler instead of mocking Refit directly to test HolidaySyncService logic."
metrics:
  duration: 10
  tasks_completed: 2
  files_changed: 4
---

# Phase 03 Plan 06: Integrate cached holidays into calculation logic and replace placeholder tests

Integrated user holidays into a robust `CalculationEngine` service that calculates actual remaining school days, and added robust unit testing to ensure data mapping correctly accounts for both general Austrian public holidays and region-specific school holidays.

## Plan Goals
- System calculates total school days excluding holidays
- Test coverage replaces placeholders with real tests

## Tasks Completed

### Task 1: Integrate Holidays into CalculationEngine
- Created `CalculationEngine` service injected into the application structure.
- Loaded WebUntis location data to filter and compute remaining school days dynamically based on cached standard and state holidays.
- Showed remaining days excluding holidays gracefully onto `Home.razor`.

### Task 2: Replace Placeholder Tests
- Discarded placeholder `Assert.True(true)` assertions.
- Intercepted `HttpClient` instances utilizing `Moq.Protected` to accurately map tests for `Refit` REST behavior implicitly.
- Validated `SchoolHolidayService` state mapping routing and generic state API retrieval parameters accurately.

## Deviations from Plan

**1. [Rule 2 - Missing Functionality] Created CalculationEngine**
- **Found during:** Task 1
- **Issue:** The application lacked an actual `CalculationEngine.cs` despite being referenced in the plan as needing modifications.
- **Fix:** Created the `CalculationEngine.cs` from scratch and implemented the underlying calculation loop logic excluding weekends and holidays. Included it in `Program.cs`.

## Known Stubs
None

## Self-Check: PASSED
- `SchoolTimeCalc/Services/CalculationEngine.cs` generated and logic deployed.
- Placeholder assertions purged from `HolidaySyncServiceTests.cs`.
- `Home.razor` modified visually correctly.