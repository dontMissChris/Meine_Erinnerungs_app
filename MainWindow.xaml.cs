using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Termin> Termine { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Termine = new ObservableCollection<Termin>();
            ErgebnisListBox.ItemsSource = Termine;
            StartReminderCheck();
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Grund")
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Grund";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void DatumTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Hier können Sie zusätzliche Logik hinzufügen, wenn das Datum geändert wird
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GrundTextBox.Text == "Grund")
            {
                MessageBox.Show("Bitte geben Sie einen Grund ein.");
                return;
            }
            if (!DatumTextBox.SelectedDate.HasValue)
            {
                MessageBox.Show("Bitte geben Sie ein Datum ein.");
                return;
            }
            if (StundenComboBox.SelectedItem == null || MinutenComboBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte geben Sie eine Uhrzeit ein.");
                return;
            }

            string grund = GrundTextBox.Text;
            string datum = DatumTextBox.SelectedDate.HasValue ? DatumTextBox.SelectedDate.Value.ToShortDateString() : "Kein Datum ausgewählt";
            string stunden = StundenComboBox.SelectedItem != null ? (StundenComboBox.SelectedItem as ComboBoxItem).Content.ToString() : "00";
            string minuten = MinutenComboBox.SelectedItem != null ? (MinutenComboBox.SelectedItem as ComboBoxItem).Content.ToString() : "00";
            string uhrzeit = $"{stunden}:{minuten}";

            if (DateTime.TryParse(uhrzeit, out DateTime parsedTime))
            {
                var termin = new Termin
                {
                    Grund = grund,
                    Datum = datum,
                    Uhrzeit = uhrzeit,
                    Zeitpunkt = DatumTextBox.SelectedDate.Value.Add(parsedTime.TimeOfDay)
                };

                Termine.Add(termin);
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige Uhrzeit ein.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ErgebnisListBox.SelectedItem is Termin selectedTermin)
            {
                Termine.Remove(selectedTermin);
            }
        }

        private void StartReminderCheck()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var termin in Termine)
            {
                if (DateTime.Now > termin.Zeitpunkt)
                {
                    termin.Background = new SolidColorBrush(Colors.Red);
                    if (!termin.AlarmTriggered)
                    {
                        MessageBox.Show($"Erinnerung: {termin.Grund} um {termin.Uhrzeit} am {termin.Datum}");
                        termin.AlarmTriggered = true;
                    }
                }
                else if (DateTime.Now.AddMinutes(5) > termin.Zeitpunkt)
                {
                    termin.Background = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    termin.Background = new SolidColorBrush(Colors.Green);
                }
            }
        }

        private void ErgebnisListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ErgebnisListBox.SelectedItem is Termin selectedTermin)
            {
                var listBoxItem = (ListBoxItem)ErgebnisListBox.ItemContainerGenerator.ContainerFromItem(selectedTermin);
                if (listBoxItem != null)
                {
                    // Animation für das Auswählen eines Termins soll langsamer und sanfter sein, Termin soll dem Benutzer entgegenkommen
                    DoubleAnimation scaleAnimation = new DoubleAnimation(1, 1.2, TimeSpan.FromSeconds(0.5));
                    DoubleAnimation opacityAnimation = new DoubleAnimation(1, 0.8, TimeSpan.FromSeconds(0.5));
                    ScaleTransform scaleTransform = new ScaleTransform();
                    listBoxItem.RenderTransform = scaleTransform;
                    listBoxItem.RenderTransformOrigin = new Point(0.5, 0.5);
                    listBoxItem.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                    
                    // wenn Termin deselektiert wird, soll er wieder in die ursprüngliche Größe zurückkehren mit Animation
                    listBoxItem.Unselected += (s, ev) =>
                    {
                        DoubleAnimation scaleAnimation2 = new DoubleAnimation(1.2, 1, TimeSpan.FromSeconds(0.5));
                        DoubleAnimation opacityAnimation2 = new DoubleAnimation(0.8, 1, TimeSpan.FromSeconds(0.5));
                        ScaleTransform scaleTransform2 = new ScaleTransform();
                        listBoxItem.RenderTransform = scaleTransform2;
                        listBoxItem.RenderTransformOrigin = new Point(0.5, 0.5);
                        listBoxItem.BeginAnimation(UIElement.OpacityProperty, opacityAnimation2);
                        scaleTransform2.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation2);
                        scaleTransform2.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation2);
                    };
                }
            }

            DeleteButton.IsEnabled = ErgebnisListBox.SelectedItem != null;

            if (ErgebnisListBox.Items.Count == 0)
            {
                DeleteButton.IsEnabled = false;
            }
        }
    }

    public class Termin
    {
        public string Grund { get; set; }
        public string Datum { get; set; }
        public string Uhrzeit { get; set; }
        public DateTime Zeitpunkt { get; set; }
        public Brush Background { get; set; } = new SolidColorBrush(Colors.Green);
        public bool AlarmTriggered { get; set; } = false;
    }
}
