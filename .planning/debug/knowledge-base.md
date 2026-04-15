# GSD Debug Knowledge Base

Resolved debug sessions. Used by `gsd-debugger` to surface known-pattern hypotheses at the start of new investigations.

---

## postgres-connection-refused — App fails to start with Npgsql connection refused
- **Date:** 2026-04-15T00:00:00Z
- **Error patterns:** NpgsqlException, SocketException, Connection refused, database unavailability
- **Root cause:** No PostgreSQL instance was running locally and there was no docker-compose.yml file provided to start one for development.
- **Fix:** Created a docker-compose.yml file configured with the credentials expected by the application (postgres/postgres on port 5432) and started the database.
- **Files changed:** docker-compose.yml
---
