# Phase 05: Presentation & Dashboard - Context

**Gathered:** 2026-04-16
**Status:** Ready for planning

<domain>
## Phase Boundary

Deliver the user-facing web dashboard and interactive views. This phase takes the data calculated in Phase 04 and presents it to the user, including high-level macro progress, detailed subject-by-subject remaining lessons, and an interactive calendar view overlaid with holidays.

</domain>

<decisions>
## Implementation Decisions

### Calendar View (UI-03)
- **D-01:** The interactive calendar view will be structured as a **Week View**. This is standard for school timetables and is easier to read on mobile devices compared to a dense month view.

### Mobile Experience for Dense Data (UI-02 & UI-04)
- **D-02:** On mobile devices, dense tabular data (like the remaining lessons per subject) will be displayed using **Expandable Cards (Accordions)**. Key information will be shown upfront, and users can tap to expand and see details like canceled lessons.

### Remaining Lessons Breakdown (UI-02)
- **D-03:** The detailed view showing remaining lessons on a per-subject basis will use **Progress Bars** to visually represent how much of the year is left per subject.

### the agent's Discretion
- The exact layout and color palette of the progress bars and expandable cards.
- The mechanism for manually refreshing or syncing data on the dashboard.
- How holidays are visually overlaid or highlighted in the Week View calendar.

</decisions>

<specifics>
## Specific Ideas

- The user specifically requested a view where "the remaining school days can be broken down into the subjects, showing how many lessons of a certain subject are still remaining." This aligns with the Progress Bars decision (D-03).

</specifics>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Project Specs
- `.planning/PROJECT.md` — Active UI requirements (calendar interface, mobile responsiveness).
- `.planning/REQUIREMENTS.md` — UI-01, UI-02, UI-03, and UI-04 specifications.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- The existing Blazor Web App structure established in Phase 01.
- Tailwind CSS configuration for styling the dashboard components.
- Models and services from the Calculation Engine (Phase 04).

### Established Patterns
- Auto Interactive mode for Blazor components.

### Integration Points
- The dashboard components will consume the `ICalculationService` to fetch the remaining days, hours, and lessons data.

</code_context>

<deferred>
## Deferred Ideas

- None — discussion stayed within phase scope.

</deferred>

---

*Phase: 05-presentation-dashboard*
*Context gathered: 2026-04-16*