using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualDesktopTimecodeServer
{
    public sealed class VirtualDesktopUiSettings
    {
        public static VirtualDesktopUiSettings Instance
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

        public double PlayerSpeed
        {
            get
            {
                return _playerSpeed;
            }
        }

        public void Update()
        {
            if (_videoPositionTextBlock is null)
            {
                _videoPositionTextBlock = GetElementByTreePath<TextBlock>(
                    Application.Current.MainWindow,
                    "Border.AdornerDecorator.ContentPresenter.Grid.Border.DockPanel.Grid.VideoPlayerPanel.Border.ContentPresenter.Grid.DockPanel.StackPanel.Grid.TextBlock",
                    o => o.Name == ""
                );
            }

            if (_playButton is null)
            {
                _playButton = GetElementByTreePath<Button>(
                    Application.Current.MainWindow,
                    "Border.AdornerDecorator.ContentPresenter.Grid.Border.DockPanel.Grid.VideoPlayerPanel.Border.ContentPresenter.Grid.DockPanel.StackPanel.Grid.StackPanel.Grid.Button",
                    o => o.Name == "playButton"
                );
            }

            if (_playerSpeedTextBlock is null)
            {
                _playerSpeedTextBlock = GetElementByTreePath<TextBlock>(
                    Application.Current.MainWindow,
                    "Border.AdornerDecorator.ContentPresenter.Grid.Border.DockPanel.Grid.VideoPlayerPanel.Border.ContentPresenter.Grid.DockPanel.StackPanel.Grid.StackPanel.DropDownButton.Grid.ContentControl.ContentPresenter.ContentPresenter.TextBlock",
                    o => true
                );
            }

            if (_playerSpeedTextBlock is not null)
            {
                _playerSpeed = Convert.ToDouble(
                        _playerSpeedTextBlock
                            .Text
                            .ToLower()
                            .Replace("x", "")
                        );
            }

            if (_videoPositionTextBlock is not null)
            {
                SetVideoPosition(_videoPositionTextBlock.Text);
            }

            if (_playButton is not null)
            {
                _isPlaying = !_playButton.IsEnabled;
            }
        }

        private VirtualDesktopUiSettings()
        {
            Update();
        }

        private void SetVideoPosition(string videoPosition)
        {
            if (_lastVideoPositionString != videoPosition)
            {
                _lastVideoPositionString = videoPosition;
                _breakEvenTime = DateTime.Now;
            }

            // TODO: implement for playersped != 1.0
            double interpolatedTimestamp = 0;
            if (videoPosition.Split(':').Length == 3)
            {
                int hours = Convert.ToInt32(videoPosition.Split(':')[0]);
                int minutes = Convert.ToInt32(videoPosition.Split(':')[1]);
                int seconds = Convert.ToInt32(videoPosition.Split(':')[2]);
                interpolatedTimestamp = (hours * 3600.0 + minutes * 60.0 + seconds) * 1000.0;
            }
            else if (videoPosition.Split(':').Length == 2)
            {
                int minutes = Convert.ToInt32(videoPosition.Split(':')[0]);
                int seconds = Convert.ToInt32(videoPosition.Split(':')[1]);
                interpolatedTimestamp = (minutes * 60.0 + seconds) * 1000.0;
            }
            else
            {
                return;
            }

            interpolatedTimestamp += ((TimeSpan)(DateTime.Now - _breakEvenTime)).TotalMilliseconds;
            _videoPositionInSeconds = interpolatedTimestamp / 1000.0;
        }

        private static TControlType GetElementByTreePath<TControlType>(
            DependencyObject parent, 
            String treePath, 
            Predicate<TControlType> predicate, 
            int pos = 0)
            where TControlType : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string[] elements = treePath.Split('.');
                if (elements.Length > pos)
                {
                    String searchElement = elements[pos];
                    if (child.ToString().Split(' ')[0].EndsWith(searchElement))
                    {
                        if (elements.Length == pos + 1)
                        {
                            if (child is TControlType castedChild && predicate(castedChild))
                            {
                                return castedChild;
                            }
                            continue;
                        }

                        var result = GetElementByTreePath<TControlType>(child, treePath, predicate, pos + 1);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        private TextBlock _videoPositionTextBlock = null;
        private Button _playButton = null;
        private TextBlock _playerSpeedTextBlock = null;
        private bool _isPlaying = false;
        private double _playerSpeed = 1.0;
        private double _videoPositionInSeconds = 0.0;
        private string _lastVideoPositionString = String.Empty;
        private DateTime _breakEvenTime = DateTime.Now;
        private static readonly Lazy<VirtualDesktopUiSettings> lazy =
         new Lazy<VirtualDesktopUiSettings>(() => new VirtualDesktopUiSettings());

    }
}
