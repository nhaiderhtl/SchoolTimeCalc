# Architecture Patterns

**Domain:** Student Academic Tracking Application
**Researched:** 2026-04-14

## Recommended Architecture

A decoupled Frontend/Backend architecture or a unified ASP.NET Core Web application (e.g., Blazor). Given the existing .NET context and API integration needs, an architecture emphasizing a robust **Integration Layer** and **Caching Strategy** is strongly recommended to handle third-party data reliably.

### Component Boundaries

| Component | Responsibility | Communicates With |
|-----------|---------------|-------------------|
| **Web Frontend (Blazor/SPA)** | Renders dashboard, calendar view, progress charts | Backend API |
| **Backend API (ASP.NET Core)** | Exposes endpoints for progress data, auth, settings | Frontend, Calculation Engine, Database |
| **Calculation Engine** | Business logic: diffs current date, timetable, and holidays to yield remaining days/lessons | Backend API, Integration Adapters |
| **Integration Layer (WebUntis)** | Syncs user timetable, handles WebUntis session auth | Calculation Engine, WebUntis API |
| **Integration Layer (Holidays)** | Fetches & normalizes Austrian public school holidays | Calculation Engine, Austrian Gov API |
| **Database/Cache** | Caches user timetables (reduces API quotas), stores preferences | Backend API, Integration Layer |

### Data Flow

1. **Authentication:** User logs into the Web App, securely providing WebUntis session credentials.
2. **Ingestion:** Backend API requests the Integration Layer to fetch the user's timetable (from WebUntis) and regional holidays (from Austrian API).
3. **Caching:** Raw timetable and holiday data are cached in the Database to minimize external API roundtrips and improve load times.
4. **Processing:** The Calculation Engine merges the timetable with the holiday schedule, subtracting past days/lessons from the total to compute the remaining academic balance.
5. **Presentation:** Processed metrics (remaining school days, total remaining hours, per-subject breakdown) are returned to the Web Frontend via JSON or SignalR (if Blazor Server).

## Patterns to Follow

### Pattern 1: Adapter Pattern for External APIs
**What:** Isolate third-party API specifics (WebUntis proprietary formats, Austrian Gov data) behind standard internal interfaces (`ITimetableService`, `IHolidayProvider`).
**When:** ALWAYS, when communicating with external services.
**Example:**
```csharp
public interface ITimetableProvider
{
    Task<StudentTimetable> GetTimetableAsync(string studentId, DateTime start, DateTime end);
}
```

## Anti-Patterns to Avoid

### Anti-Pattern 1: Synchronous External Fetching on Load
**What:** Calling the WebUntis API and Holiday API synchronously every time a user loads the dashboard.
**Why bad:** High latency, potential rate-limiting from WebUntis, brittle UX if APIs are down.
**Instead:** Cache timetable data locally for a specific duration (e.g., 24 hours) and sync in the background or on explicit user request.

### Anti-Pattern 2: Unsafe Credential Storage
**What:** Saving student WebUntis credentials unencrypted in the DB to fetch timetables offline.
**Why bad:** Massive security vulnerability for student data.
**Instead:** Use WebUntis API token/session based authentication, and avoid persisting passwords if possible.

## Suggested Build Order (Dependencies)

1. **Integration Services (Foundation):** Build the WebUntis adapter and Holiday API consumer first. Without reliable data ingestion, the product cannot function.
2. **Calculation Engine (Core Logic):** Build the pure C# engine that merges the timetable data and holiday data to correctly calculate remaining days/lessons.
3. **Backend API & Persistence (Plumbing):** Wrap the logic in ASP.NET endpoints and introduce a local caching layer (SQLite/PostgreSQL or Redis) to avoid hitting APIs constantly.
4. **Web Frontend (Presentation):** Build the WebUI to consume the endpoints and display the calendar and metric dashboard.

## Sources

- Context from `.planning/PROJECT.md`
- Microsoft ASP.NET Core Architecture Guidelines
