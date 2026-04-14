# Domain Pitfalls: Student Academic Tracking App

**Domain:** EdTech / Student Academic Tracking App
**Researched:** 2026-04-14
**Overall Confidence:** HIGH

## Critical Pitfalls

Mistakes that cause rewrites, loss of user trust, or major system failures.

### Pitfall 1: WebUntis API Rate Limiting and Session Mismanagement
**What goes wrong:** The app hammers the WebUntis API on every user login or page load to fetch the live timetable. This quickly results in rate-limiting (HTTP 429), IP bans, and session expiration errors, breaking the app for all users.
**Why it happens:** Treating a 3rd-party school API as a fast, highly-available local database, and failing to account for the stateful session ID (`JSESSIONID`) mechanism WebUntis sometimes relies on.
**Consequences:** Complete service outage, angry users, and blocked servers.
**Prevention:** Implement an aggressive, intelligent caching layer (e.g., Redis). Fetch the timetable in the background (cron job) or cache it for at least 1-4 hours. Do not fetch synchronously on the critical path of page loads.
**Detection:** High API latency or increased HTTP 429/401 responses from the WebUntis client.
**Phase Addressed:** **Phase 2 / Backend Foundation** (when designing the sync service).

### Pitfall 2: Ignoring Timetable Exceptions (Substitutions/Cancellations)
**What goes wrong:** The app calculates "Total remaining Math lessons: 40". However, a teacher gets sick, 2 math lessons are cancelled, and a substitute teaches physics instead of math. The app fails to reflect this, displaying incorrect remaining times.
**Why it happens:** Fetching only the "base timetable" (Stundenplan) and ignoring the "substitution plan" (Vertretungsplan), which WebUntis tracks as exceptions.
**Consequences:** The core value proposition—"Accurately calculating exact remaining time"—is broken. Users lose trust and abandon the app.
**Prevention:** Data models must support "Overrides" or "Exceptions." When syncing with WebUntis, ensure the API request includes parameters to fetch actual current-week data (including substitutions/cancellations) rather than just the generic base schedule.
**Detection:** User reports of discrepancies between the app and the official WebUntis app.
**Phase Addressed:** **Phase 3 / Data Modeling & Sync** (when mapping WebUntis data).

### Pitfall 3: Regional Holiday Blindness (Bundesländer differences)
**What goes wrong:** The app miscalculates remaining days because it uses a generic "Austrian Public Holidays" list, missing staggered school holidays (Semesterferien). 
**Why it happens:** Austria has national public holidays (Feiertage), but school holidays (Schulferien) like the February Semester break are staggered across three different weeks depending on the state (e.g., Vienna vs. Styria).
**Consequences:** Off-by-a-week errors in day/lesson calculations for students outside the developer's home state.
**Prevention:** Use an API that explicitly provides *school holidays* per *Bundesland* (state), not just federal public holidays. The user must configure their state, or the app must infer it from the school's WebUntis profile.
**Detection:** Holiday calendar shows Semesterferien at the wrong time for certain users.
**Phase Addressed:** **Phase 2 / Holiday Integration** (when selecting the Holiday API).

## Moderate Pitfalls

### Pitfall 4: Naive Date Math and Timezones (DST Shifts)
**What goes wrong:** The app calculates "Days until summer break" and is off by one day, or lessons shift to the wrong day of the week.
**Why it happens:** Austria observes Daylight Saving Time (DST). Using naive `DateTime` math across the October/March DST boundaries can result in missing/extra hours, pushing boundaries past midnight.
**Prevention:** Use a robust timezone-aware library (like `NodaTime` if sticking with .NET). Store all absolute timestamps in UTC and project to `Europe/Vienna` only at the presentation layer. Use "Date-only" types for counting whole days.
**Phase Addressed:** **Phase 1 / Core Foundation** (when setting up the project's date utilities).

### Pitfall 5: Oversimplified Lesson Projection (A/B Weeks)
**What goes wrong:** Counting lessons by multiplying "Weekly Math Lessons (3)" × "Weeks Left (10)" = 30.
**Why it happens:** Assuming a static weekly schedule. High schools often have A/B weeks (ungerade/gerade Wochen) or block subjects that only run for half a semester.
**Prevention:** Do not use multiplication for projections. Instead, generate a "Virtual Calendar" from today until the end of the school year. Iterate day-by-day, applying the specific week's schedule, skipping holidays and weekends, and counting the actual instances of each subject.
**Phase Addressed:** **Phase 4 / Calculation Logic** (when writing the counting algorithms).

## Phase-Specific Warnings

| Phase Topic | Likely Pitfall | Mitigation |
|-------------|---------------|------------|
| **Authentication** | Storing WebUntis plaintext credentials. | Avoid storing user passwords if WebUntis provides OAuth/tokens. If credentials must be stored for background sync, encrypt them symmetrically at rest. |
| **Data Sync** | Endless sync loops on bad data. | Implement circuit breakers and max-retry limits when parsing 3rd-party API responses. |
| **Calculation Engine** | Sluggish performance from calculating counts on the fly. | Materialize the calculations. Run the counting algorithm on a background worker and save the "remaining counts" to the DB, updating only when the timetable/holidays change. |

## Sources

- Domain knowledge: Calendar app architecture, WebUntis API community documentation, Timezone and DST programming standards.
- Confidence: HIGH (These are universally acknowledged architectural and domain-specific challenges for school timetable integrations).