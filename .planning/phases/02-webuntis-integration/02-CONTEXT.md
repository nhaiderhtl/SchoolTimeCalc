# Phase 2: WebUntis Integration - Context

**Gathered:** 2026-04-14
**Status:** Ready for planning

<domain>
## Phase Boundary

Securely authenticate the user with their WebUntis instance, retrieve their base timetable, and store the necessary data locally. This integration must be robust and act as the foundational data source for all future calculations.

</domain>

<decisions>
## Implementation Decisions

### Authentication Strategy
- **D-01:** Token Only (Zero Knowledge). The application will only ask for WebUntis credentials to fetch a short-lived session token. It will *never* store the user's WebUntis password in the database. Users will need to re-authenticate with WebUntis if their session token expires.

### Sync Frequency
- **D-02:** On-Demand / Login. Timetable data is only fetched when the user actively logs into the application or explicitly clicks a "Sync" button. No background or nightly sync jobs will be used.

### Data Storage Strategy
- **D-03:** Postgres JSONB. Store the raw WebUntis JSON payloads (subjects, teachers, rooms, lessons) within PostgreSQL `JSONB` columns. Extract and project only the top-level data needed for calculations. This approach leverages EF Core 10's native JSON capabilities and remains resilient to WebUntis API structural changes.

### the agent's Discretion
- Design and layout of the WebUntis login form (School Name, Username, Password).
- Structuring the Refit REST client for the WebUntis JSON-RPC API.
- Error handling UI (e.g., rate limits, invalid credentials, or Untis API outages).

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Project Architecture
- `.planning/research/STACK.md` — Detailed guidance on using Refit, Polly, and Postgres JSONB for the Untis integration.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `MockAuthService.cs` — The existing mock authentication service can be used as a reference point for dependency injection, but will be replaced or augmented by the actual WebUntis authentication service.
- The Tailwind CSS setup in `Settings.razor` and `Home.razor` provides the baseline for styling the new WebUntis login component.

### Established Patterns
- Blazor Web App (Interactive Server) architecture with Entity Framework Core for database access.

### Integration Points
- A new route (e.g., `/webuntis-login`) or a modal on the `Home` page needs to be integrated to trigger the WebUntis authentication flow.

</code_context>

<specifics>
## Specific Ideas

No specific UI constraints — focus on a clean, accessible login form. Ensure any API keys or secrets are handled securely server-side and never exposed to the browser.

</specifics>

<deferred>
## Deferred Ideas

- Background syncing (Hangfire/Quartz.NET) is explicitly deferred since the On-Demand approach was selected.
- Daily substitution/cancellation updates (SYNC-05) remain out of scope for v1, as this phase focuses purely on the base timetable.

</deferred>

---

*Phase: 02-webuntis-integration*
*Context gathered: 2026-04-14*
