@echo off
rem Keeps the Crystal Reports bridge running: starts it, and starts it again whenever it exits (it exits on its own
rem when a report hangs). The key is RPT_BRIDGE_KEY from backend\.env unless the variable is already set.
if "%RPT_BRIDGE_KEY%"=="" (
  for /f "usebackq tokens=1,* delims==" %%a in ("%~dp0..\..\.env") do if /i "%%a"=="RPT_BRIDGE_KEY" set "RPT_BRIDGE_KEY=%%b"
)
if "%RPT_BRIDGE_KEY%"=="" ( echo Set RPT_BRIDGE_KEY in backend\.env first & exit /b 2 )
cd /d "%~dp0bin"
:again
.\RptBridge.exe 8765
ping -n 3 127.0.0.1 >nul
goto again
