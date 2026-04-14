---
status: testing
phase: 02-webuntis-integration
source: 02-01-SUMMARY.md, 02-02-SUMMARY.md, 02-03-SUMMARY.md
started: 2026-04-14T12:00:00Z
updated: 2026-04-14T12:00:00Z
---

## Current Test
<!-- OVERWRITE each test - shows where we are -->

number: 1
name: Cold Start Smoke Test
expected: |
  Kill any running server/service. Clear ephemeral state (temp DBs, caches, lock files). Start the application from scratch. Server boots without errors, any seed/migration completes, and a primary query (health check, homepage load, or basic API call) returns live data.
awaiting: user response

## Tests

### 1. Cold Start Smoke Test
expected: Kill any running server/service. Clear ephemeral state (temp DBs, caches, lock files). Start the application from scratch. Server boots without errors, any seed/migration completes, and a primary query (health check, homepage load, or basic API call) returns live data.
result: [pending]

### 2. WebUntis Login Form
expected: Navigate to WebUntis login page. Form displays inputs for server, school, username, and password. Submitting shows a loading state and successfully authenticates.
result: [pending]

### 3. Home Page Integration
expected: Home page shows a Call-to-Action to connect WebUntis if not linked. Once linked, it shows a success state or sync status for the current user.
result: [pending]

## Summary

total: 3
passed: 0
issues: 0
pending: 3
skipped: 0
blocked: 0

## Gaps
