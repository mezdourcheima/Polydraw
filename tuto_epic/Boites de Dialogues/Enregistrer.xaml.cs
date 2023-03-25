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
    /// Logique d'interaction pour Enregistrer.xaml
    /// </summary>
    public partial class Enregistrer : Window
    {
        public Enregistrer()
        {
            InitializeComponent();
        }
        private void Btn_Click_Annuler(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Btn_Click_Non(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_Click_Oui(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}
