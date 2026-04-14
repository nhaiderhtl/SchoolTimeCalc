---
phase: 02-webuntis-integration
verified: 2026-04-14T13:55:00Z
status: gaps_found
score: 4/7 must-haves verified
gaps:
  - truth: "Refit interface accurately defines the WebUntis JSON-RPC endpoints"
    status: failed
    reason: "IWebUntisClient only defines AuthenticateAsync, missing all data-fetching endpoints and logout."
    artifacts:
      - path: "SchoolTimeCalc/Services/IWebUntisClient.cs"
        issue: "Missing endpoints for getSubjects, getTeachers, getRooms, getTimetable, logout"
    missing:
      - "Refit methods and corresponding request/response models for timetable data and logout."

  - truth: "System fetches and stores base timetable data as JSON in the database"
    status: failed
    reason: "WebUntisService hardcodes JSON payloads to empty arrays '[]' instead of fetching from the API."
    artifacts:
      - path: "SchoolTimeCalc/Services/WebUntisService.cs"
        issue: "Hardcoded empty arrays for subjectsJson, teachersJson, roomsJson, and lessonsJson"
    missing:
      - "Actual Refit calls to fetch the timetable data and map them to the JSON variables."

  - truth: "Service can orchestration login, fetch, and logout flows with WebUntis"
    status: failed
    reason: "Logout is commented out as a placeholder, meaning sessions are left open."
    artifacts:
      - path: "SchoolTimeCalc/Services/WebUntisService.cs"
        issue: "Placeholder comment for logout instead of actual implementation"
    missing:
      - "Implement LogoutAsync in the Refit client and call it in a finally block to ensure cleanup."
---

# Phase 2: WebUntis Integration Verification Report

**Phase Goal:** Securely authenticate the user with their WebUntis instance, retrieve their base timetable, and store the necessary data locally.
**Verified:** 2026-04-14T13:55:00Z
**Status:** gaps_found
**Re-verification:** No

## Goal Achievement

### Observable Truths

| # | Truth | Status | Evidence |
| --- | --- | --- | --- |
| 1 | Database schema can store raw WebUntis JSON responses | ✓ VERIFIED | `ApplicationDbContext` configures `jsonb` column types. |
| 2 | Refit interface accurately defines the WebUntis JSON-RPC endpoints | ✗ FAILED | `IWebUntisClient` only defines `AuthenticateAsync`. |
| 3 | Polly resilience policies are configured for WebUntis API calls | ✓ VERIFIED | `Program.cs` registers `WaitAndRetryAsync` for the Refit client. |
| 4 | System fetches and stores base timetable data as JSON in the database | ✗ FAILED | Data variables are hardcoded to `"[]"`. |
| 5 | Service can orchestration login, fetch, and logout flows with WebUntis | ✗ FAILED | Fetching is stubbed; logout is commented out. |
| 6 | User can enter WebUntis credentials in a login form | ✓ VERIFIED | `WebUntisLogin.razor` has the required form and bindings. |
| 7 | User sees a success state when their timetable is successfully synced | ✓ VERIFIED | `Home.razor` displays "Timetable Synced Successfully" state. |

**Score:** 4/7 truths verified

### Required Artifacts

| Artifact | Expected | Status | Details |
| --- | --- | --- | --- |
| `SchoolTimeCalc/Models/WebUntisData.cs` | EF Core entity for JSONB | ✓ VERIFIED | Exists, substantive properties, wired to DbContext. |
| `SchoolTimeCalc/Services/IWebUntisClient.cs` | Refit client interface | ✗ STUB | Missing endpoints for fetching and logout. |
| `SchoolTimeCalc/Services/WebUntisService.cs` | Orchestration logic | ✗ STUB | Contains hardcoded empty arrays and placeholder comments. |
| `SchoolTimeCalc/Components/Pages/WebUntisLogin.razor` | UI form for auth | ✓ VERIFIED | Fully implemented and wired. |

### Key Link Verification

| From | To | Via | Status | Details |
| --- | --- | --- | --- | --- |
| `Program.cs` | `IWebUntisClient` | AddRefitClient | ✓ WIRED | Correctly registered with Polly. |
| `WebUntisService.cs` | `IWebUntisClient` | DI | ✓ WIRED | Injected via constructor. |
| `WebUntisLogin.razor` | `WebUntisService.cs` | DI | ✓ WIRED | Injected and invoked in `HandleSubmit`. |

### Data-Flow Trace (Level 4)

| Artifact | Data Variable | Source | Produces Real Data | Status |
| --- | --- | --- | --- | --- |
| `WebUntisService.cs` | `subjectsJson` etc. | Hardcoded string | No | ✗ HOLLOW |

### Behavioral Spot-Checks

| Behavior | Command | Result | Status |
| --- | --- | --- | --- |
| WebUntis Login | N/A | N/A | ? SKIP (Blazor UI requires server interaction) |

### Requirements Coverage

| Requirement | Source Plan | Description | Status | Evidence |
| --- | --- | --- | --- | --- |
| SYNC-01 | 02-01, 02-02, 02-03 | User can authenticate with their school's WebUntis instance. | ✓ SATISFIED | Auth endpoint and form are implemented. |
| SYNC-02 | 02-01, 02-02 | System automatically fetches and securely caches the user's base timetable. | ✗ BLOCKED | Fetching is stubbed out with empty arrays. |

### Anti-Patterns Found

| File | Line | Pattern | Severity | Impact |
| --- | --- | --- | --- | --- |
| `SchoolTimeCalc/Services/WebUntisService.cs` | 44 | Hardcoded empty data | 🛑 Blocker | No actual data is fetched or stored. |
| `SchoolTimeCalc/Services/WebUntisService.cs` | 64 | Placeholder comment | 🛑 Blocker | Logout is not called, leaving sessions open. |

### Human Verification Required

None at this time (blocked by programmatic gaps).

### Gaps Summary

The authentication step is properly implemented with a UI and database schema ready to store JSONB data. However, the data fetching and session cleanup (logout) are incomplete stubs. `IWebUntisClient` needs the remaining JSON-RPC endpoints, and `WebUntisService` must be updated to make actual API calls instead of saving hardcoded empty arrays `"[]"`. Additionally, the logout method must be implemented and called to adhere to the zero-knowledge security constraint.

---
_Verified: 2026-04-14T13:55:00Z_
_Verifier: the agent (gsd-verifier)_