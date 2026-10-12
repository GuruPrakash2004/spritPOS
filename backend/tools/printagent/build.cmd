@echo off
rem Builds VlqmsPrintAgent.exe with the .NET Framework compiler that comes with Windows (no SDK needed).
setlocal
set CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe
if not exist "%~dp0bin" mkdir "%~dp0bin"
"%CSC%" /nologo /target:winexe /optimize+ /out:"%~dp0bin\VlqmsPrintAgent.exe" /r:System.Drawing.dll /r:System.Web.Extensions.dll "%~dp0Program.cs"
if errorlevel 1 (echo Build failed. & exit /b 1)
echo Built %~dp0bin\VlqmsPrintAgent.exe
