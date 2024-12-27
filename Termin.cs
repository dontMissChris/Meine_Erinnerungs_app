using System;
using System.Windows.Media;

namespace Meine_Erinnerungs_app
{
    public class Termin
    {
        public string Grund { get; internal set; }
        public string Datum { get; internal set; }
        public string Uhrzeit { get; internal set; }
        public DateTime Zeitpunkt { get; internal set; }
        public SolidColorBrush Background { get; internal set; }
        public bool AlarmTriggered { get; internal set; }
        public bool FiveMinutesWarning { get; internal set; }
        public bool FifteenMinutesWarning { get; internal set; }
        public bool ThirtyMinutesWarning { get; internal set; }
        public bool SixtyMinutesWarning { get; internal set; }
    }
}