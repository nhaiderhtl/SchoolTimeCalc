---
status: resolved
trigger: "[verbatim user input]"
created: 2026-04-16T00:00:00Z
updated: 2026-04-16T00:00:00Z
---

## Current Focus

hypothesis: INationalHolidayService and ISchoolHolidayService were registered as their concrete types instead of their interfaces.
test: Change `builder.Services.AddScoped<NationalHolidayService>();` to `builder.Services.AddScoped<INationalHolidayService, NationalHolidayService>();` in Program.cs.
expecting: The application should build and run successfully, resolving the DI error.
next_action: Awaiting human verification to confirm the build error is resolved.

## Symptoms

expected: The application builds and starts successfully.
actual: The build fails with System.AggregateException: Some services are not able to be constructed.
errors: Unhandled exception. System.AggregateException: Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: SchoolTimeCalc.Services.IHolidaySyncService Lifetime: Scoped ImplementationType: SchoolTimeCalc.Services.WebUntisHolidaySyncService': Unable to resolve service for type 'SchoolTimeCalc.Services.INationalHolidayService' while attempting to activate 'SchoolTimeCalc.Services.WebUntisHolidaySyncService'.) ---> System.InvalidOperationException: Error while validating the service descriptor 'ServiceType: SchoolTimeCalc.Services.IHolidaySyncService Lifetime: Scoped ImplementationType: SchoolTimeCalc.Services.WebUntisHolidaySyncService': Unable to resolve service for type 'SchoolTimeCalc.Services.INationalHolidayService' while attempting to activate 'SchoolTimeCalc.Services.WebUntisHolidaySyncService'. ---> System.InvalidOperationException: Unable to resolve service for type 'SchoolTimeCalc.Services.INationalHolidayService' while attempting to activate 'SchoolTimeCalc.Services.WebUntisHolidaySyncService'.
reproduction: Run `dotnet build` or `dotnet run`.
started: Started after Phase 03 wave 2 where orphaned holiday services were wired up.

## Eliminated

## Evidence

- timestamp: 2026-04-16T00:00:00Z
  checked: `SchoolTimeCalc/Program.cs`
  found: `NationalHolidayService` and `SchoolHolidayService` were registered as concrete types `builder.Services.AddScoped<NationalHolidayService>();` rather than interfaces.
  implication: `WebUntisHolidaySyncService` expects interfaces `INationalHolidayService` and `ISchoolHolidayService`, which fails DI resolution because only concrete types were registered.

## Resolution

root_cause: `NationalHolidayService` and `SchoolHolidayService` were registered as concrete types in `Program.cs` instead of their respective interfaces (`INationalHolidayService` and `ISchoolHolidayService`). `WebUntisHolidaySyncService` expects the interfaces in its constructor.
fix: Updated `Program.cs` to register the interfaces instead: `builder.Services.AddScoped<INationalHolidayService, NationalHolidayService>();` and `builder.Services.AddScoped<ISchoolHolidayService, SchoolHolidayService>();`.
verification: Awaiting human verification.
files_changed: [SchoolTimeCalc/Program.cs]