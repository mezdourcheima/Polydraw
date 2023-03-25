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
    public partial class DimensionPolygone : Window
    {
        public bool OK { get; set; }
        public bool ThicknessOnly { get; set; }
        public bool RadiusEnable { get; set; }
        public double W { get; private set; } = 100;
        public double H { get; private set; } = 100;
        public double S { get; private set; } = 0.5;
        public double R { get; set; } = 0;
        public DimensionPolygone()
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
                rTextBox.IsEnabled = false;
                sTextBox.Focus();
            }
            else
            {
                rTextBox.IsEnabled = RadiusEnable;
                wTextBox.Focus();
            }
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (wTextBox.IsEnabled)
                    W = double.Parse(wTextBox.Text);
                if (hTextBox.IsEnabled)
                    H = double.Parse(hTextBox.Text);
                if (sTextBox.IsEnabled)
                    S = double.Parse(sTextBox.Text);
                if (rTextBox.IsEnabled)
                    R = double.Parse(rTextBox.Text);

                if (W < 2 || H < 2 || S < 0)
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

    }
}
