@echo off
for /f "TOKENS=1" %%a in ('wmic PROCESS where "Name='Virtual Desktop.exe'" get ProcessID ^| findstr [0-9]') do set VirtualDesktopPID=%%a
echo Virtual Desktop PID: %VirtualDesktopPID%
for /R %%f in (VirtualDesktopTimecodeServer_*.dll) do set DllFileName=%%f
echo Virtual Desktop Timecode Extension: %DllFileName%
cd C:\Program Files (x86)\Snoop
Snoop.InjectorLauncher.x64.exe --targetPID %VirtualDesktopPID% --assembly "%DllFileName%" --className VirtualDesktopTimecodeServer.TimecodeDriver --methodName StartTimecodeServer --settingsFile ""
timeout 5
