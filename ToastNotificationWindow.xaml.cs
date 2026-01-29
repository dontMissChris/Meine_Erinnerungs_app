using System;
using System.Windows;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    /// <summary>
    /// Interaktionslogik für ToastNotificationWindow.xaml
    /// </summary>
    public partial class ToastNotificationWindow : Window
    {
        private DispatcherTimer closeTimer;

        public ToastNotificationWindow(string reason, string dateTime)
        {
            InitializeComponent();
            ReasonTextBlock.Text = reason;
            DateTimeTextBlock.Text = dateTime;

            // Position window after it's loaded to ensure correct dimensions
            this.Loaded += ToastNotificationWindow_Loaded;
            
            // Clean up timer when window closes
            this.Closing += ToastNotificationWindow_Closing;

            // Auto-close after 5 seconds
            closeTimer = new DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromSeconds(5);
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                this.Close();
            };
            closeTimer.Start();
        }

        private void ToastNotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Position window at bottom-right corner after it's fully loaded
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth - 10;
            this.Top = SystemParameters.WorkArea.Height - this.ActualHeight - 10;
        }

        private void ToastNotificationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Ensure timer is stopped and disposed
            if (closeTimer != null)
            {
                closeTimer.Stop();
                closeTimer = null;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
