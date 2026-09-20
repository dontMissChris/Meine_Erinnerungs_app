using System;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    public partial class ToastNotificationWindow : Window
    {
        private DispatcherTimer autoCloseTimer;
        private bool isClosing = false;
        private bool isPersistent; // True = dauerhaft (für abgelaufene Termine)

        public ToastNotificationWindow(string message, bool isPersistent = false)
        {
            InitializeComponent();
            MessageText.Text = message;
            this.isPersistent = isPersistent;

            var workingArea = SystemParameters.WorkArea;
            Width = workingArea.Width;
            Left = workingArea.Left;
            Top = workingArea.Top - Height;

            Loaded += ToastNotificationWindow_Loaded;
        }

        private void ToastNotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (isClosing) return;

            try
            {
                Opacity = 0;

                var workingArea = SystemParameters.WorkArea;
                double startTop = workingArea.Top - Height;
                double endTop = workingArea.Top;

                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                var slideDown = new DoubleAnimation
                {
                    From = startTop,
                    To = endTop,
                    Duration = TimeSpan.FromSeconds(0.4),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                BeginAnimation(OpacityProperty, fadeIn);
                BeginAnimation(TopProperty, slideDown);

                if (!isPersistent)
                {
                    autoCloseTimer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(10)
                    };
                    autoCloseTimer.Tick += (s, args) =>
                    {
                        if (!isClosing)
                        {
                            CloseWithAnimation();
                        }
                    };
                    autoCloseTimer.Start();
                }

                StartBlinkAnimation();
                StartGlowAnimation();
                PlayAlarmSound();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Toast Loaded Error: {ex.Message}");
                SafeClose();
            }
        }

        private void StartBlinkAnimation()
        {
            try
            {
                var colorAnimation = new ColorAnimation
                {
                    From = Color.FromRgb(211, 47, 47), // #D32F2F
                    To = Color.FromRgb(244, 67, 54),   // Helleres Rot
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                MainBorder.Background = new SolidColorBrush(Color.FromRgb(211, 47, 47));
                MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }
            catch { }
        }

        // NEU: Leuchtender Rand um das gesamte Toast-Fenster (Desktop-Glow)
        private void StartGlowAnimation()
        {
            try
            {
                if (MainBorder.Effect is DropShadowEffect glow)
                {
                    var blurAnimation = new DoubleAnimation
                    {
                        From = 5,
                        To = 40,
                        Duration = TimeSpan.FromSeconds(0.6),
                        AutoReverse = true,
                        RepeatBehavior = RepeatBehavior.Forever,
                        EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                    };
                    glow.BeginAnimation(DropShadowEffect.BlurRadiusProperty, blurAnimation);
                }
            }
            catch { }
        }

        // NEU: Warnton beim Erscheinen
        private void PlayAlarmSound()
        {
            try
            {
                SystemSounds.Exclamation.Play();
            }
            catch { }
        }

        private void CloseButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseButton.Background = new SolidColorBrush(Color.FromRgb(255, 241, 118)); // #FFF176
        }

        private void CloseButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseButton.Background = new SolidColorBrush(Color.FromRgb(255, 235, 59)); // #FFEB3B
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWithAnimation();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopAllAnimations();
            if (autoCloseTimer != null)
            {
                autoCloseTimer.Stop();
                autoCloseTimer = null;
            }
        }

        private void CloseWithAnimation()
        {
            if (isClosing) return;
            isClosing = true;

            try
            {
                if (autoCloseTimer != null)
                {
                    autoCloseTimer.Stop();
                    autoCloseTimer = null;
                }

                StopAllAnimations();

                var fadeOut = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                fadeOut.Completed += (s, args) =>
                {
                    try
                    {
                        Dispatcher.InvokeAsync(() => SafeClose());
                    }
                    catch { }
                };

                BeginAnimation(OpacityProperty, fadeOut);
            }
            catch
            {
                SafeClose();
            }
        }

        private void StopAllAnimations()
        {
            try
            {
                BeginAnimation(OpacityProperty, null);
                BeginAnimation(TopProperty, null);
                BeginAnimation(LeftProperty, null);

                if (MainBorder?.Background != null)
                {
                    MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                }

                if (MainBorder?.Effect is DropShadowEffect glow)
                {
                    glow.BeginAnimation(DropShadowEffect.BlurRadiusProperty, null);
                }
            }
            catch { }
        }

        private void SafeClose()
        {
            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => SafeClose());
                    return;
                }

                if (IsVisible)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SafeClose Error: {ex.Message}");
            }
        }
    }
}