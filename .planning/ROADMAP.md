# Project Roadmap

## Phases

- [ ] **Phase 1: Setup & Settings** - Application scaffolding and state configuration
- [ ] **Phase 2: WebUntis Integration** - Secure authentication and timetable synchronization
- [ ] **Phase 3: WebUntis Holiday Integration** - Fetching holidays directly from the WebUntis API
- [ ] **Phase 4: Calculation Engine** - Computing remaining days, hours, and subject lessons
- [ ] **Phase 5: Presentation & Dashboard** - Mobile-responsive UI with metrics and calendar views

## Phase Details

### Phase 1: Setup & Settings
**Goal**: Application is scaffolded and user can configure their base settings
**Depends on**: None
**Requirements**: SET-01
**Success Criteria** (what must be TRUE):
  1. Application compiles and runs as a modern web application (e.g., Blazor)
  2. User can select their specific Austrian Bundesland and save it to their profile/session
**Plans**: 3 plans
- [x] 01-01-PLAN.md — Scaffold Blazor Web App and Tailwind CSS
- [x] 01-02-PLAN.md — Configure EF Core, PostgreSQL, and Mock Auth
- [x] 01-03-PLAN.md — Build Settings UI for Bundesland selection
**UI hint**: yes

### Phase 2: WebUntis Integration
**Goal**: Application can securely authenticate and retrieve the user's base timetable
**Depends on**: Phase 1
**Requirements**: SYNC-01, SYNC-02
**Success Criteria** (what must be TRUE):
  1. User can successfully authenticate using their WebUntis credentials
  2. System retrieves and stores the user's timetable securely without requiring re-fetch on every page load
**Plans**: 3 plans
- [x] 02-01-PLAN.md — Database Models & API Client Contracts
- [x] 02-02-PLAN.md — Authentication & Sync Service
- [x] 02-03-PLAN.md — Login UI & Integration

### Phase 3: WebUntis Holiday Integration
**Goal**: Retrieve and cache holiday data directly from the WebUntis API to exclude those days from calculations
**Depends on**: Phase 2
**Requirements**: SYNC-03
**Success Criteria** (what must be TRUE):
  1. System successfully fetches holiday data from WebUntis API alongside timetables
  2. Holiday data is structured to be subtracted from or not added to total school day counts
**Plans**: 3 plans
- [x] 03-01-PLAN.md — Holiday Data Models & API Client Expansion
- [x] 03-02-PLAN.md — Holiday Sync Service
- [x] 03-03-PLAN.md — Integration & Settings UI Updates

### Phase 4: Calculation Engine
**Goal**: Application accurately computes all remaining academic time metrics
**Depends on**: Phase 2, Phase 3
**Requirements**: CALC-01, CALC-02, CALC-03, CALC-04
**Success Criteria** (what must be TRUE):
  1. System outputs total remaining school days accurately excluding all holidays and weekends
  2. System outputs total remaining lessons globally and broken down per subject
  3. Calculations successfully merge the base timetable with the acquired holiday datasets
**Plans**: TBD

### Phase 5: Presentation & Dashboard
**Goal**: Users can view their remaining time and schedules via an accessible interface
**Depends on**: Phase 4
**Requirements**: UI-01, UI-02, UI-03, UI-04
**Success Criteria** (what must be TRUE):
  1. User can view high-level macro progress (days/hours) on a mobile-friendly dashboard
  2. User can navigate to a detailed breakdown of remaining lessons per subject
  3. User can view an interactive calendar displaying their timetable overlaid with holidays
**Plans**: TBD
**UI hint**: yes

## Progress

| Phase | Plans Complete | Status | Completed |
|-------|----------------|--------|-----------|
| 1. Setup & Settings | 0/0 | Not started | - |
| 2. WebUntis Integration | 0/0 | Not started | - |
| 3. WebUntis Holiday Integration | 2/3 | In Progress|  |
| 4. Calculation Engine | 0/0 | Not started | - |
| 5. Presentation & Dashboard | 0/0 | Not started | - |
