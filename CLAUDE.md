<!-- GSD:project-start source:PROJECT.md -->
## Project

**SchoolTimeCalc**

A web application for Austrian students to track their progress through the school year. It syncs with WebUntis to fetch timetables and integrates with an Austrian public API to automatically account for holidays, providing detailed calculations of remaining school days, hours, and specific lessons left per subject.

**Core Value:** Accurately and automatically calculating the exact remaining time (days, hours, and lessons) left in the school year, removing the manual effort of counting days and accounting for holidays or schedule changes.

### Constraints

- **Compatibility:** Must integrate seamlessly with WebUntis authentication and data formats.
- **Localization:** Specific to the Austrian school system and holiday structure.
- **Platform Constraints:** Since the current repo is a .NET Console App, the stack will likely pivot to ASP.NET to leverage the existing directory, or utilize a decoupled frontend/backend architecture.
<!-- GSD:project-end -->

<!-- GSD:stack-start source:research/STACK.md -->
## Technology Stack

## Recommended Stack
### Core Technologies
| Technology | Version | Purpose | Why Recommended |
|------------|---------|---------|-----------------|
| **.NET** | 10.0 (LTS) | Backend / Platform | Native C# ecosystem, high performance, and allows seamless transition from the existing Console App structure. |
| **Blazor Web App** | 10.0 | Full-Stack UI | Unified architecture (Auto Interactive mode). Allows sharing C# timetable/holiday models directly between the WebUntis API logic and the browser UI. Reduces context switching. |
| **EF Core** | 10.0 | ORM & Data Access | Standard for .NET. Simplifies mapping the complex nested JSON data from WebUntis (subjects, teachers, rooms, lessons) into a relational schema. |
| **PostgreSQL** | 17+ | Primary Database | Excellent JSONB support (useful for raw API responses from WebUntis/Holidays) and robust relational integrity for user caching. |
### Supporting Libraries
| Library | Version | Purpose | When to Use |
|---------|---------|---------|-------------|
| **Nager.Date** | 1.x (Latest) | Bank Holidays (Offline) | Computes standard Austrian public holidays (Feiertage) locally without external HTTP calls. Use for base non-school days. |
| **Refit** | 8.x | REST/JSON-RPC Client | Declarative API definitions. Dramatically simplifies wrapping the WebUntis JSON-RPC API and Austrian Open Data API. |
| **Polly** | 8.x | Resilience / Retries | WebUntis APIs and government APIs (data.gv.at) can rate-limit or drop connections. Essential for robust syncing. |
| **Hangfire / Quartz.NET**| Latest | Background Jobs | For syncing timetables asynchronously (e.g., nightly syncs) without blocking user requests. |
### Development Tools
| Tool | Purpose | Notes |
|------|---------|-------|
| **Npgsql.EntityFrameworkCore.PostgreSQL** | PostgreSQL EF Provider | Required for EF Core + Postgres integration. |
| **bUnit** | UI Component Testing | For unit testing Blazor components (calendar views, progress bars) independently. |
## Installation
# Core Web App Setup (assuming starting from scratch/pivoting)
# Data & Resilience
# API Clients & Utilities
## Alternatives Considered
| Recommended | Alternative | When to Use Alternative |
|-------------|-------------|-------------------------|
| **Blazor Web App** | **React / Vue SPA + ASP.NET Web API** | Use if a native mobile app (React Native) is a near-term goal requiring a decoupled JSON API, or if the team lacks Blazor experience. |
| **PostgreSQL** | **SQLite** | Use for the MVP/Phase 1 to avoid infrastructure overhead, migrating to Postgres later when user accounts scale. |
| **Refit** | **Raw HttpClient** | Use if the WebUntis JSON-RPC payload structures are too dynamic/inconsistent for static C# interfaces. |
## What NOT to Use
| Avoid | Why | Use Instead |
|-------|-----|-------------|
| **Community WebUntis NuGet Packages** | Most are abandoned (last updated 3-5 years ago) and don't support modern async/await or .NET 10 patterns. They break when WebUntis updates their API. | **Refit + Custom HttpClient** (Build a scoped wrapper specific to your needs) |
| **Hardcoding Holidays** | Austrian school holidays (Herbstferien, Semesterferien) vary dynamically by *Bundesland* (State) and change yearly. | **data.gv.at API + Nager.Date** |
| **Blazor WebAssembly (Standalone)** | Exposes API keys/secrets for the WebUntis sync directly to the client browser. | **Blazor Web App (Interactive Server or Auto)** |
## Stack Patterns by Variant
- Use **Blazor WebAssembly (Interactive WebAssembly)** for the UI, paired with a distinct ASP.NET Core API backend.
- Because offline timetables require caching data directly in the browser via IndexedDB, which Blazor WebAssembly handles natively while keeping WebUntis API credentials safe on the server.
- Use **Hangfire** backed by PostgreSQL.
- Because syncing thousands of students' schedules simultaneously during morning rush hour will overwhelm WebUntis rate limits. Queue them.
## Version Compatibility
| Package A | Compatible With | Notes |
|-----------|-----------------|-------|
| EF Core 10 | .NET 10 | Ensure Npgsql provider matches the major version (10.x). |
| Refit 8+ | `System.Text.Json` | Refit uses STJ by default now; no need for Newtonsoft.Json unless specifically required by a weird WebUntis serialization quirk. |
## Sources
- Official .NET 10 Release Notes / Documentation — HIGH confidence for core framework support.
- Austrian Open Data (data.gv.at) — HIGH confidence for school holiday data source.
- Nager.Date GitHub Repository — HIGH confidence for offline public holiday calculation in C#.
- WebUntis JSON-RPC Documentation — MEDIUM confidence (often community-maintained wikis vs official docs).
<!-- GSD:stack-end -->

<!-- GSD:conventions-start source:CONVENTIONS.md -->
## Conventions

Conventions not yet established. Will populate as patterns emerge during development.
<!-- GSD:conventions-end -->

<!-- GSD:architecture-start source:ARCHITECTURE.md -->
## Architecture

Architecture not yet mapped. Follow existing patterns found in the codebase.
<!-- GSD:architecture-end -->

<!-- GSD:workflow-start source:GSD defaults -->
## GSD Workflow Enforcement

Before using Edit, Write, or other file-changing tools, start work through a GSD command so planning artifacts and execution context stay in sync.

Use these entry points:
- `/gsd:quick` for small fixes, doc updates, and ad-hoc tasks
- `/gsd:debug` for investigation and bug fixing
- `/gsd:execute-phase` for planned phase work

Do not make direct repo edits outside a GSD workflow unless the user explicitly asks to bypass it.
<!-- GSD:workflow-end -->



<!-- GSD:profile-start -->
## Developer Profile

> Profile not yet configured. Run `/gsd:profile-user` to generate your developer profile.
> This section is managed by `generate-claude-profile` -- do not edit manually.
<!-- GSD:profile-end -->
