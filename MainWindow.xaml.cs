using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Meine_Erinnerungs_app
{
    public partial class MainWindow : Window
    {
        private int stunden = 0;
        private int minuten = 0;
        private int sekunden = 0;
        private DispatcherTimer zeitgeber;
        private HashSet<Termin> blinkendeTermine = new HashSet<Termin>();

        public ObservableCollection<Termin> Termine { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Termine = new ObservableCollection<Termin>();
            ErgebnisListBox.ItemsSource = Termine;
            ZeitgeberStarten();
            AnzeigeAktualisieren();
        }

        private void StundenHoch_Klick(object sender, RoutedEventArgs e)
        {
            stunden = (stunden + 1) % 24;
            AnzeigeAktualisieren();
        }

        private void StundenRunter_Klick(object sender, RoutedEventArgs e)
        {
            stunden = (stunden - 1 + 24) % 24;
            AnzeigeAktualisieren();
        }

        private void MinutenHoch_Klick(object sender, RoutedEventArgs e)
        {
            minuten = (minuten + 1) % 60;
            AnzeigeAktualisieren();
        }

        private void MinutenRunter_Klick(object sender, RoutedEventArgs e)
        {
            minuten = (minuten - 1 + 60) % 60;
            AnzeigeAktualisieren();
        }

        private void SekundenHoch_Klick(object sender, RoutedEventArgs e)
        {
            sekunden = (sekunden + 1) % 60;
            AnzeigeAktualisieren();
        }

        private void SekundenRunter_Klick(object sender, RoutedEventArgs e)
        {
            sekunden = (sekunden - 1 + 60) % 60;
            AnzeigeAktualisieren();
        }

        private void AnzeigeAktualisieren()
        {
            StundenTextBox.Text = stunden.ToString("D2");
            MinutenTextBox.Text = minuten.ToString("D2");
            SekundenTextBox.Text = sekunden.ToString("D2");
        }

        private void HeuteKnopf_Klick(object sender, RoutedEventArgs e)
        {
            DatumTextBox.SelectedDate = DateTime.Today;
        }

        private void HinzufuegenKnopf_Klick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GrundTextBox.Text) || !DatumTextBox.SelectedDate.HasValue)
            {
                MessageBox.Show("Bitte Beschreibung und Datum eingeben!");
                return;
            }

            if (int.TryParse(StundenTextBox.Text, out int std) && std >= 0 && std < 24)
                stunden = std;
            if (int.TryParse(MinutenTextBox.Text, out int min) && min >= 0 && min < 60)
                minuten = min;
            if (int.TryParse(SekundenTextBox.Text, out int sek) && sek >= 0 && sek < 60)
                sekunden = sek;

            DateTime zeitpunkt = DatumTextBox.SelectedDate.Value.Date.Add(new TimeSpan(stunden, minuten, sekunden));
            
            if (zeitpunkt < DateTime.Now.AddMinutes(-1))
            {
                MessageBox.Show($"Die Zeit liegt in der Vergangenheit!\nEingegebene Zeit: {zeitpunkt}\nAktuelle Zeit: {DateTime.Now}");
                return;
            }

            var farbVerlaeufe = new[]
            {
                new { Bg = "#37474F", Border = "#263238" },
                new { Bg = "#455A64", Border = "#37474F" },
                new { Bg = "#546E7A", Border = "#455A64" },
                new { Bg = "#4A5568", Border = "#2D3748" },
                new { Bg = "#5C6B7A", Border = "#3D4857" },
                new { Bg = "#3F4F5F", Border = "#2A3644" },
            };

            var farbPaar = farbVerlaeufe[Termine.Count % farbVerlaeufe.Length];
            var pinsel = new LinearGradientBrush();
            pinsel.StartPoint = new System.Windows.Point(0, 0);
            pinsel.EndPoint = new System.Windows.Point(1, 1);
            pinsel.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(farbPaar.Bg), 0));
            pinsel.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(farbPaar.Border), 1));

            var termin = new Termin
            {
                Grund = GrundTextBox.Text.Trim(),
                Datum = DatumTextBox.SelectedDate.Value.ToShortDateString(),
                Uhrzeit = $"{stunden:D2}:{minuten:D2}:{sekunden:D2}",
                Zeitpunkt = zeitpunkt,
                CreatedAt = DateTime.Now,
                Background = pinsel,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(farbPaar.Border))
            };

            Termine.Add(termin);
            GrundTextBox.Text = "";
            DatumTextBox.SelectedDate = null;
            stunden = 0;
            minuten = 0;
            sekunden = 0;
            AnzeigeAktualisieren();
        }

        private void LoeschenKnopf_Klick(object sender, RoutedEventArgs e)
        {
            if (ErgebnisListBox.SelectedItem is Termin ausgewaehlt)
            {
                var listenEintrag = ErgebnisListBox.ItemContainerGenerator.ContainerFromItem(ausgewaehlt) as ListBoxItem;
                if (listenEintrag != null)
                {
                    var animationsAblauf = new Storyboard();
                    
                    var ausblenden = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.0,
                        Duration = TimeSpan.FromSeconds(0.8),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
                    };
                    Storyboard.SetTarget(ausblenden, listenEintrag);
                    Storyboard.SetTargetProperty(ausblenden, new PropertyPath("Opacity"));
                    
                    var skalierungX = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.5,
                        Duration = TimeSpan.FromSeconds(0.8),
                        EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn, Exponent = 3 }
                    };
                    var skalierungY = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.5,
                        Duration = TimeSpan.FromSeconds(0.8),
                        EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn, Exponent = 3 }
                    };
                    
                    var transformGruppe = new TransformGroup();
                    var skalierung = new ScaleTransform(1, 1, 0.5, 0.5);
                    transformGruppe.Children.Add(skalierung);
                    listenEintrag.RenderTransform = transformGruppe;
                    listenEintrag.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                    
                    Storyboard.SetTarget(skalierungX, skalierung);
                    Storyboard.SetTargetProperty(skalierungX, new PropertyPath("ScaleX"));
                    Storyboard.SetTarget(skalierungY, skalierung);
                    Storyboard.SetTargetProperty(skalierungY, new PropertyPath("ScaleY"));
                    
                    animationsAblauf.Children.Add(ausblenden);
                    animationsAblauf.Children.Add(skalierungX);
                    animationsAblauf.Children.Add(skalierungY);
                    
                    animationsAblauf.Completed += (s, args) =>
                    {
                        Termine.Remove(ausgewaehlt);
                    };
                    
                    animationsAblauf.Begin();
                }
                else
                {
                    Termine.Remove(ausgewaehlt);
                }
            }
        }

        private void ErgebnisListe_AuswahlGeaendert(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = ErgebnisListBox.SelectedItem != null;
        }

        private void ZeitgeberStarten()
        {
            zeitgeber = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            zeitgeber.Tick += (s, e) => TerminePruefen();
            zeitgeber.Start();
        }

        private void TerminePruefen()
        {
            foreach (var termin in Termine)
            {
                TimeSpan differenz = termin.Zeitpunkt - DateTime.Now;
                TimeSpan gesamtZeit = termin.Zeitpunkt - termin.CreatedAt;
                
                if (gesamtZeit.TotalSeconds > 0)
                {
                    double vergangen = (DateTime.Now - termin.CreatedAt).TotalSeconds;
                    double fortschritt = (vergangen / gesamtZeit.TotalSeconds) * 100;
                    termin.ProgressValue = Math.Min(100, Math.Max(0, fortschritt));
                }
                else
                {
                    termin.ProgressValue = 100;
                }

                if (differenz.TotalSeconds > 0)
                {
                    if (differenz.TotalDays >= 1)
                    {
                        termin.CountdownText = $"In {(int)differenz.TotalDays} Tag(en) {differenz.Hours}h {differenz.Minutes}m";
                    }
                    else if (differenz.TotalHours >= 1)
                    {
                        termin.CountdownText = $"In {differenz.Hours}h {differenz.Minutes}m {differenz.Seconds}s";
                    }
                    else if (differenz.TotalMinutes >= 1)
                    {
                        termin.CountdownText = $"In {differenz.Minutes}m {differenz.Seconds}s";
                    }
                    else
                    {
                        termin.CountdownText = $"In {differenz.Seconds}s - GLEICH!";
                    }
                }
                else
                {
                    termin.CountdownText = "ABGELAUFEN!";
                }

                if (differenz <= TimeSpan.Zero)
                {
                    termin.Background = new SolidColorBrush(Colors.Gray);
                    termin.BorderBrush = new SolidColorBrush(Colors.DarkGray);
                    termin.ProgressBarColor = new SolidColorBrush(Colors.DarkGray);
                    termin.ShouldBlink = false;
                    termin.ShouldPulseText = false;
                    termin.ItemScale = 0.75;
                    if (!termin.AlarmTriggered)
                    {
                        MessageBox.Show($"?? ALARM!\n\n{termin.Grund}\n\n{termin.Datum} um {termin.Uhrzeit}", 
                                       "TERMIN JETZT!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        termin.AlarmTriggered = true;
                    }
                }
                else if (differenz <= TimeSpan.FromMinutes(5))
                {
                    termin.Background = new SolidColorBrush(Colors.OrangeRed);
                    termin.BorderBrush = new SolidColorBrush(Colors.Red);
                    termin.ProgressBarColor = new SolidColorBrush(Colors.Red);
                    termin.ShouldBlink = true;
                    termin.ShouldPulseText = true;
                    termin.ItemScale = 1.4;
                    
                    if (!termin.FiveMinutesWarning && differenz.TotalMinutes <= 5)
                    {
                        var warningWindow = MessageBox.Show($"? 5 MINUTEN!\n{termin.Grund}", $"Noch 5 Minuten bis {termin.Uhrzeit}");
                        // removed
                        termin.FiveMinutesWarning = true;
                    }
                }
                else if (differenz <= TimeSpan.FromMinutes(10))
                {
                    termin.Background = new SolidColorBrush(Colors.Orange);
                    termin.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C00"));
                    termin.ProgressBarColor = new SolidColorBrush(Colors.Orange);
                    termin.ShouldBlink = true;
                    termin.ShouldPulseText = true;
                    termin.ItemScale = 1.3;
                    
                    if (!termin.TenMinutesWarning && differenz.TotalMinutes <= 10)
                    {
                        var warningWindow = MessageBox.Show($"? 10 MINUTEN!\n{termin.Grund}", $"Noch 10 Minuten bis {termin.Uhrzeit}");
                        // removed
                        termin.TenMinutesWarning = true;
                    }
                }
                else if (differenz <= TimeSpan.FromMinutes(15))
                {
                    termin.Background = new SolidColorBrush(Colors.Orange);
                    termin.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C00"));
                    termin.ProgressBarColor = new SolidColorBrush(Colors.Orange);
                    termin.ShouldBlink = true;
                    termin.ShouldPulseText = true;
                    termin.ItemScale = 1.25;
                    
                    if (!termin.FifteenMinutesWarning && differenz.TotalMinutes <= 15)
                    {
                        var warningWindow = MessageBox.Show($"? 15 MINUTEN!\n{termin.Grund}", $"Noch 15 Minuten bis {termin.Uhrzeit}");
                        // removed
                        termin.FifteenMinutesWarning = true;
                    }
                }
                else if (differenz <= TimeSpan.FromMinutes(30))
                {
                    termin.Background = new SolidColorBrush(Colors.Gold);
                    termin.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
                    termin.ProgressBarColor = new SolidColorBrush(Colors.Gold);
                    termin.ShouldBlink = true;
                    termin.ShouldPulseText = false;
                    termin.ItemScale = 1.15;
                }
                else if (differenz <= TimeSpan.FromHours(1))
                {
                    termin.Background = new SolidColorBrush(Colors.Gold);
                    termin.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
                    termin.ProgressBarColor = new SolidColorBrush(Colors.Gold);
                    termin.ShouldBlink = false;
                    termin.ShouldPulseText = false;
                    termin.ItemScale = 1.0;
                }
                else if (differenz <= TimeSpan.FromHours(3))
                {
                    termin.ProgressBarColor = new SolidColorBrush(Colors.LimeGreen);
                    termin.ShouldBlink = false;
                    termin.ShouldPulseText = false;
                    termin.ItemScale = 0.85;
                }
                else
                {
                    termin.ProgressBarColor = new SolidColorBrush(Colors.LimeGreen);
                    termin.ShouldBlink = false;
                    termin.ShouldPulseText = false;
                    termin.ItemScale = 0.7;
                }
            }
            
            var sortierteTermine = Termine.OrderBy(t => t.Zeitpunkt).ToList();
            for (int i = 0; i < sortierteTermine.Count; i++)
            {
                int aktuellIndex = Termine.IndexOf(sortierteTermine[i]);
                if (aktuellIndex != i)
                {
                    Termine.Move(aktuellIndex, i);
                }
            }
        }
    }
}
