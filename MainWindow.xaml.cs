using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Meine_Erinnerungs_app
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Termin> Termine { get; set; }
        public string Stunden { get; private set; }
        public string Minuten { get; private set; }

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
            if (textBox != null && textBox.Text == "Bitte Begründung eingeben")
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
                textBox.Text = "Bitte Begründung eingeben";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void GrundTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RemovePlaceholderText(sender, e);
        }

        private void GrundTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddPlaceholderText(sender, e);
        }

        private void DatumTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatumTextBox.SelectedDate.HasValue && DatumTextBox.SelectedDate.Value.Date == DateTime.Today)
            {
                ZeitComboBox.Items.Clear();
                DateTime now = DateTime.Now;
                for (int hour = now.Hour; hour < 24; hour++)
                {
                    for (int minute = (hour == now.Hour ? now.Minute : 0); minute < 60; minute += 15)
                    {
                        ZeitComboBox.Items.Add(new ComboBoxItem { Content = new DateTime(1, 1, 1, hour, minute, 0).ToString("HH:mm") });
                    }
                }
            }
            else
            {
                // Füllen Sie die Zeit-ComboBox mit allen möglichen Zeiten
                ZeitComboBox.Items.Clear();
                for (int hour = 0; hour < 24; hour++)
                {
                    for (int minute = 0; minute < 60; minute += 15)
                    {
                        ZeitComboBox.Items.Add(new ComboBoxItem { Content = new DateTime(1, 1, 1, hour, minute, 0).ToString("HH:mm") });
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Moderne Animation beim Klicken des Buttons
            Button addButton = sender as Button;
            if (addButton == null) return;

            DoubleAnimation scaleUpAnimation = new DoubleAnimation(1, 1.3, TimeSpan.FromSeconds(0.5));
            DoubleAnimation scaleDownAnimation = new DoubleAnimation(1.3, 1, TimeSpan.FromSeconds(0.5));
            DoubleAnimation opacityAnimation = new DoubleAnimation(1, 0.9, TimeSpan.FromSeconds(0.5));
            DoubleAnimation opacityBackAnimation = new DoubleAnimation(0.9, 1, TimeSpan.FromSeconds(0.5));

            ScaleTransform scaleTransform = new ScaleTransform();
            addButton.RenderTransform = scaleTransform;
            addButton.RenderTransformOrigin = new Point(0.5, 0.5);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(scaleUpAnimation);
            storyboard.Children.Add(scaleDownAnimation);
            storyboard.Children.Add(opacityAnimation);
            storyboard.Children.Add(opacityBackAnimation);

            Storyboard.SetTarget(scaleUpAnimation, addButton);
            Storyboard.SetTargetProperty(scaleUpAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(scaleUpAnimation, new PropertyPath("RenderTransform.ScaleY"));

            Storyboard.SetTarget(scaleDownAnimation, addButton);
            Storyboard.SetTargetProperty(scaleDownAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(scaleDownAnimation, new PropertyPath("RenderTransform.ScaleY"));

            Storyboard.SetTarget(opacityAnimation, addButton);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(opacityBackAnimation, addButton);
            Storyboard.SetTargetProperty(opacityBackAnimation, new PropertyPath("Opacity"));

            storyboard.Begin();

            AddTermin();
        }

        private void AddTermin()
        {
            if (GrundTextBox.Text == "Grund")
            {
                MessageBox.Show("Grund fehlt!");
                return;
            }
            if (!DatumTextBox.SelectedDate.HasValue)
            {
                MessageBox.Show("Hey, das Datum fehlt!");
                return;
            }
            if (string.IsNullOrEmpty(ZeitComboBox.Text))
            {
                MessageBox.Show("Die Uhrzeit fehlt auch noch!");
                return;
            }

            string grund = GrundTextBox.Text;
            string datum = DatumTextBox.SelectedDate.HasValue ? DatumTextBox.SelectedDate.Value.ToShortDateString() : "Kein Datum ausgewählt";
            string uhrzeit = ZeitComboBox.Text;

            if (DateTime.TryParse(uhrzeit, out DateTime parsedTime))
            {
                DateTime selectedDateTime = DatumTextBox.SelectedDate.Value.Add(parsedTime.TimeOfDay);
                if (selectedDateTime < DateTime.Now)
                {
                    MessageBox.Show("Die ausgewählte Uhrzeit liegt in der Vergangenheit!");
                    return;
                }

                var termin = new Termin
                {
                    Grund = grund,
                    Datum = datum,
                    Uhrzeit = uhrzeit,
                    Zeitpunkt = selectedDateTime
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
                TimeSpan timeUntilTermin = termin.Zeitpunkt - DateTime.Now;

                if (timeUntilTermin <= TimeSpan.Zero)
                {
                    termin.Background = new SolidColorBrush(Colors.Red);
                    if (!termin.AlarmTriggered)
                    {
                        ShowPopup($"Erinnerung: {termin.Grund} um {termin.Uhrzeit} am {termin.Datum}");
                        termin.AlarmTriggered = true;
                    }
                }
                else if (timeUntilTermin <= TimeSpan.FromMinutes(5))
                {
                    termin.Background = new SolidColorBrush(Colors.Yellow);
                    if (!termin.FiveMinutesWarning)
                    {
                        ShowPopup($"Erinnerung: {termin.Grund} in 5 Minuten");
                        termin.FiveMinutesWarning = true;
                    }
                }
                else if (timeUntilTermin <= TimeSpan.FromMinutes(15))
                {
                    termin.Background = new SolidColorBrush(Colors.Yellow);
                    if (!termin.FifteenMinutesWarning)
                    {
                        ShowPopup($"Erinnerung: {termin.Grund} in 15 Minuten");
                        termin.FifteenMinutesWarning = true;
                    }
                }
                else if (timeUntilTermin <= TimeSpan.FromMinutes(30))
                {
                    termin.Background = new SolidColorBrush(Colors.Yellow);
                    if (!termin.ThirtyMinutesWarning)
                    {
                        ShowPopup($"Erinnerung: {termin.Grund} in 30 Minuten");
                        termin.ThirtyMinutesWarning = true;
                    }
                }
                else if (timeUntilTermin <= TimeSpan.FromMinutes(60))
                {
                    termin.Background = new SolidColorBrush(Colors.Yellow);
                    if (!termin.SixtyMinutesWarning)
                    {
                        ShowPopup($"Erinnerung: {termin.Grund} in 60 Minuten");
                        termin.SixtyMinutesWarning = true;
                    }
                }
                else
                {
                    termin.Background = new SolidColorBrush(Colors.Green);
                }

                if (timeUntilTermin <= TimeSpan.FromMinutes(60) && timeUntilTermin > TimeSpan.Zero)
                {
                    BlinkBackground(termin);
                }
            }
        }

        private void BlinkBackground(Termin termin)
        {
            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = Colors.Yellow,
                To = Colors.Transparent,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            termin.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private void ShowPopup(string message)
        {
            System.Media.SystemSounds.Exclamation.Play();

            Popup popup = new Popup
            {
                Placement = PlacementMode.Center,
                StaysOpen = false,
                Child = new TextBlock
                {
                    Text = message,
                    Background = new SolidColorBrush(Colors.LightYellow),
                    Foreground = new SolidColorBrush(Colors.Black),
                    Padding = new Thickness(10),
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Width = SystemParameters.PrimaryScreenWidth // Set the width to the screen width
                }
            };

            DoubleAnimation moveAnimation = new DoubleAnimation
            {
                From = -200,
                To = 200,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = Colors.Red,
                To = Colors.Blue,
                Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            TranslateTransform translateTransform = new TranslateTransform();
            popup.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, moveAnimation);

            SolidColorBrush backgroundBrush = new SolidColorBrush(Colors.Red);
            backgroundBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            ((TextBlock)popup.Child).Background = backgroundBrush;

            // Text animation
            DoubleAnimation textSizeAnimation = new DoubleAnimation
            {
                From = 24,
                To = 30,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ((TextBlock)popup.Child).BeginAnimation(TextBlock.FontSizeProperty, textSizeAnimation);

            popup.IsOpen = true;
        }

        private void ErgebnisListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ErgebnisListBox.SelectedItem is Termin selectedTermin)
            {
                var listBoxItem = (ListBoxItem)ErgebnisListBox.ItemContainerGenerator.ContainerFromItem(selectedTermin);
                if (listBoxItem != null)
                {
                    DoubleAnimation scaleAnimation = new DoubleAnimation(1, 1.2, TimeSpan.FromSeconds(0.5));
                    DoubleAnimation opacityAnimation = new DoubleAnimation(1, 0.8, TimeSpan.FromSeconds(0.5));
                    // rot leuchtendes Blinken
                    ColorAnimation colorAnimation = new ColorAnimation
                    {
                        From = Colors.Red,
                        To = Colors.Transparent,
                        Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                        AutoReverse = true,
                        RepeatBehavior = RepeatBehavior.Forever
                    };
                    ScaleTransform scaleTransform = new ScaleTransform();
                    listBoxItem.RenderTransform = scaleTransform;
                    listBoxItem.RenderTransformOrigin = new Point(0.5, 0.5);
                    listBoxItem.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

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

        private void GrundTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ZeitComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Öffnen Sie ein TimePicker-Fenster
            TimePickerWindow timePicker = new TimePickerWindow();
            timePicker.Owner = this; // Setzen Sie das Hauptfenster als Besitzer
            if (timePicker.ShowDialog() == true)
            {
                // Holen Sie sich die ausgewählte Zeit
                DateTime selectedTime = timePicker.SelectedTime;

                // Setzen Sie die Zeit für den Termin
                ZeitComboBox.Text = selectedTime.ToString("HH:mm");
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
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
            if (ZeitComboBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte geben Sie eine Uhrzeit ein.");
                return;
            }

            string grund = GrundTextBox.Text;
            string datum = DatumTextBox.SelectedDate.HasValue ? DatumTextBox.SelectedDate.Value.ToShortDateString() : "Kein Datum ausgewählt";
            string uhrzeit = ZeitComboBox.SelectedItem != null ? (ZeitComboBox.SelectedItem as ComboBoxItem).Content.ToString() : "00:00";

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

        private void HeuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Heutiges Datum einstellen und anzeigen
            DatumTextBox.SelectedDate = DateTime.Today;
        }

        public void UpdateZeitComboBox(DateTime selectedTime)
        {
            ZeitComboBox.Items.Clear();
            ZeitComboBox.Items.Add(selectedTime.ToString("HH:mm"));
            ZeitComboBox.SelectedIndex = 0;
        }

        private void ZeitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Zeit Digital horizontal und vertikal zentriert anzeigen
            if (ZeitComboBox.SelectedItem != null)
            {
                ZeitComboBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                ZeitComboBox.VerticalContentAlignment = VerticalAlignment.Center;
            }

            if (ZeitComboBox.SelectedItem != null)
            {
                string selectedTime;
                if (ZeitComboBox.SelectedItem is ComboBoxItem comboBoxItem)
                {
                    selectedTime = comboBoxItem.Content.ToString();
                }
                else
                {
                    selectedTime = ZeitComboBox.SelectedItem.ToString();
                }
                Stunden = selectedTime.Split(':')[0];
                Minuten = selectedTime.Split(':')[1];
            }
            else
            {
                Stunden = DateTime.Now.Hour.ToString("D2");
                Minuten = DateTime.Now.Minute.ToString("D2");
                ZeitComboBox.Items.Add($"{Stunden}:{Minuten}");
                ZeitComboBox.SelectedIndex = 0;
            }
            ZeitComboBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        // "MainWindow" enthält keine Definition für "ZeitComboBox_GotFocus", und es konnte keine zugängliche ZeitComboBox_GotFocus-Erweiterungsmethode gefunden werden
        private void ZeitComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ZeitComboBox.Text == "Eine UHRZEIT wird noch benötigt!")
            {
                ZeitComboBox.Text = "";
                ZeitComboBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ZeitComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ZeitComboBox.Text))
            {
                ZeitComboBox.Text = "Eine UHRZEIT wird noch benötigt!";
                ZeitComboBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            }
        }
    }
}
        
        
