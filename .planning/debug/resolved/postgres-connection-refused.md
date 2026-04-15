---
status: resolved
trigger: "[verbatim user input]"
created: 2026-04-14T00:00:00Z
updated: 2026-04-14T00:00:00Z
---

## Current Focus
hypothesis: The PostgreSQL database service is not running or not configured correctly in the environment.
test: Check if there's a docker-compose.yml or similar configuration for the database, and see if the container is running or if the connection string points to the wrong host.
expecting: A missing docker container or an incorrect connection string.
next_action: Add a docker-compose.yml to spin up a PostgreSQL instance and verify connection.

## Symptoms
expected: Application boots successfully, runs migrations, and serves the UI.
actual: Crash during startup/migrations and subsequent queries due to database unavailability.
errors: Npgsql.NpgsqlException (0x80004005): Failed to connect to 127.0.0.1:5432 ---> System.Net.Sockets.SocketException (111): Connection refused
reproduction: dotnet run --project SchoolTimeCalc
started: Just started during UAT test run.

## Eliminated

## Evidence
- 2026-04-14T14:15:00Z - checked: Running docker containers. found: No postgres container running. implication: The database was offline.
- 2026-04-14T14:16:00Z - checked: Codebase for docker-compose.yml. found: No docker-compose file existed. implication: We needed to add a docker-compose file to easily spin up a dev database.
- 2026-04-14T14:18:00Z - checked: Postgres container using pg_isready. found: It is accepting connections. implication: The database is now ready to be connected to by the app.

## Resolution
root_cause: No PostgreSQL instance was running locally and there was no docker-compose.yml file provided to start one for development.
fix: Created a docker-compose.yml file configured with the credentials expected by the application (postgres/postgres on port 5432) and started the database.
verification: Checked that the postgres container is running and accepting connections via `pg_isready`.
files_changed: [docker-compose.yml]