using System;
using System.Windows;

namespace Meine_Erinnerungs_app
{
    public partial class AlarmWindow : Window
    {
        private Termin _termin;

        public AlarmWindow(Termin termin)
        {
            InitializeComponent();
            _termin = termin;
            
            GrundText.Text = termin.Grund;
            ZeitText.Text = $"{termin.Datum} um {termin.Uhrzeit}";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}