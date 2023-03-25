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
    /// Logique d'interaction pour Cercle.xaml
    /// </summary>
    /// public bool OK { get; set; }

    public partial class Cercle : Window
    {
        public bool OK { get; set; }
        public bool ThicknessOnly { get; set; }
        public bool RadiusEnable { get; set; }
        public String N { get; private set; } = "Cercle"; //nom
        public double W { get; private set; } = 100;
        public double H { get; private set; } = 100;
        public double S { get; private set; } = 0.5;
        public double R { get; set; } = 0;
        public Cercle()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OK = false;

            if (ThicknessOnly)
            {
                wTextBox.IsEnabled = false;
                hTextBox.IsEnabled = false;
                nTextBox.IsEnabled = false;
                sTextBox.Focus();
            }
            else
            {
                wTextBox.Focus();
            }
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nTextBox.IsEnabled)
                    N = nTextBox.Text;
                if (wTextBox.IsEnabled)
                    W = double.Parse(wTextBox.Text);
                if (hTextBox.IsEnabled)
                    H = double.Parse(hTextBox.Text);
                if (sTextBox.IsEnabled)
                    S = double.Parse(sTextBox.Text);


                if (W < 2 || H < 2 || S < 0)
                {
                    throw (new ArgumentException("Entrée invalide."));
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
        private void Button_Click_Annuler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void sTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

