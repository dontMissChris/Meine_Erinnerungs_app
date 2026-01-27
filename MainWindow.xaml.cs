using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Termin> Termine { get; set; }
        private DispatcherTimer timer;

        // Konstanten für Datum- und Zeitformate
        private const string DateFormat = "dd.MM.yyyy";
        private const string TimeFormat = "HH:mm";
        private const string DateTimeFormat = DateFormat + " " + TimeFormat;
        
        // Konstante für maximale Minuten bis zum Termin (für Fortschrittsberechnung)
        private const double MaxMinutenFuerFortschritt = 120; // 2 Stunden

        public MainWindow()
        {
            InitializeComponent();
            Termine = new ObservableCollection<Termin>();
            this.DataContext = this;

            // Timer für regelmäßige Prüfung der Termine
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5); // Alle 5 Sekunden prüfen
            timer.Tick += (s, e) => TerminePruefen();
            timer.Start();
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
            // Termin hinzufügen
            if (GrundTextBox.Text != "Grund" && !string.IsNullOrWhiteSpace(GrundTextBox.Text) &&
                DatumTextBox.Text != "Datum" && !string.IsNullOrWhiteSpace(DatumTextBox.Text) &&
                UhrzeitTextBox.Text != "Uhrzeit" && !string.IsNullOrWhiteSpace(UhrzeitTextBox.Text))
            {
                try
                {
                    // Datum und Uhrzeit parsen
                    string datumStr = DatumTextBox.Text;
                    string uhrzeitStr = UhrzeitTextBox.Text;
                    
                    DateTime terminZeit = DateTime.ParseExact(
                        datumStr + " " + uhrzeitStr,
                        DateTimeFormat,
                        CultureInfo.InvariantCulture);

                    var termin = new Termin
                    {
                        Grund = GrundTextBox.Text,
                        TerminZeit = terminZeit
                    };

                    Termine.Add(termin);
                    TerminePruefen();

                    MessageBox.Show("ERINNERUNG SCHARF GESTELLT", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Felder zurücksetzen
                    GrundTextBox.Text = "Grund";
                    GrundTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                    DatumTextBox.Text = "Datum";
                    DatumTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                    UhrzeitTextBox.Text = "Uhrzeit";
                    UhrzeitTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Bitte geben Sie das Datum im Format '{DateFormat}' und die Uhrzeit im Format '{TimeFormat}' ein.",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Ausgewählten Termin löschen
            if (TermineListView.SelectedItem is Termin selectedTermin)
            {
                Termine.Remove(selectedTermin);
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie einen Termin zum Löschen aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TerminePruefen()
        {
            DateTime jetzt = DateTime.Now;

            foreach (var termin in Termine)
            {
                TimeSpan differenz = termin.TerminZeit - jetzt;
                double totalMinutes = differenz.TotalMinutes;

                // Fortschritt berechnen (0-100%)
                // Wir nehmen an, dass MaxMinutenFuerFortschritt vor dem Termin der Fortschritt bei 0% beginnt
                double fortschritt = Math.Max(0, Math.Min(100, ((MaxMinutenFuerFortschritt - totalMinutes) / MaxMinutenFuerFortschritt) * 100));
                termin.Fortschritt = fortschritt;

                // Farben basierend auf verbleibender Zeit setzen
                if (totalMinutes > 60)
                {
                    // Grün: Mehr als 1 Stunde
                    termin.BackgroundColor = new SolidColorBrush(Color.FromRgb(144, 238, 144)); // LightGreen
                    termin.ProgressBarColor = new SolidColorBrush(Color.FromRgb(34, 139, 34)); // ForestGreen
                }
                else if (totalMinutes >= 15)
                {
                    // Gold/Gelb: 15 Minuten bis 1 Stunde
                    termin.BackgroundColor = new SolidColorBrush(Color.FromRgb(255, 255, 153)); // Light Yellow
                    termin.ProgressBarColor = new SolidColorBrush(Color.FromRgb(255, 215, 0)); // Gold
                }
                else if (totalMinutes >= 5)
                {
                    // Orange: 5 bis 15 Minuten
                    termin.BackgroundColor = new SolidColorBrush(Color.FromRgb(255, 200, 124)); // Light Orange
                    termin.ProgressBarColor = new SolidColorBrush(Color.FromRgb(255, 140, 0)); // DarkOrange
                }
                else
                {
                    // Rot: Weniger als 5 Minuten / Abgelaufen
                    termin.BackgroundColor = new SolidColorBrush(Color.FromRgb(255, 160, 160)); // Light Red
                    termin.ProgressBarColor = new SolidColorBrush(Color.FromRgb(220, 20, 60)); // Crimson
                }
            }
        }
    }
}
