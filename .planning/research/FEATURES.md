# Feature Research

**Domain:** Student Academic Tracking / Timetable Applications
**Researched:** 2026-04-14
**Confidence:** HIGH

## Feature Landscape

### Table Stakes (Users Expect These)

Features users assume exist. Missing these = product feels incomplete.

| Feature | Why Expected | Complexity | Notes |
|---------|--------------|------------|-------|
| **Current Timetable View** | Students need to see their active schedule to trust the data. | LOW | Standard calendar/agenda view. |
| **Holiday/Break Accounting** | Time tracking is useless if it counts weekends or standard holidays as school days. | MEDIUM | Needs accurate regional data. |
| **Mobile-Friendly UI** | Students check school apps on their phones during the day. | MEDIUM | Responsive web design is mandatory. |
| **Secure Authentication** | Handling school data (WebUntis) requires trusted, secure login. | MEDIUM | OAuth or token-based integration with Untis. |
| **Basic Progress Metrics** | Users expect to see some form of "time left" (days/weeks). | LOW | Simple date math. |

### Differentiators (Competitive Advantage)

Features that set the product apart. Not required, but valuable.

| Feature | Value Proposition | Complexity | Notes |
|---------|-------------------|------------|-------|
| **Zero-Entry Sync (WebUntis)** | Eliminates the tedious setup of competitor apps (MyStudyLife, Notion). | MEDIUM | Core value prop. Requires robust API integration. |
| **Subject-Level Granularity** | Knowing exact *lessons* left per subject (e.g., "Only 12 math lessons left") provides unique motivation. | HIGH | Requires parsing timetable and mapping recurring schedules against holidays. |
| **Dynamic Austrian Holidays** | Live API integration means no manual entry of "Zwickeltage" (bridge days) or regional holidays. | LOW | Reliable data source required. |
| **Macro Year-at-a-Glance** | Most apps focus on the *day* or *week*. Focusing on the *entire year's* progress is unique. | MEDIUM | Visualizing a year's progress intuitively (progress bars, charts). |

### Anti-Features (Commonly Requested, Often Problematic)

Features that seem good but create problems.

| Feature | Why Requested | Why Problematic | Alternative |
|---------|---------------|-----------------|-------------|
| **Manual Timetable Entry** | Users without WebUntis want to use the app. | Breaks the "Zero-Entry" value prop; introduces massive UX overhead for a niche audience. | Strictly enforce WebUntis-only for v1. |
| **Daily Bell Countdown** | Students love watching a timer count down to the end of the current lesson/day. | Requires exact bell schedules which vary wildly and change often; distracts from macro progress. | Focus on macro progress (days/lessons left). |
| **Task/Homework Tracking** | Consolidating tools (calendar + to-do). | Massive scope creep. Competes poorly against established tools (Todoist, Notion). | Stick to time calculation; let other tools handle tasks. |
| **Social / Friends Sync** | "Compare progress with friends." | Privacy nightmare with school schedules; complex to build. | Keep it a personal utility tool. |

## Feature Dependencies

```
[WebUntis Authentication]
    └──requires──> [User Profile/Session]

[Timetable Data Fetching]
    └──requires──> [WebUntis Authentication]
                       └──requires──> [Data Caching Strategy]

[Subject-Level Lesson Calculation]
    └──requires──> [Timetable Data Fetching]
    └──requires──> [Austrian Holiday API Data]

[Macro Progress Dashboard]
    └──requires──> [Subject-Level Lesson Calculation]
```

### Dependency Notes

- **[Timetable Data Fetching] requires [WebUntis Authentication]:** Cannot fetch schedule without user credentials/token.
- **[Subject-Level Lesson Calculation] requires [Austrian Holiday API Data]:** Cannot calculate accurate remaining lessons without knowing which days are skipped due to holidays.
- **[Data Caching Strategy] enhances [Timetable Data Fetching]:** Crucial to avoid hitting Untis API limits and to improve app performance.

## MVP Definition

### Launch With (v1)

Minimum viable product — what's needed to validate the concept.

- [x] **WebUntis Authentication** — Required to get data.
- [x] **Austrian Holiday API Integration** — Required for accurate calculations.
- [x] **Total Days/Hours Remaining Calc** — The core macro value proposition.
- [x] **Basic Dashboard (Mobile-Web)** — For viewing the macro progress.

### Add After Validation (v1.x)

Features to add once core is working.

- [ ] **Subject-Level Remaining Lessons** — High value, but higher complexity math. Wait until basic days/hours calc is validated.
- [ ] **Calendar/Agenda View** — Helpful context, but secondary to the core calculations.
- [ ] **Progress Sharing (Image Export)** — Let students share a screenshot of their progress to social media (viral loop).

### Future Consideration (v2+)

Features to defer until product-market fit is established.

- [ ] **Custom Excluded Days** — Letting users mark days they will be absent (sick leave, appointments) to adjust personal calculations.
- [ ] **Push Notifications** — "You are halfway through the year!" (Requires PWA or native app).
- [ ] **Support for other systems** — Untis regions outside Austria, or platforms like Sokrates.

## Feature Prioritization Matrix

| Feature | User Value | Implementation Cost | Priority |
|---------|------------|---------------------|----------|
| WebUntis Login & Data Fetch | HIGH | MEDIUM | P1 |
| Holiday API Integration | HIGH | LOW | P1 |
| Global Progress Calc (Days/Hours) | HIGH | LOW | P1 |
| Mobile-Responsive Dashboard | HIGH | MEDIUM | P1 |
| Subject-Specific Lesson Calc | HIGH | HIGH | P2 |
| Calendar/Agenda View | MEDIUM | MEDIUM | P2 |
| Custom Absences | LOW | MEDIUM | P3 |
| Export/Share Progress Image | MEDIUM | LOW | P3 |

**Priority key:**
- P1: Must have for launch
- P2: Should have, add when possible
- P3: Nice to have, future consideration

## Competitor Feature Analysis

| Feature | MyStudyLife | School / Notion Templates | Our Approach (SchoolTimeCalc) |
|---------|--------------|---------------------------|-------------------------------|
| **Data Entry** | Manual | Manual | **100% Automated** (WebUntis) |
| **Progress Tracking**| Basic (Current term dates) | Requires manual formulas | **Deep Calculation** (Exact lessons left per subject) |
| **Holiday Handling** | Manual entry | Manual entry | **Automated API** (Austrian Holidays) |
| **Focus** | Daily tasks/schedule | Highly customizable workspaces | **Macro motivation** (Yearly progress focus) |

## Sources

- `.planning/PROJECT.md`
- Industry standard student planning app patterns (MyStudyLife, Untis Mobile, PowerSchool).
- Austrian educational software ecosystem constraints.

---
*Feature research for: Student Academic Tracking (SchoolTimeCalc)*
*Researched: 2026-04-14*