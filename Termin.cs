using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Meine_Erinnerungs_app
{
    public class Termin : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        // ---- Felder, die persistiert werden ----
        // (Brushes werden NICHT direkt gespeichert - die werden aus Zeitpunkt neu berechnet)

        private string grund = "";
        public string Grund
        {
            get { return grund; }
            set { grund = value; OnPropertyChanged(); }
        }

        private string datum = "";
        public string Datum
        {
            get { return datum; }
            set { datum = value; OnPropertyChanged(); }
        }

        private string uhrzeit = "";
        public string Uhrzeit
        {
            get { return uhrzeit; }
            set { uhrzeit = value; OnPropertyChanged(); }
        }

        public DateTime Zeitpunkt { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool AlarmTriggered { get; set; }
        public bool FiveMinutesWarning { get; set; }

        // ---- Laufzeit-/Anzeige-Felder (NICHT persistiert) ----

        private Brush background = Brushes.Transparent;
        [JsonIgnore]
        public Brush Background
        {
            get { return background; }
            set { background = value; OnPropertyChanged(); }
        }

        private Brush borderBrush = Brushes.Transparent;
        [JsonIgnore]
        public Brush BorderBrush
        {
            get { return borderBrush; }
            set { borderBrush = value; OnPropertyChanged(); }
        }

        private Brush progressBarColor = Brushes.LimeGreen;
        [JsonIgnore]
        public Brush ProgressBarColor
        {
            get { return progressBarColor; }
            set { progressBarColor = value; OnPropertyChanged(); }
        }

        private double progressValue;
        [JsonIgnore]
        public double ProgressValue
        {
            get { return progressValue; }
            set { progressValue = value; OnPropertyChanged(); }
        }

        private string countdownText = "";
        [JsonIgnore]
        public string CountdownText
        {
            get { return countdownText; }
            set { countdownText = value; OnPropertyChanged(); }
        }

        private bool shouldBlink;
        [JsonIgnore]
        public bool ShouldBlink
        {
            get { return shouldBlink; }
            set { shouldBlink = value; OnPropertyChanged(); }
        }

        private bool shouldPulseText;
        [JsonIgnore]
        public bool ShouldPulseText
        {
            get { return shouldPulseText; }
            set { shouldPulseText = value; OnPropertyChanged(); }
        }

        private double itemScale = 1.0;
        [JsonIgnore]
        public double ItemScale
        {
            get { return itemScale; }
            set { itemScale = value; OnPropertyChanged(); }
        }
    }
}