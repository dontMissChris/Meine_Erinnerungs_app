using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Termin> termine = new ObservableCollection<Termin>();

        public MainWindow()
        {
            InitializeComponent();
            TerminListe.ItemsSource = termine;
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
                    textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                }
                else if (textBox.Name == "DatumTextBox")
                {
                    textBox.Text = "Datum";
                    textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                }
                else if (textBox.Name == "UhrzeitTextBox")
                {
                    textBox.Text = "Uhrzeit";
                    textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Termin hinzufügen
            string grund = GrundTextBox.Text;
            string datum = DatumTextBox.Text;
            string uhrzeit = UhrzeitTextBox.Text;

            if (grund != "Grund" && !string.IsNullOrWhiteSpace(grund) &&
                datum != "Datum" && !string.IsNullOrWhiteSpace(datum) &&
                uhrzeit != "Uhrzeit" && !string.IsNullOrWhiteSpace(uhrzeit))
            {
                termine.Add(new Termin { Grund = grund, Datum = datum, Uhrzeit = uhrzeit });
                
                // Felder zurücksetzen
                GrundTextBox.Text = "Grund";
                GrundTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                DatumTextBox.Text = "Datum";
                DatumTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                UhrzeitTextBox.Text = "Uhrzeit";
                UhrzeitTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                
                MessageBox.Show("Erinnerung hinzugefügt!");
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Ausgewählten Termin löschen
            if (TerminListe.SelectedItem != null)
            {
                termine.Remove((Termin)TerminListe.SelectedItem);
                MessageBox.Show("Erinnerung gelöscht!");
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie einen Termin zum Löschen aus!");
            }
        }
    }

    public class Termin
    {
        public string Grund { get; set; }
        public string Datum { get; set; }
        public string Uhrzeit { get; set; }
    }
}
