# Virtual Desktop Timecode Server

Timecode Server for [Virtual Desktop Wired Version](www.vrdesktop.net). This Extension provide the same API as the [Whirligig](http://www.whirligig.xyz/) Timecode Server.

## Dependencies

### Binary

- [Virtual Desktop Wired Version](www.vrdesktop.net) (testet with Version 1.18.11)
- [Snoop v4.0.1](https://github.com/snoopwpf/snoopwpf/releases/tag/v4.0.1)
- [Net Framework 4.7.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net471)

### Build from Source

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [Net Framework 4.7.1 Dev Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net471)

### Usage

1. Build the Timecode Server Extension.
2. Start Virtual Desktop.
3. Use `./inject.bat` to inject the Extension to Virtual Desktop.
4. Open an Video with Virtual Desktop that contains an Funscript in the same directory.
5. Connect Your Funscript Player and select the Whirligig API.
