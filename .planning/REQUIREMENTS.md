# Requirements: SchoolTimeCalc

**Defined:** 2026-04-14
**Core Value:** Accurately and automatically calculating the exact remaining time (days, hours, and lessons) left in the school year, removing the manual effort of counting days and accounting for holidays or schedule changes.

## v1 Requirements

### Data Sync

- [ ] **SYNC-01**: User can authenticate with their school's WebUntis instance.
- [ ] **SYNC-02**: System automatically fetches and securely caches the user's base timetable.
- [ ] **SYNC-03**: System fetches Austrian public bank holidays dynamically via an API (e.g., data.gv.at or Nager.Date).
- [ ] **SYNC-04**: System fetches Austrian state-specific school holidays dynamically (e.g., Semesterferien).

### Calculations

- [ ] **CALC-01**: System calculates the total number of school days remaining in the academic year.
- [ ] **CALC-02**: System calculates the total number of school hours/lessons remaining in the academic year.
- [ ] **CALC-03**: System calculates the remaining number of lessons broken down on a per-subject basis.
- [ ] **CALC-04**: System correctly factors out weekends, bank holidays, and school holidays from all calculations.

### User Interface

- [ ] **UI-01**: Dashboard displays high-level macro progress (total days and hours remaining).
- [ ] **UI-02**: Detailed view displays remaining lessons on a per-subject basis.
- [ ] **UI-03**: Interactive calendar view visualizes the user's timetable overlaid with fetched holidays.
- [ ] **UI-04**: Application is mobile-responsive and accessible from standard web browsers.

### Settings

- [ ] **SET-01**: User can select their specific Austrian Bundesland (State) to ensure accurate school holiday calculations.

## v2 Requirements

### Data Sync

- **SYNC-05**: System fetches daily schedule changes (substitutions, cancellations) and updates calculations in real-time.

### Settings

- **SET-02**: Comprehensive user profile and preference management (e.g., theme, notification preferences).

## Out of Scope

| Feature | Reason |
|---------|--------|
| Native Mobile App | Focused on a web application for initial accessibility and ease of deployment. |
| Manual Timetable Entry | Defeats the core value of the product (automation via WebUntis). |
| Non-Austrian Holidays | Focused specifically on the Austrian school system and API ecosystem for v1. |
| Social Features | Scope creep; focus is purely on personal time calculation. |

## Traceability

| Requirement | Phase | Status |
|-------------|-------|--------|
| SYNC-01 | Phase | Pending |
| SYNC-02 | Phase | Pending |
| SYNC-03 | Phase | Pending |
| SYNC-04 | Phase | Pending |
| CALC-01 | Phase | Pending |
| CALC-02 | Phase | Pending |
| CALC-03 | Phase | Pending |
| CALC-04 | Phase | Pending |
| UI-01 | Phase | Pending |
| UI-02 | Phase | Pending |
| UI-03 | Phase | Pending |
| UI-04 | Phase | Pending |
| SET-01 | Phase | Pending |

**Coverage:**
- v1 requirements: 13 total
- Mapped to phases: 0
- Unmapped: 13 ⚠️

---
*Requirements defined: 2026-04-14*
*Last updated: 2026-04-14 after initial definition*
