@echo off
rem Run on the development PC: builds the frontend if needed and writes one zip for the server (deploy\out\).
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Make-Package.ps1" %*
set RC=%ERRORLEVEL%
if "%~1"=="" pause
exit /b %RC%
