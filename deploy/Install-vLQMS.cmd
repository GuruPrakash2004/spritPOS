@echo off
rem Double-click launcher: runs the installer wizard (it asks for Administrator rights itself).
rem Any arguments are passed on, for example:  Install-vLQMS.cmd -Mode Install -AppPath C:\vLQMS -Unattended
title e-vLQMS installer
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Install-vLQMS.ps1" %*
set RC=%ERRORLEVEL%
if not %RC%==0 if not "%~1"=="" exit /b %RC%
if not %RC%==0 (
  echo.
  echo The installer ended with an error. Press any key to close.
  pause >nul
)
exit /b %RC%
