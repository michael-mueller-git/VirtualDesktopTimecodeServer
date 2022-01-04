namespace VirtualDesktopTimecodeServer
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    public class TimecodeDriver
    {
        public static int StartTimecodeServer(string debugLogFile)
        {

            if (Application.Current == null)
            {
                return 1;
            }

            Application.Current.Dispatcher.Invoke(async () => await RunTimecodeServer(debugLogFile));

            return 0;
        }

        private static async Task RunTimecodeServer(string debugLogFile)
        {
            const int UPDATE_LOOP_TIME_IN_MILLLISECONDS = 50;
            var virtualDesktopPlaybackSettings = VirtualDesktopPlaybackSettings.Instance;
            //var virtualDesktopUiSettings = VirtualDesktopUiSettings.Instance;
            var timecodeServer = VirtualDesktopTimecodeServer.Instance;

            timecodeServer.SetServerLoopTimeInMilliseconds(UPDATE_LOOP_TIME_IN_MILLLISECONDS);

            while (Application.Current.MainWindow is not null)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(UPDATE_LOOP_TIME_IN_MILLLISECONDS));

                virtualDesktopPlaybackSettings.Update();
                //virtualDesktopUiSettings.Update();

                timecodeServer.SetVideoPath(virtualDesktopPlaybackSettings.VideoPath);
                timecodeServer.SetIsPlaying(virtualDesktopPlaybackSettings.IsPlaying);
                timecodeServer.SetVideoPositionInSeconds(virtualDesktopPlaybackSettings.VideoPositionInSecondsString);
            }

            await Task.CompletedTask;
        }
    }
}
