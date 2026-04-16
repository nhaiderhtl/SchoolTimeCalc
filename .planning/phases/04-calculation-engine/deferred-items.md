# Deferred Items

## Pre-existing Test Failure
- **File**: `SchoolTimeCalc.Tests/HolidaySyncServiceTests.cs`
- **Test**: `SyncHolidaysAsync_SavesHolidaysToDatabase`
- **Issue**: `Assert.Contains() Failure: Filter not matched in collection. Collection only contains Halloween, missing National Holiday and School Holiday.`
- **Action**: Deferred per deviation rules, as it is unrelated to `CalculationService`.