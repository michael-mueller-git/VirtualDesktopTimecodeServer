# Virtual Desktop Timecode Server

Timecode Server for [Virtual Desktop Wired Version](www.vrdesktop.net). This Extension provide the same API as the [Whirligig](http://www.whirligig.xyz/) Timecode Server.

## Dependencies

- [Virtual Desktop Wired Version](www.vrdesktop.net) (Testet with Version 1.18.11)
- [Snoop v4.0.1](https://github.com/snoopwpf/snoopwpf/releases/tag/v4.0.1)
- [Net Framework 4.7.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net471)

## Setup

1. Download an Install [Snoop v4.0.1](https://github.com/snoopwpf/snoopwpf/releases/tag/v4.0.1)
2. Download and Install [Net Framework 4.7.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net471)
3. Download latest `inject_vx.x.x.bat` and `VirtualDesktopTimecodeServer_vx.x.x.dll` from GitHub [release tab](https://github.com/michael-mueller-git/VirtualDesktopTimecodeServer/releases/latest).
4. Store both file in the same Directory!

## Usage

1. Start Virtual Desktop.
2. Use `./inject_vx.x.x.bat` to inject the Timecode Server Extension to Virtual Desktop.
3. Open an Video with Virtual Desktop that contains an Funscript in the same directory.
4. Connect Your Funscript Player and select the Whirligig API.
