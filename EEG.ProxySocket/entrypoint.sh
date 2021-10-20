#!/bin/bash
set -e

cd /app/publish/
dotnet EEG.ProxySocket.WebHost.dll

exec "$@"
