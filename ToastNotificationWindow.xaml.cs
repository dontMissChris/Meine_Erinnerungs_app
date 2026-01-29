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

            // Position window at bottom-right corner
            this.Left = SystemParameters.WorkArea.Width - this.Width - 10;
            this.Top = SystemParameters.WorkArea.Height - this.Height - 10;

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            closeTimer.Stop();
            this.Close();
        }
    }
}
