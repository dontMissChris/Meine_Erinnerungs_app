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
            if (textBox != null && (textBox.Text == "Grund" || textBox.Text == "Datum" || textBox.Text == "Uhrzeit"))
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
                if (textBox.Name == "GrundTextBox")
                {
                    textBox.Text = "Grund";
                }
                else if (textBox.Name == "DatumTextBox")
                {
                    textBox.Text = "Datum";
                }
                else if (textBox.Name == "UhrzeitTextBox")
                {
                    textBox.Text = "Uhrzeit";
                }
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get TextBox values
            var stackPanel = ((sender as Button).Parent as StackPanel);
            var textBoxes = stackPanel.Children.OfType<TextBox>().ToList();

            if (textBoxes.Count >= 3)
            {
                string grund = textBoxes[0].Text;
                string datum = textBoxes[1].Text;
                string uhrzeit = textBoxes[2].Text;

                // Validate input
                if (string.IsNullOrWhiteSpace(grund) || grund == "Grund" ||
                    string.IsNullOrWhiteSpace(datum) || datum == "Datum" ||
                    string.IsNullOrWhiteSpace(uhrzeit) || uhrzeit == "Uhrzeit")
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create and show toast notification
                string dateTimeText = $"{datum} um {uhrzeit}";
                ToastNotificationWindow toast = new ToastNotificationWindow(grund, dateTimeText);
                toast.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Clear all text boxes
            var stackPanel = ((sender as Button).Parent as StackPanel);
            var textBoxes = stackPanel.Children.OfType<TextBox>().ToList();

            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }

            // Reset placeholder text
            if (textBoxes.Count >= 3)
            {
                textBoxes[0].Text = "Grund";
                textBoxes[1].Text = "Datum";
                textBoxes[2].Text = "Uhrzeit";
            }
        }
    }
}
