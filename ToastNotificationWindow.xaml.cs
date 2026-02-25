using System.Windows;

namespace Meine_Erinnerungs_app
{
    public partial class ToastNotificationWindow : Window
    {
        public ToastNotificationWindow(Termin termin)
        {
            InitializeComponent();
            MessageText.Text = termin.Grund;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
