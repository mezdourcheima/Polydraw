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
    /// Logique d'interaction pour Exporter.xaml
    /// </summary>
    /// 
    public partial class RotationAll : Window
    {
        public bool OK { get; set; }
        public bool ThicknessOnly { get; set; }
        public bool RadiusEnable { get; set; }
        public double S { get; private set; } = 90;
        public RotationAll()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OK = false;

            if (ThicknessOnly)
            {
                sTextBox.Focus();
            }

        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (sTextBox.IsEnabled)
                    S = double.Parse(sTextBox.Text);


                if (S < 0)
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
    }
}
