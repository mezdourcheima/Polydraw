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
    public partial class Polygone : Window
    {
        public bool OK { get; set; }
        public bool ThicknessOnly { get; set; }
        public String N { get; private set; } = "Polygone";

        public double S { get; private set; } = 100;
        public double R { get; private set; } = 100;
        public double C { get; private set; } = 0.5;
        public Polygone()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OK = false;

            if (ThicknessOnly)
            {
                sgTextBox.IsEnabled = false;
                rTextBox.IsEnabled = false;
                sTextBox.Focus();
            }
            else
            {
                sgTextBox.Focus();
            }
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nTextBox.IsEnabled)
                    N = nTextBox.Text;
                if (sgTextBox.IsEnabled)
                    S = double.Parse(sgTextBox.Text);
                if (rTextBox.IsEnabled)
                    R = double.Parse(rTextBox.Text);
                if (sTextBox.IsEnabled)
                    C = double.Parse(sTextBox.Text);


                if (S < 2 || R < 2 || C < 0)
                {
                    throw (new ArgumentException("Invalid input."));
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
