# Phase 3: Holiday Integration - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives considered.

**Date:** 2026-04-15
**Phase:** 3-Holiday Integration
**Areas discussed:** Bank Holiday Source, Caching Strategy, Error Handling

---

## Bank Holiday Source

| Option | Description | Selected |
|--------|-------------|----------|
| Nager.Date (Recommended) | Use Nager.Date library (fast, offline, standard C# solution) | ✓ |
| Public API | Fetch from data.gv.at or similar public API | |

**User's choice:** Nager.Date (Recommended)
**Notes:** 

---

## Caching Strategy

| Option | Description | Selected |
|--------|-------------|----------|
| Global Cache (Recommended) | Cache state holidays globally in DB, refresh periodically when users sync | ✓ |
| Per-User Cache | Fetch holidays per user and cache with their timetable data | |
| No Cache (Live) | Fetch directly from API on every calculation | |

**User's choice:** Global Cache (Recommended)
**Notes:** 

---

## Error Handling

| Option | Description | Selected |
|--------|-------------|----------|
| Soft fail (Recommended) | Log the error, keep using the previously cached holidays, and show a soft warning to the user | ✓ |
| Hard fail | Halt the sync completely and display a hard error requiring the user to try again later | |
| Silent fail | Ignore the error entirely and just skip school holidays in the calculation | |

**User's choice:** Soft fail (Recommended)
**Notes:** 

---

## the agent's Discretion

- Database schema design for the global holiday cache.
- Exact refresh interval for checking the data.gv.at API.
- HTTP client configuration (Polly retries).

## Deferred Ideas

None.
