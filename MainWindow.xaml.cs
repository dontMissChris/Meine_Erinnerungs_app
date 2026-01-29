using System.Windows;

namespace Meine_Erinnerungs_app
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ERINNERUNG SCHARF GESTELLT");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GrundTextBox.Clear();
            DatumTextBox.Clear();
            UhrzeitTextBox.Clear();
        }
    }
}
