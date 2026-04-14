# Project State

## Project Reference

- **Core Value:** Accurately and automatically calculating the exact remaining time (days, hours, and lessons) left in the school year, removing the manual effort of counting days and accounting for holidays or schedule changes.
- **Current Focus:** Scaffold application and set up user state configuration (Phase 1).

## Current Position

- **Phase:** 1. Setup & Settings
- **Plan:** Not started
- **Status:** Not started

### Progress
[ ] 0% (0/5 Phases)

## Performance Metrics

- **Velocity:** N/A (Project just starting)
- **Quality:** N/A

## Accumulated Context

### Architectural Decisions
- Application stack will be .NET 10 Blazor Web App with EF Core/PostgreSQL to allow shared C# models between integrations and UI.
- External dependencies (WebUntis and Holiday APIs) will be aggressively cached to avoid page load delays and API rate limits.

### Known Blockers
- None currently.
- Potential future challenge: finding exact Open Data endpoint for state-level school holidays on data.gv.at.

### Immediate Todos
- Run `/gsd-plan-phase 1` to break down Phase 1.

## Session Continuity

- **Last updated:** 2026-04-14
- **Next steps:** Start planning Phase 1 (Setup & Settings).
