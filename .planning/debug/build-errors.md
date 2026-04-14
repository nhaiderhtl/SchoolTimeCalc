---
status: awaiting_human_verify
trigger: "building the project led to multiple errors, read the terminal output and fix them"
created: 2026-04-14T14:15:00Z
updated: 2026-04-14T14:20:00Z
---

## Current Focus
hypothesis: "WebUntisService was applying the null-conditional operator '?.ValueKind' on a JsonElement, which is a struct and cannot be null, causing CS0023 compilation errors."
test: "Replace '?.' with a null check on the parent object."
expecting: "CS0023 error goes away and project compiles."
next_action: "Await human verification to confirm the build succeeds for the user."

## Symptoms
expected: Project builds without errors.
actual: Compilation fails with "error CS0023: Operator '?' cannot be applied to operand of type 'JsonElement'" on lines 80-83 in WebUntisService.cs.
errors: "error CS0023: Operator '?' cannot be applied to operand of type 'JsonElement'"
reproduction: "Run `dotnet build`."
started: Always broken during build.

## Eliminated

## Evidence

- timestamp: 2026-04-14T14:15:00Z
  checked: "Terminal output of dotnet build"
  found: "Multiple CS0023 errors in WebUntisService.cs lines 80, 81, 82, 83 involving the '?' operator on JsonElement."
  implication: "The code attempts to use the null-conditional operator on a struct (JsonElement) which is invalid in C#."

- timestamp: 2026-04-14T14:18:00Z
  checked: "Code structure in WebUntisService.cs"
  found: "Result of UntisRpcResponse is an unconstrained generic T, which resolves to a value type (JsonElement) without Nullable<> wrapper. The operator ?. was used illegally."
  implication: "Replacing the null-conditional operator with explicit null checks and ValueKind checks will satisfy the compiler."

- timestamp: 2026-04-14T14:19:00Z
  checked: "Compilation output after applying fix"
  found: "Build succeeded with 0 errors and 0 warnings."
  implication: "The issue is resolved from the compiler's perspective."

## Resolution
root_cause: "The null-conditional operator '?' was used on a 'JsonElement' struct property ('ValueKind'), which is illegal in C# because structs cannot be null. This was triggered because the generic response 'Result' property is resolved as the unboxed struct 'JsonElement' rather than 'Nullable<JsonElement>'."
fix: "Changed 'subjRes?.Result?.ValueKind' to 'subjRes != null && subjRes.Result.ValueKind'."
verification: "Ran 'dotnet build' locally and confirmed the project builds successfully with 0 errors and 0 warnings."
files_changed: ["SchoolTimeCalc/Services/WebUntisService.cs"]