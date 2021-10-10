#!/bin/bash

dotnet ef migrations add --context SqliteServerDbContext -o Migrations/Sqlite $1
dotnet ef migrations add --context PostgresServerDbContext -o Migrations/Postgres $1
