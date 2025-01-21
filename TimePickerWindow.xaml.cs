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
        public DateTime SelectedTime { get; private set; }
        public ComboBox MinuteComboBox { get; private set; }

        private int hours = 0;
        private int minutes = 0;

        public TimePickerWindow()
        {
            InitializeComponent();
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            HoursTextBlock.Text = $"Stunden: {hours:D2}";
            MinutesTextBlock.Text = $"Minuten: {minutes:D2}";
        }

        private void HoursSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            hours = (int)e.NewValue;
            UpdateTimeDisplay();

        }

        private void MinutesSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            minutes = (int)e.NewValue;
            UpdateTimeDisplay();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (HoursComboBox.SelectedItem != null && MinutesComboBox.SelectedItem != null)
            {
                
                ComboBoxItem selectedHourItem = (ComboBoxItem)HoursComboBox.SelectedItem;
                ComboBoxItem selectedMinuteItem = (ComboBoxItem)MinutesComboBox.SelectedItem;

                hours = Convert.ToInt32(selectedHourItem.Content);
                minutes = Convert.ToInt32(selectedMinuteItem.Content);
                SelectedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);

                
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.UpdateZeitComboBox(SelectedTime);

                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie eine gültige Uhrzeit aus.");
            }
        }

        private void MinutesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Wenn Wert sich geändert hat
        {
            // 1-59 Minuten Darstellung in der ComboBox
            sbyte minute = 1;
            for (int i = 0; i < 59; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = minute;
                MinutesComboBox.Items.Add(item);
                minute++;
            }
        }

        private void HoursComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Selection Changed heißt , dass sich der Wert geändert hat
        {
            // 1-23 Stunden Darstellung in der ComboBox
            sbyte hour = 1;
            for (int i = 0; i < 23; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = hour;
                HoursComboBox.Items.Add(item);
                hour++;
            }
        }
    }
}
