---
phase: 03-WebUntis-Holiday-Integration
plan: 05
subsystem: Sync
tags: [auth, sync, map, ui]
requirements: ["SYNC-03", "SYNC-04"]
dependency_graph:
  requires: ["03-04-PLAN.md"]
  provides: ["Working WebUntis credentials persistence and Austrian state mapping"]
  affects: ["WebUntisService", "WebUntisHolidaySyncService", "SchoolHolidayService"]
tech_stack:
  added: []
  patterns: ["Exception bubbling", "Integration with Entity Framework"]
key_files:
  created: ["SchoolTimeCalc/Migrations/20260416110000_AddUntisUsername.cs"]
  modified: ["SchoolTimeCalc/Models/WebUntisData.cs", "SchoolTimeCalc/Services/HolidaySyncBackgroundService.cs", "SchoolTimeCalc/Services/SchoolHolidayService.cs", "SchoolTimeCalc/Services/WebUntisHolidaySyncService.cs", "SchoolTimeCalc/Services/WebUntisService.cs", "SchoolTimeCalc/Migrations/ApplicationDbContextModelSnapshot.cs"]
key_decisions:
  - "Add Username field to WebUntisData table to persist untis user login for background sync alongside Server and EncryptedPassword."
  - "Bubble up exceptions from WebUntisHolidaySyncService so that UI logic catches them and displays appropriate user error messages."
  - "Directly trigger SyncHolidaysAsync from AuthenticateAndSyncAsync to fulfill the 'alongside timetables' requirement."
metrics:
  duration: "10m"
  completed_date: "2026-04-16"
---

# Phase 03 Plan 05: Fix core integration issues Summary

**One-Liner:** Fixed WebUntis credentials persistence, mapped correct Austrian states, and improved error handling for holiday sync.

## Overview
This plan implements the fixes resulting from Phase 03 code reviews. The main goals were ensuring that background syncing had access to complete credentials (Server, Username, Password), the background sync triggered automatically alongside timetable sync, the subdivision codes accurately mapped to Austrian states, and syncing errors produced clear visual feedback on the UI rather than silent failures.

## Actions Taken
1. Added `Username` to `WebUntisData` and created a migration so background service has proper WebUntis credentials.
2. Updated `WebUntisService.AuthenticateAndSyncAsync` to persist `Server`, `Username`, and `EncryptedPassword`.
3. Updated `WebUntisService.AuthenticateAndSyncAsync` to call `WebUntisHolidaySyncService.SyncHolidaysAsync`.
4. Fixed the state mapping in `SchoolHolidayService.FetchAndCacheSchoolHolidaysAsync` by implementing a dictionary to accurately map state names to ISO 3166-2:AT subdivision codes (e.g. Wien -> AT-9, Niederösterreich -> AT-3).
5. Refactored `WebUntisHolidaySyncService.SyncHolidaysAsync` to throw specific exceptions upon authentication or retrieval failures, allowing `Settings.razor` to display an error notification instead of falsely succeeding.

## Deviations from Plan
None - plan executed exactly as written.

## Self-Check: PASSED
- Created files verified.
- Commits exist.
- Required logic implemented.