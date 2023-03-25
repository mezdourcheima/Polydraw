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
    /// Logique d'interaction pour BrushWindow.xaml
    /// </summary>
    public partial class BrushWindow : Window
    {
        public VisualBrush BackgroundBrush { get; set; }

        public BrushWindow()
        {
            InitializeComponent();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            bwGrid.Background = BackgroundBrush;
            this.Width = Application.Current.MainWindow.ActualWidth / 2;
            this.Height = Application.Current.MainWindow.ActualHeight / 2;
        }
    }
}
