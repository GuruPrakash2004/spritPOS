@echo off
rem Read-only pre-flight check of this PC: database, indexes, Crystal Reports bridge, .rpt files, and two timed test prints.
cd /d "%~dp0.."
.venv\Scripts\python.exe tools\preflight.py %*
pause
