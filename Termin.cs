using System;
using System.Windows.Media;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Meine_Erinnerungs_app
{
    // die Termin Klasse
    public class Termin : INotifyPropertyChanged
    {
        public string Grund { get; internal set; } // was ist der Grund
        public string Datum { get; internal set; } // welches Datum
        public string Uhrzeit { get; internal set; } // Uhrzeit halt
        public DateTime Zeitpunkt { get; internal set; } // wann genau der Termin ist
        
        
        [JsonIgnore] // nicht speichern das ist nur fürs aussehen
        public Brush Background { get; internal set; } // Hintergrundfarbe
        [JsonIgnore]
        public Brush BorderBrush { get; internal set; } // Randfarbe
        
        
        private string _countdownText; // der Text für den Countdown
        [JsonIgnore]
        public string CountdownText // was angezeigt wird
        {
            get => _countdownText;
            set
            {
                _countdownText = value;
                OnPropertyChanged(nameof(CountdownText));
            }
        }
        
        
        private double _progressValue;
        [JsonIgnore]
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }
        
        private Brush _progressBarColor;
        [JsonIgnore]
        public Brush ProgressBarColor
        {
            get => _progressBarColor;
            set
            {
                _progressBarColor = value;
                OnPropertyChanged(nameof(ProgressBarColor));
            }
        }
        
        private bool _shouldBlink;
        [JsonIgnore]
        public bool ShouldBlink
        {
            get => _shouldBlink;
            set
            {
                _shouldBlink = value;
                OnPropertyChanged(nameof(ShouldBlink));
            }
        }
        
        private double _itemScale = 1.0;
        [JsonIgnore]
        public double ItemScale
        {
            get => _itemScale;
            set
            {
                _itemScale = value;
                OnPropertyChanged(nameof(ItemScale));
            }
        }
        
        private bool _shouldPulseText;
        [JsonIgnore]
        public bool ShouldPulseText
        {
            get => _shouldPulseText;
            set
            {
                _shouldPulseText = value;
                OnPropertyChanged(nameof(ShouldPulseText));
            }
        }
        
        
        public bool AlarmTriggered { get; internal set; }
        public bool FiveMinutesWarning { get; internal set; }
        public bool TenMinutesWarning { get; internal set; }
        public bool FifteenMinutesWarning { get; internal set; }
        public bool ThirtyMinutesWarning { get; internal set; } // 30 Min Warnung
        public bool SixtyMinutesWarning { get; internal set; } // 60 Min Warnung
        
        public DateTime CreatedAt { get; internal set; } // wann wurde der Termin erstellt

        public event PropertyChangedEventHandler PropertyChanged; // damit die UI sich updatet
        
        // wird aufgerufen wenn sich was ändert
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


