# SchoolTimeCalc

## What This Is

A web application for Austrian students to track their progress through the school year. It syncs with WebUntis to fetch timetables and integrates with an Austrian public API to automatically account for holidays, providing detailed calculations of remaining school days, hours, and specific lessons left per subject.

## Core Value

Accurately and automatically calculating the exact remaining time (days, hours, and lessons) left in the school year, removing the manual effort of counting days and accounting for holidays or schedule changes.

## Requirements

### Validated

<!-- Shipped and confirmed valuable. -->

- [x] Sync user's school timetable automatically from WebUntis (Phase 02)
- [x] Fetch official Austrian school holidays dynamically via an API (Phase 03)
- [x] Calculate total remaining school days in the academic year (Phase 04)
- [x] Calculate total remaining hours/lessons for the academic year (Phase 04)
- [x] Calculate remaining lessons on a per-subject basis (Phase 04)
- [x] Display a calendar interface showing the timetable and holidays (Phase 05)

### Active

<!-- Current scope. Building toward these. -->

### Out of Scope

<!-- Explicit boundaries. Includes reasoning to prevent re-adding. -->

- Native Mobile App — Focused on a responsive Web App for v1 accessibility.
- Manual Timetable Entry — The core value lies in automatic syncing with WebUntis.
- Daily Dismissal Countdown — Focused primarily on macro/yearly progress tracking, though daily schedule is available.

## Context

- **Platform:** Web Application
- **Existing Setup:** Currently initialized as a barebones .NET Console App, needs to be transitioned to a web project structure (e.g., ASP.NET Core Web API or Blazor).
- **Target Audience:** Students in Austria using WebUntis.
- **External Dependencies:** WebUntis API and an Austrian Public Holiday API.

## Constraints

- **Compatibility:** Must integrate seamlessly with WebUntis authentication and data formats.
- **Localization:** Specific to the Austrian school system and holiday structure.
- **Platform Constraints:** Since the current repo is a .NET Console App, the stack will likely pivot to ASP.NET to leverage the existing directory, or utilize a decoupled frontend/backend architecture.

## Key Decisions

| Decision | Rationale | Outcome |
|----------|-----------|---------|
| WebUntis Sync | Most prevalent school management software in Austria, eliminates manual data entry | — Pending |
| Live API for Holidays | Ensures accuracy year over year without manual updates | — Pending |
| Yearly Focus | Prioritizing macro tracking (remaining days/subject lessons) over micro tracking (time until bell rings) | — Pending |

## Evolution

This document evolves at phase transitions and milestone boundaries.

**After each phase transition** (via `/gsd-transition`):
1. Requirements invalidated? → Move to Out of Scope with reason
2. Requirements validated? → Move to Validated with phase reference
3. New requirements emerged? → Add to Active
4. Decisions to log? → Add to Key Decisions
5. "What This Is" still accurate? → Update if drifted

**After each milestone** (via `/gsd-complete-milestone`):
1. Full review of all sections
2. Core Value check — still the right priority?
3. Audit Out of Scope — reasons still valid?
4. Update Context with current state

---
*Last updated: 2026-04-16 after phase 05 completion*
