@echo off
for /f "TOKENS=1" %%a in ('wmic PROCESS where "Name='Virtual Desktop.exe'" get ProcessID ^| findstr [0-9]') do set VirtualDesktopPID=%%a
echo Virtual Desktop PID: %VirtualDesktopPID%
set DllSourceDir=%CD%
cd C:\Program Files (x86)\Snoop
Snoop.InjectorLauncher.x64.exe --targetPID %VirtualDesktopPID% --assembly "%DllSourceDir%\bin\Debug\net471\VirtualDesktopTimecodeServer.dll" --className VirtualDesktopTimecodeServer.TimecodeDriver --methodName StartTimecodeServer --settingsFile "%DllSourceDir%\bin\Debug\net471\debug.log" -v
timeout 5