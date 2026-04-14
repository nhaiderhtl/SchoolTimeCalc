# Phase 1: Setup & Settings - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives considered.

**Date:** 2026-04-14
**Phase:** 1-Setup & Settings
**Areas discussed:** CSS Framework, Landing Experience, State Persistence

---

## CSS Framework

| Option | Description | Selected |
|--------|-------------|----------|
| Tailwind CSS | Utility-first CSS, very popular for modern apps | ✓ |
| MudBlazor | Material Design components specifically built for Blazor | |
| Bootstrap | Standard Bootstrap (default in Blazor template) | |

**User's choice:** Tailwind CSS

---

## Landing Experience

| Option | Description | Selected |
|--------|-------------|----------|
| Welcome Screen First | Show a welcome screen, then ask for state and WebUntis login | |
| Select State Immediately | Force the user to select their Bundesland immediately upon first visit | ✓ |

**User's choice:** Select State Immediately

---

## State Persistence

| Option | Description | Selected |
|--------|-------------|----------|
| Local Storage First | Store state in browser local storage until they log in to WebUntis | |
| Login First | Require WebUntis login before selecting state and save to user profile | ✓ |

**User's choice:** Login First

---

## Flow Clarification

| Option | Description | Selected |
|--------|-------------|----------|
| Login -> State Selection | Show login screen first, then force state selection after login | ✓ |
| State Selection -> Login | Show state selection first, then login screen | |

**User's choice:** Login -> State Selection

---

## the agent's Discretion

- Visual representation of the Bundesland selection (e.g., dropdown vs. map/cards).
- How the transition from the current console app to the Blazor app is structured within the Git repository.

## Deferred Ideas

- Actual WebUntis authentication logic is deferred to Phase 2.
