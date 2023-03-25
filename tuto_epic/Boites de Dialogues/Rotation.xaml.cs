using System;
using System.Collections.Generic;
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

namespace tuto_epic
{
    /// <summary>
    /// Logique d'interaction pour DimensionPolygone.xaml
    /// </summary>
    public partial class Rotation : Window
    {
        public bool OK { get; set; }
        public bool ThicknessOnly { get; set; }
        public bool RadiusEnable { get; set; }
        public double WR { get; private set; } = 100;
        public double HR { get; private set; } = 100;
        public double SR { get; private set; } = 0.5;
        public double RR { get; set; } = 0;
        public Rotation()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OK = false;

            if (ThicknessOnly)
            {
                wrTextBox.IsEnabled = false;
                hrTextBox.IsEnabled = false;
                rrTextBox.IsEnabled = false;
                srTextBox.Focus();
            }
            else
            {
                wrTextBox.Focus();

            }
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (wrTextBox.IsEnabled)
                    WR = double.Parse(wrTextBox.Text);
                if (hrTextBox.IsEnabled)
                    HR = double.Parse(hrTextBox.Text);
                if (srTextBox.IsEnabled)
                    SR = double.Parse(srTextBox.Text);
                if (rrTextBox.IsEnabled)
                    RR = double.Parse(rrTextBox.Text);

                if (WR < 2 || HR < 2 || SR < 0)
                {
                    throw (new ArgumentException("Entrée Invalide."));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            OK = true;
            Close();
        }
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}


