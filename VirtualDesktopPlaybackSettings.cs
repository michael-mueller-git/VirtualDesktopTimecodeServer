using System;
using System.Reflection;
using System.Windows;
using MethodRedirect;

namespace VirtualDesktopTimecodeServer
{
    public sealed class VirtualDesktopPlaybackSettings
    {
         public static VirtualDesktopPlaybackSettings Instance 
        { 
            get 
            { 
                return lazy.Value; 
            } 
        }

        public double VideoPositionInSeconds
        {
            get
            {
                return _videoPositionInSeconds;
            }
        }

        public string VideoPositionInSecondsString
        {
            get
            {
                return String
                    .Format("{0:0.000}", _videoPositionInSeconds)
                    .Replace(",", ".");
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }

        public string VideoPath
        {
            get
            {
                return _videoPath;
            }
            set
            {
                _videoPath = value;
            }
        }

        public void Update()
        {
            var playbackSettingsInstance = GetPlaybackSettingsInstance();
            var videoPosition = (long)playbackSettingsInstance
                .GetType()
                .GetProperty("VideoPosition")
                .GetValue(playbackSettingsInstance);
            var videoPositionTimeSpan = new TimeSpan(videoPosition - videoPosition % 10000L);
            _videoPositionInSeconds = videoPositionTimeSpan.TotalSeconds;
            _isPlaying = (bool)playbackSettingsInstance
                .GetType()
                .GetProperty("IsPlaying")
                .GetValue(playbackSettingsInstance);
        }

        private VirtualDesktopPlaybackSettings()
        {
            var playbackSettinsType = Type.GetType("VirtualDesktop.Engine.PlaybackSettings,Virtual Desktop");
            // we have to use `1 to get the generic class type
            var settingsBaseType = Type.GetType("VirtualDesktop.Core.SettingsBase`1,Virtual Desktop");
            var settingsBasePlaybackSettingsType = settingsBaseType.MakeGenericType(playbackSettinsType);
            _settingsBasePlaybackSettingsDefaultProperty = settingsBasePlaybackSettingsType
                .GetProperty(
                    "Default",
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
                );
            _playVideo = Application.Current.MainWindow
                .GetType()
                .GetMethod(
                    "PlayVideo", 
                    BindingFlags.Instance | BindingFlags.Public
                );
            _playVideoReplacement = this
                .GetType()
                .GetMethod(
                    "PlayVideoReplacement", 
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
            InjectPlayVideoMethodRedirect();
        }

        private void PlayVideoReplacement(string path)
        {
            VirtualDesktopPlaybackSettings.Instance.VideoPath = path;
            VirtualDesktopPlaybackSettings.Instance.PlayVideoInvoker(path);
        }

        public void PlayVideoInvoker(string path)
        {
            _playVideoMethodRedirekt.Restore();
            _playVideo.Invoke(Application.Current.MainWindow, new object[] { path });
            InjectPlayVideoMethodRedirect();
        }

        private void InjectPlayVideoMethodRedirect()
        {
            _playVideoMethodRedirekt = _playVideo.RedirectTo(_playVideoReplacement);
        }

        private object GetPlaybackSettingsInstance()
        {
            return _settingsBasePlaybackSettingsDefaultProperty.GetValue(null, null);
        }

        private PropertyInfo _settingsBasePlaybackSettingsDefaultProperty;
        private double _videoPositionInSeconds = 0.0;
        private bool _isPlaying = false;
        private string _videoPath = String.Empty;
        private MethodInfo _playVideo;
        private MethodInfo _playVideoReplacement;
        private MethodOperation _playVideoMethodRedirekt;
        private static readonly Lazy<VirtualDesktopPlaybackSettings> lazy =
         new Lazy<VirtualDesktopPlaybackSettings>(() => new VirtualDesktopPlaybackSettings());

    }
}
