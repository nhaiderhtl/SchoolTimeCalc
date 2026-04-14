# Project Research Summary

**Project:** SchoolTimeCalc
**Domain:** Student Academic Tracking Web App (WebUntis Integration)
**Researched:** 2026-04-14
**Confidence:** HIGH

## Executive Summary

SchoolTimeCalc is a web-based academic tracking application focused on calculating the remaining school days and lessons by integrating with the WebUntis API and Austrian school holiday datasets. It provides a "zero-entry" value proposition where all schedule and holiday data is fully automated, eliminating the tedious manual entry required by competitor apps.

The recommended technical approach is a .NET 10 Blazor Web App (Auto Interactive mode) backed by EF Core, PostgreSQL, and Refit. This provides a unified C# architecture, allowing timetable and holiday models to be shared seamlessly between the backend sync logic and the frontend browser UI. 

The major risks involve handling third-party integrations: WebUntis API rate limits, varying regional school holidays (Bundesländer differences), and timetable exceptions like canceled or substituted lessons. Mitigating these requires a robust architecture emphasizing an aggressive caching strategy (preventing synchronous fetches on page load) and a precise calculation engine that accounts for A/B weeks, DST shifts, and state-specific holidays.

## Key Findings

### Recommended Stack

The application should be built on the latest .NET ecosystem to maximize developer productivity and performance, transitioning smoothly from any existing console app structure.

**Core technologies:**
- **.NET 10 & Blazor Web App:** Full-Stack UI — Unified architecture, allows sharing C# models directly between API logic and UI.
- **EF Core 10 & PostgreSQL:** ORM & Primary Database — Robust relational integrity and excellent JSONB support for caching raw WebUntis/Holiday API responses.
- **Refit & Polly:** REST/JSON-RPC Client & Resilience — Declaratively wraps external APIs and handles retries/rate-limiting seamlessly.
- **Nager.Date:** Bank Holidays — Computes standard Austrian public holidays offline.

### Expected Features

**Must have (table stakes):**
- Current Timetable View — Standard agenda view to build trust.
- Holiday/Break Accounting — Essential for accurate time tracking.
- Secure Authentication — Handling school data requires trusted WebUntis login.
- Basic Progress Metrics — Simple "time left" (days/weeks).
- Mobile-Friendly UI — Responsive design is mandatory for students.

**Should have (competitive):**
- Zero-Entry Sync (WebUntis) — Core differentiator; fully automated setup.
- Subject-Level Granularity — Knowing exact lessons left per subject provides unique motivation.
- Dynamic Austrian Holidays — Live API integration for state-specific breaks.
- Macro Year-at-a-Glance — Visualizing a year's progress intuitively.

**Defer (v2+):**
- Custom Excluded Days (absences/sick leave)
- Push Notifications
- Support for other systems outside Austria/WebUntis.

### Architecture Approach

A decoupled yet unified ASP.NET Core architecture emphasizing a robust Integration Layer and local caching to protect against 3rd-party API instability.

**Major components:**
1. **Integration Layer (WebUntis & Holidays)** — Adapters fetching and normalizing proprietary 3rd-party data securely.
2. **Database/Cache** — Persists user timetables and holidays to strictly limit external API roundtrips.
3. **Calculation Engine** — Core C# business logic that merges timetables with holidays to compute remaining academic balances.
4. **Web Frontend (Blazor)** — Renders the processed metrics, charts, and calendar views to the user.

### Critical Pitfalls

1. **WebUntis API Rate Limiting** — Prevent by implementing an aggressive caching layer; never fetch timetables synchronously on the critical path of a page load.
2. **Ignoring Timetable Exceptions** — Prevent by fetching the "substitution plan" (Vertretungsplan) alongside the base schedule to account for sickness/cancellations.
3. **Regional Holiday Blindness** — Prevent by using an API that provides school holidays per *Bundesland* (state), not just federal public holidays.
4. **Naive Date Math and Timezones (DST)** — Prevent by storing absolute timestamps in UTC and using robust timezone-aware libraries, projecting to Europe/Vienna only at presentation.

## Implications for Roadmap

Based on research, suggested phase structure:

### Phase 1: Foundation & Integration
**Rationale:** External data ingestion is the fundamental bottleneck; without it, calculations cannot occur.
**Delivers:** WebUntis auth adapter, Holiday API consumer, and Database caching layer.
**Addresses:** WebUntis Login & Data Fetch, Holiday API Integration.
**Avoids:** WebUntis API Rate Limiting (establishes caching from day 1) and Regional Holiday Blindness.

### Phase 2: Core Calculation Engine
**Rationale:** Once data is reliably cached, the core business logic must process it before any UI can be built.
**Delivers:** Pure C# engine that merges timetable data with holidays and computes remaining days/lessons.
**Addresses:** Global Progress Calc (Days/Hours), Subject-Specific Lesson Calc.
**Avoids:** Oversimplified Lesson Projection (A/B weeks), Naive Date Math (DST).

### Phase 3: Presentation & Dashboard
**Rationale:** Surfaces the validated metrics to the user.
**Delivers:** Mobile-responsive Blazor UI, macro progress dashboard, and calendar view.
**Uses:** Blazor Web App (Auto), bUnit.
**Implements:** Web Frontend (Blazor/SPA).

### Phase Ordering Rationale

- **Dependency-Driven:** The app cannot display progress without calculations, and cannot calculate without cached data. Therefore, Integrations -> Calculations -> UI is the strict path.
- **Risk Mitigation:** Tackling the WebUntis JSON-RPC integration and Austrian API in Phase 1 eliminates the highest technical risks immediately.

### Research Flags

Phases likely needing deeper research during planning:
- **Phase 1:** Needs `/gsd-research-phase` to pinpoint the exact Austrian Open Data (data.gv.at) endpoints for state-level school holidays and the specific WebUntis JSON-RPC methods for substitutions.

Phases with standard patterns (skip research-phase):
- **Phase 2 & 3:** Standard C# calculation logic and Blazor UI components; well-documented patterns apply.

## Confidence Assessment

| Area | Confidence | Notes |
|------|------------|-------|
| Stack | HIGH | Based on official .NET 10 docs and established ecosystem tools (EF Core, Refit). |
| Features | HIGH | Clear alignment on value prop vs. scope creep. |
| Architecture | HIGH | Standard ASP.NET Core integration and caching patterns apply perfectly here. |
| Pitfalls | HIGH | Known and heavily documented challenges for ed-tech / calendar apps. |

**Overall confidence:** HIGH

### Gaps to Address

- **WebUntis Substitutions Format:** We need to validate exactly how the WebUntis API returns substituted/canceled lessons to map them correctly in Phase 1.
- **Austrian Open Data Holiday API:** Need to identify the specific, reliable dataset ID on data.gv.at that provides staggered *school* holidays (Schulferien) per Bundesland, not just national bank holidays.

## Sources

### Primary (HIGH confidence)
- Official .NET 10 Release Notes / Documentation — Framework and tooling support
- Austrian Open Data (data.gv.at) — School holiday data source
- Nager.Date GitHub Repository — Offline public holiday calculation

### Secondary (MEDIUM confidence)
- WebUntis JSON-RPC Documentation — Often community-maintained, requires live validation
- EdTech / Student Calendar App feature patterns (MyStudyLife, Notion templates)

---
*Research completed: 2026-04-14*
*Ready for roadmap: yes*