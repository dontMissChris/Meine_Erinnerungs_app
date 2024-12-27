using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Meine_Erinnerungs_app
{
    /// <summary>
    /// Interaktionslogik für TimePickerWindow.xaml
    /// </summary>
    public partial class TimePickerWindow : Window
    {
        public int SelectedHour { get; set; }
        public int SelectedMinute { get; set; }
        public DateTime SelectedTime { get; internal set; }

        public TimePickerWindow()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (StundenComboBox.SelectedItem != null && MinutenComboBox.SelectedItem != null)
            {
                // Extract the content of the ComboBoxItem and convert it to an integer
                ComboBoxItem selectedHourItem = (ComboBoxItem)StundenComboBox.SelectedItem;
                ComboBoxItem selectedMinuteItem = (ComboBoxItem)MinutenComboBox.SelectedItem;

                SelectedHour = Convert.ToInt32(selectedHourItem.Content);
                SelectedMinute = Convert.ToInt32(selectedMinuteItem.Content);
                SelectedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SelectedHour, SelectedMinute, 0);

                // Update ZeitComboBox in MainWindow
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.UpdateZeitComboBox(SelectedTime);

                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie eine gültige Uhrzeit aus.");
            }
        }
    }
}
