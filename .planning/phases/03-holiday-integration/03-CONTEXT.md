# Phase 3: Holiday Integration - Context

**Gathered:** 2026-04-15
**Status:** Ready for planning

<domain>
## Phase Boundary

Application has comprehensive data of all Austrian holidays for the academic year. This includes both public bank holidays and state-specific school holidays based on the user's selected Bundesland, which will be necessary for calculations in Phase 4.
</domain>

<decisions>
## Implementation Decisions

### Bank Holiday Source
- **D-01:** Use the `Nager.Date` library (offline, C# native) to compute standard Austrian public bank holidays (Feiertage).

### Caching Strategy
- **D-02:** Cache state-specific school holidays (from data.gv.at or similar) globally in the database.
- **D-03:** Refresh the global cache periodically when users trigger a sync, to avoid redundant API calls per user for the exact same state data.

### Error Handling
- **D-04:** Soft fail on API errors (e.g. data.gv.at outage). Log the error, continue using the previously cached holidays for that state, and show a soft warning to the user that holiday data might be slightly outdated.

### the agent's Discretion
- Database schema design for the global holiday cache (e.g., storing by year/state).
- Exact refresh interval (e.g., daily or weekly) for checking the data.gv.at API.
- HTTP client configuration (Polly retries, timeouts) for the external API.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Project Architecture
- `.planning/research/STACK.md` — Mentions Nager.Date for bank holidays, Austrian Open Data API, and Postgres JSONB support which might be useful for caching.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- The `ApplicationDbContext` will need a new table or property for caching global holidays.

### Established Patterns
- Entity Framework Core for DB access, aligned with Phase 1 and Phase 2.

### Integration Points
- Will be integrated into the existing manual Sync process implemented in Phase 2, augmenting the WebUntis fetch with the Holiday fetch.

</code_context>

<specifics>
## Specific Ideas

No specific UI or additional requirements — strictly data fetching and caching for the calculation engine in Phase 4.

</specifics>

<deferred>
## Deferred Ideas

None — discussion stayed within phase scope.

</deferred>

---

*Phase: 03-holiday-integration*
*Context gathered: 2026-04-15*
