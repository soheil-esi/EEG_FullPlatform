#!/bin/bash
set -e

cd /app/publish/
dotnet EEG.Connector.Web.dll

exec "$@"
