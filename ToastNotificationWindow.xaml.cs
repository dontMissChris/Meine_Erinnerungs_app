using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    public partial class ToastNotificationWindow : Window
    {
        private DispatcherTimer autoCloseTimer;

        public ToastNotificationWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
            
            Loaded += ToastNotificationWindow_Loaded;
        }

        private void ToastNotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = SystemParameters.WorkArea;
            Width = workingArea.Width;
            Left = workingArea.Left;
            Top = workingArea.Top;
            
            var slideDown = new DoubleAnimation
            {
                From = workingArea.Top - Height,
                To = workingArea.Top,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            
            BeginAnimation(TopProperty, slideDown);
            
            autoCloseTimer = new DispatcherTimer();
            autoCloseTimer.Interval = TimeSpan.FromSeconds(10);
            autoCloseTimer.Tick += (s, args) => CloseWithAnimation();
            autoCloseTimer.Start();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWithAnimation();
        }

        private void CloseWithAnimation()
        {
            if (autoCloseTimer != null)
            {
                autoCloseTimer.Stop();
                autoCloseTimer = null;
            }

            var slideUp = new DoubleAnimation
            {
                To = SystemParameters.WorkArea.Top - Height,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            
            slideUp.Completed += (s, args) => Close();
            BeginAnimation(TopProperty, slideUp);
        }
    }
}
