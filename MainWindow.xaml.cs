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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Meine_Erinnerungs_app
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Grund")
            {
                textBox.Text = "";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Grund";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void DatumTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Hier können Sie zusätzliche Logik hinzufügen, wenn das Datum geändert wird
        }

        private void UhrzeitTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Hier können Sie zusätzliche Logik hinzufügen, wenn die Uhrzeit geändert wird
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string grund = GrundTextBox.Text;
            string datum = DatumTextBox.SelectedDate.HasValue ? DatumTextBox.SelectedDate.Value.ToShortDateString() : "Kein Datum ausgewählt";
            string uhrzeit = UhrzeitTextBox.SelectedItem != null ? (UhrzeitTextBox.SelectedItem as ComboBoxItem).Content.ToString() : "Keine Uhrzeit ausgewählt";

            ErgebnisTextBox.Text = $"Grund: {grund}\nDatum: {datum}\nUhrzeit: {uhrzeit}";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GrundTextBox.Text = "Grund";
            GrundTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            DatumTextBox.SelectedDate = null;
            UhrzeitTextBox.SelectedItem = null;
            ErgebnisTextBox.Text = string.Empty;
        }
    }
}
