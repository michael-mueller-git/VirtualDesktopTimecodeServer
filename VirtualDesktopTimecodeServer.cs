using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopTimecodeServer
{
    public sealed class VirtualDesktopTimecodeServer
    {
        public static VirtualDesktopTimecodeServer Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void SetServerLoopTimeInMilliseconds(int value)
        {
            _serverLoopTimeInMilliseconds = value;
        }

        public void SetVideoPath(string videoPath)
        {
            _videoPath = videoPath;
        }

        public void SetIsPlaying(bool isPlaying)
        {
            if (_isPlaying != isPlaying)
            {
                _isPlaying = isPlaying;
                _isPlayingChanged = true;
            }
        }

        public void SetVideoPositionInSeconds(string videoPosition)
        {
            _videoPositionInSecondsString = videoPosition;
        }

        private VirtualDesktopTimecodeServer()
        {
            StartServer();
        }

        private void StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 2000);
            _server.Start();
            AcceptConnection();

        }

        private void AcceptConnection()
        {
            _server.BeginAcceptTcpClient(HandleConnection, _server);
        }

        private void HandleConnection(IAsyncResult result)
        {
            try
            {
                AcceptConnection();
                TcpClient client = _server.EndAcceptTcpClient(result);
                NetworkStream ns = client.GetStream();
                String lastTimecode = "";
                String lastVideoFile = "";

                while (true)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(_serverLoopTimeInMilliseconds));

                    if (_videoPath != lastVideoFile)
                    {
                        lastVideoFile = _videoPath;
                        byte[] videoFile = Encoding.Default.GetBytes("C" + _videoPath + "\n");
                        ns.Write(videoFile, 0, videoFile.Length);
                    }

                    if (_isPlayingChanged)
                    {
                        _isPlayingChanged = false;
                        if (!_isPlayingChanged)
                        {
                            byte[] stop = Encoding.Default.GetBytes("S\n");
                            ns.Write(stop, 0, stop.Length);
                        }
                    }

                    if (_isPlaying)
                    {
                        if (lastTimecode != _videoPositionInSecondsString)
                        {
                            lastTimecode = _videoPositionInSecondsString;
                            byte[] timecode = Encoding.Default.GetBytes("P" + _videoPositionInSecondsString + "\n");
                            ns.Write(timecode, 0, timecode.Length);
                        }

                    }
                }
            }
            catch (Exception) { };
        }

        private TcpListener _server;
        private string _videoPath = String.Empty;
        private bool _isPlayingChanged = true;
        private bool _isPlaying = false;
        private string _videoPositionInSecondsString = "0.0";
        private int _serverLoopTimeInMilliseconds = 20;
        private static readonly Lazy<VirtualDesktopTimecodeServer> lazy =
         new Lazy<VirtualDesktopTimecodeServer>(() => new VirtualDesktopTimecodeServer());
    }
}
