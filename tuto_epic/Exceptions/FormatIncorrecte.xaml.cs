using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tuto_epic
{
    public partial class FormatIncorrecte : Window
    {
        public FormatIncorrecte()
        {
            InitializeComponent();
        }

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
