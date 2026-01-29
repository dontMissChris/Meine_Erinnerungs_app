using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    public partial class ToastNotificationWindow : Window
    {
        private DispatcherTimer autoCloseTimer;
        private bool isClosing = false;

        public ToastNotificationWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
            
            // Window-Position sofort setzen (vor Loaded)
            var workingArea = SystemParameters.WorkArea;
            Width = workingArea.Width;
            Left = workingArea.Left;
            Top = workingArea.Top - Height; // Starte außerhalb des Bildschirms
            
            Loaded += ToastNotificationWindow_Loaded;
        }

        private void ToastNotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (isClosing) return;

            try
            {
                // Einfache Animation: Fade In + Slide Down
                Opacity = 0;
                
                var workingArea = SystemParameters.WorkArea;
                double startTop = workingArea.Top - Height;
                double endTop = workingArea.Top;

                // Opacity Animation
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                // Top Position Animation
                var slideDown = new DoubleAnimation
                {
                    From = startTop,
                    To = endTop,
                    Duration = TimeSpan.FromSeconds(0.4),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                // Animationen starten
                BeginAnimation(OpacityProperty, fadeIn);
                BeginAnimation(TopProperty, slideDown);

                // Auto-Close Timer
                autoCloseTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(8)
                };
                autoCloseTimer.Tick += (s, args) =>
                {
                    if (!isClosing)
                    {
                        CloseWithAnimation();
                    }
                };
                autoCloseTimer.Start();

                // Blink-Effekt für Aufmerksamkeit
                StartBlinkAnimation();
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
                    RepeatBehavior = new RepeatBehavior(3) // 3x blinken
                };

                MainBorder.Background = new SolidColorBrush(Color.FromRgb(211, 47, 47));
                MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
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
            // Cleanup
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
                // Timer stoppen
                if (autoCloseTimer != null)
                {
                    autoCloseTimer.Stop();
                    autoCloseTimer = null;
                }

                // Alle laufenden Animationen stoppen
                StopAllAnimations();

                // Fade Out Animation
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

