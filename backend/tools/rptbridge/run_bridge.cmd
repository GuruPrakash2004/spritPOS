@echo off
rem Keeps the Crystal Reports bridge running: starts it, and starts it again whenever it exits (it exits on its own
rem when a report hangs). Set RPT_BRIDGE_KEY first (the same text as RPT_BRIDGE_KEY in backend\.env).
if "%RPT_BRIDGE_KEY%"=="" ( echo Set RPT_BRIDGE_KEY first & exit /b 2 )
cd /d "%~dp0bin"
:again
.\RptBridge.exe 8765
ping -n 3 127.0.0.1 >nul
goto again
