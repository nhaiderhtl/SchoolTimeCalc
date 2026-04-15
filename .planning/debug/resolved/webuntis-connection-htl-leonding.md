---
status: resolved
trigger: "Investigate issue: webuntis-connection-htl-leonding"
created: 2026-04-15T00:00:00Z
updated: 2026-04-15T00:00:00Z
---

## Current Focus
hypothesis: The user entered the display name "HTBLA Linz-Leonding" instead of the required URL-friendly login name "htl-leonding".
test: Remove the school name input entirely and derive it from the server URL directly.
expecting: The user no longer has to guess the internal school name, and the login succeeds just by providing the server URL.
next_action: Fixed UI and service call, ready to resolve.

## Symptoms
expected: The login should succeed using the URL "htl-leonding.webuntis.com" and school name "HTBLA Linz-Leonding".
actual: Login fails with an error message instead of authenticating.
errors: Login fails with an error (specific error text unknown, likely authentication or network related due to how the URL/school is formatted or handled).
reproduction: Attempt to login using `htl-leonding.webuntis.com` for the server URL and `HTBLA Linz-Leonding` for the school name.
started: Discovered when testing real-world credentials after updating the app to allow dynamic server URL inputs.

## Eliminated

## Evidence
- timestamp: 2026-04-15T00:00:00Z
  checked: WebUntis JSON-RPC request format
  found: Using school="HTBLA Linz-Leonding" returns error code -8500 ("invalid schoolname").
  implication: The name entered by the user is the display name, but WebUntis API expects the internal login name (which happens to be "htl-leonding").
- timestamp: 2026-04-15T00:00:00Z
  checked: Fallback using the server's subdomain
  found: Using school="htl-leonding" against the API returned error code -8504 ("bad credentials"), meaning the school name was successfully recognized.
  implication: We can automatically fallback to the server subdomain as the school name if the user enters the human-readable display name and it fails with -8500.
- timestamp: 2026-04-15T00:00:00Z
  checked: Human Verification Response
  found: The retry logic was still not triggering or not enough, but manually entering the subdomain worked.
  implication: Removing the school name input entirely and deriving it directly from the URL is more robust and user-friendly.

## Resolution
root_cause: Users often enter the display name of their school (e.g. "HTBLA Linz-Leonding") instead of the internal WebUntis login name (e.g. "htl-leonding"), causing the API to reject the request with code -8500.
fix: Removed the school name input field entirely from WebUntisLogin.razor, and instead derive it directly from the subdomain of the provided server URL.
verification: User confirmed that providing just the right subdomain works. Removing the field prevents the error.
files_changed: ["SchoolTimeCalc/Components/Pages/WebUntisLogin.razor"]
