using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Meine_Erinnerungs_app
{
    public class Termin : INotifyPropertyChanged
    {
        private string _grund;
        private DateTime _terminZeit;
        private Brush _backgroundColor;
        private Brush _progressBarColor;
        private double _fortschritt;

        public string Grund
        {
            get => _grund;
            set
            {
                _grund = value;
                OnPropertyChanged(nameof(Grund));
            }
        }

        public DateTime TerminZeit
        {
            get => _terminZeit;
            set
            {
                _terminZeit = value;
                OnPropertyChanged(nameof(TerminZeit));
                OnPropertyChanged(nameof(TerminZeitString));
            }
        }

        public string TerminZeitString => TerminZeit.ToString("dd.MM.yyyy HH:mm");

        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public Brush ProgressBarColor
        {
            get => _progressBarColor;
            set
            {
                _progressBarColor = value;
                OnPropertyChanged(nameof(ProgressBarColor));
            }
        }

        public double Fortschritt
        {
            get => _fortschritt;
            set
            {
                _fortschritt = value;
                OnPropertyChanged(nameof(Fortschritt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Termin()
        {
            BackgroundColor = new SolidColorBrush(Colors.White);
            ProgressBarColor = new SolidColorBrush(Colors.Green);
            Fortschritt = 0;
        }
    }
}
