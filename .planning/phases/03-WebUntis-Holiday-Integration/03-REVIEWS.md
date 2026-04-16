# Phase 03 Review: WebUntis Holiday Integration

## Consensus Summary

The review indicates that Phase 03 is incomplete and should not be approved for transition yet. Several critical issues must be addressed: missing WebUntis credentials preventing background sync, incorrect Austrian state subdivision mapping, silent failure handling in the UI, and a lack of integration between the fetched holidays and the core school-day calculations. Furthermore, test coverage consists of placeholders and needs implementation.

## Reviewer: Codex

### Findings

1. `HolidaySyncBackgroundService` cannot perform a real WebUntis sync because the required WebUntis credentials are never persisted, and it uses the wrong username field when it tries. `WebUntisService.AuthenticateAndSyncAsync` saves only `SchoolName` and timetable JSON; it never stores `Server`, `EncryptedPassword`, or the WebUntis username. The background service then requires `Server` and `EncryptedPassword` to exist and passes `data.ApplicationUser.Username` to WebUntis, which is the app user (`testuser`), not the WebUntis login. This breaks `03-04` in practice.

2. The implementation does not meet the phase goal “fetch holiday data from WebUntis API alongside timetables.” Timetable sync and holiday sync are separate flows. `WebUntisService.AuthenticateAndSyncAsync` fetches timetable-related data only and never invokes holiday sync. Holidays are only fetched from the Settings page via a separate manual action or by the background service.

3. `SYNC-04` is not correctly implemented. `SchoolHolidayService` is wired up, but its subdivision mapping is wrong: it builds `subdivisionCode` from the first character of the `bundesland` string (`bundesland.Substring(0, 1)`), so `"Wien"` becomes `AT-W` and `"Niederösterreich"` becomes `AT-N`, which is not a valid Austrian state code for this API. The caller also mixes incompatible formats: fallback is `"AT-9"` while user settings store names like `"Wien"`. The inline comment explicitly says the hardcoded Wien fallback still needs proper handling.

4. The Settings UI can report a successful manual sync even when the sync failed. `WebUntisHolidaySyncService.SyncHolidaysAsync` logs and returns on auth failure or holiday fetch failure instead of throwing. The Settings page treats any completed call as success and shows `"Holidays synced successfully!"`. That creates false-positive status reporting.

5. Holiday data is stored, but I found no integration where it is actually subtracted from school-day calculations. The home page only displays connection status and cached timetable presence; it does not use `Holidays` at all. A repo-wide search only finds holiday usage in storage/sync/UI status code, not in calculation logic. That means success criterion 2 is not satisfied.

6. Test coverage does not verify the real holiday sync path. `HolidaySyncServiceTests` is effectively a placeholder ending in `Assert.True(true)` and does not instantiate or exercise the service behavior it claims to verify. The Settings tests only check rendered text and a button click path, not actual persistence or failure handling.

### Verification report

`Holiday` model and DB integration:
Present. `Holiday` exists with `Name`, `StartDate`, `EndDate`, `SchoolId`. `ApplicationDbContext` exposes `DbSet<Holiday>` and creates a unique index on `(SchoolId, StartDate, EndDate)`. Migrations for holidays and `LastHolidaySync` are present.

`WebUntisHolidaySyncService` fetch/persist behavior:
Partially present. It authenticates with WebUntis, calls `getHolidays`, parses DTOs, and writes `Holiday` rows plus `LastHolidaySync`. But it also pulls national and school holidays from other sources, so the implementation is no longer purely “WebUntis holiday integration.”

Settings UI:
Present. It shows `LastHolidaySync`, cached count, and a manual `Sync Holidays Now` button. But its status reporting is not trustworthy because service failures are swallowed.

Background sync hardcoded credential issue:
The hardcoded stubs are gone, but the replacement is incomplete. It dynamically loads from DB, yet the login flow never stores the needed fields, and the code uses the application username instead of the WebUntis username. So this requirement is not functionally complete.

`NationalHolidayService` and `SchoolHolidayService` wiring:
They are registered in DI and consumed by `WebUntisHolidaySyncService`. `NationalHolidayService` is real, not stubbed. `SchoolHolidayService` is also real, but its state-code mapping is incorrect, so `SYNC-04` is not reliable.

SYNC requirements:
`SYNC-03`: Mostly implemented via `NationalHolidayService` using `Nager.Date`, not WebUntis.
`SYNC-04`: Not correctly implemented due to bad Bundesland-to-subdivision mapping and inconsistent state formats.
Phase success criteria:
1. Not met as written, because holidays are not fetched alongside timetable sync.
2. Not met, because cached holidays are not used in school-day calculations.

### Conclusion

Phase 03 should not be approved for transition yet.

Minimum fixes before approval:
1. Persist actual WebUntis sync credentials or a usable refresh mechanism, including server and WebUntis username, and use those for background sync.
2. Trigger holiday sync from the main WebUntis timetable sync flow if “alongside timetables” remains the acceptance criterion.
3. Replace the broken Bundesland mapping with a correct Austrian-state code mapping.
4. Make sync failures observable to the UI instead of silently returning success.
5. Integrate cached holidays into the calculation path that determines remaining school days.
6. Replace placeholder tests with real service/integration tests.
