#!/bin/bash
set -e

cd /app/publish/
dotnet EEG.Archive.WebHost.dll

exec "$@"
