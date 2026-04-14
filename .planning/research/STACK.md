# Stack Research

**Domain:** Student Academic Tracking Web App (WebUntis Integration)
**Researched:** 2026-04-14
**Confidence:** HIGH

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

```bash
# Core Web App Setup (assuming starting from scratch/pivoting)
dotnet new blazor -o SchoolTimeCalc.Web --interactivity Auto -m
cd SchoolTimeCalc.Web

# Data & Resilience
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.Extensions.Http.Resilience # Polly v8 integration

# API Clients & Utilities
dotnet add package Refit.HttpClientFactory
dotnet add package Nager.Date
```

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

**If offline support (PWA) becomes a strict requirement:**
- Use **Blazor WebAssembly (Interactive WebAssembly)** for the UI, paired with a distinct ASP.NET Core API backend.
- Because offline timetables require caching data directly in the browser via IndexedDB, which Blazor WebAssembly handles natively while keeping WebUntis API credentials safe on the server.

**If syncing delays become an issue:**
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

---
*Stack research for: Student Academic Tracking (WebUntis Integration)*
*Researched: 2026-04-14*