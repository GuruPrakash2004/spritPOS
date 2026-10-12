@echo off
rem Installs the vLQMS Print Agent for the Windows user who runs this (no administrator needed): copies it to
rem %LOCALAPPDATA%\VlqmsPrintAgent, writes the web address that may use it, starts it at every sign-in, and starts it now.
rem Run it ON THE COUNTER PC whose printers should be usable, signed in as the cashier.
setlocal
set SRC=%~dp0bin\VlqmsPrintAgent.exe
if not exist "%SRC%" (echo VlqmsPrintAgent.exe not found. Run build.cmd first, or copy the bin folder next to this file. & exit /b 1)
set DEST=%LOCALAPPDATA%\VlqmsPrintAgent
echo.
echo Type the address you open vLQMS at in the browser, WITHOUT a trailing slash and without the page path.
echo Examples:  http://192.168.1.10   or   http://192.168.1.10:5173   or   https://shop.example.com
echo (Several addresses: run this again, they are added.)
set /p ORIGIN=Address:
if "%ORIGIN%"=="" (echo No address given. & exit /b 1)
taskkill /im VlqmsPrintAgent.exe /f >nul 2>&1
if not exist "%DEST%" mkdir "%DEST%"
copy /y "%SRC%" "%DEST%\VlqmsPrintAgent.exe" >nul
echo %ORIGIN%>>"%DEST%\printagent.txt"
reg add HKCU\Software\Microsoft\Windows\CurrentVersion\Run /v VlqmsPrintAgent /t REG_SZ /d "\"%DEST%\VlqmsPrintAgent.exe\"" /f >nul
start "" "%DEST%\VlqmsPrintAgent.exe"
echo.
echo Installed in %DEST%. It starts by itself at every sign-in.
echo Check it:  open http://127.0.0.1:8766/health in this PC's browser.
