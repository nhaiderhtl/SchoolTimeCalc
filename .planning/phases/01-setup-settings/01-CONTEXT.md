# Phase 1: Setup & Settings - Context

**Gathered:** 2026-04-14
**Status:** Ready for planning

<domain>
## Phase Boundary

Application is scaffolded as a .NET 10 Blazor Web App and the user can configure their base settings (Austrian Bundesland) to prepare for accurate holiday calculations.

</domain>

<decisions>
## Implementation Decisions

### CSS Framework
- **D-01:** Use Tailwind CSS for styling the Blazor Web App to ensure a modern, utility-first UI approach.

### Initial User Experience & State Persistence
- **D-02:** No public landing page for unauthenticated users; the app should act as a gated utility.
- **D-03:** Users must authenticate (WebUntis Login, to be implemented in Phase 2) before they can select their state. For Phase 1, build the scaffolding and the settings page structure assuming an authenticated context.
- **D-04:** Immediately after a user logs in for the first time, they are forced to select their Austrian Bundesland.
- **D-05:** The selected Bundesland is persisted to the authenticated user's profile (in the database), not just local storage.

### the agent's Discretion
- The exact layout of the settings page and the visual representation of the Bundesland selection (e.g., dropdown vs. map/cards).
- How the transition from the current console app to the Blazor app is structured within the Git repository.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Project Architecture
- `.planning/research/STACK.md` — Explains the choice of .NET 10 Blazor Auto Interactive mode and EF Core/PostgreSQL.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- None. The current codebase is a newly scaffolded `net10.0` console application containing `Program.cs`. 

### Established Patterns
- None established yet. The project will need to be converted or replaced with a Blazor Web App project template.

### Integration Points
- Will replace the root `SchoolTimeCalc.csproj` console application.

</code_context>

<specifics>
## Specific Ideas

No specific design references or visual examples were provided — open to standard, clean modern approaches using Tailwind CSS.

</specifics>

<deferred>
## Deferred Ideas

- Actual WebUntis authentication logic is deferred to Phase 2 (WebUntis Integration). For Phase 1, mock the authenticated state to allow building and testing the Settings page.

</deferred>

---

*Phase: 01-setup-settings*
*Context gathered: 2026-04-14*