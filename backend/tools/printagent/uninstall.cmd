@echo off
rem Removes the vLQMS Print Agent from this Windows user.
taskkill /im VlqmsPrintAgent.exe /f >nul 2>&1
reg delete HKCU\Software\Microsoft\Windows\CurrentVersion\Run /v VlqmsPrintAgent /f >nul 2>&1
rmdir /s /q "%LOCALAPPDATA%\VlqmsPrintAgent" >nul 2>&1
echo Removed.
