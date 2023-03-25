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
using System.Windows.Shapes;
using System.Reflection;

namespace tuto_epic
{
    /// <summary>
    /// Interaction logic for ColorPickerWindow.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        private List<Rectangle> patches { get; set; } = new List<Rectangle>();

        private int prevIndex = 0;
        public bool OK { get; set; } = false;

        public SolidColorBrush SelectedFillBrush { get; set; } = Brushes.White;
        public SolidColorBrush SelectedOutBrush { get; set; } = Brushes.Black;

        private SolidColorBrush sampleFillBrush = new SolidColorBrush();
        private SolidColorBrush sampleOutBrush = new SolidColorBrush();

        public ColorPickerWindow()
        {
            InitializeComponent();
        }

        public void ExtractColors()                                                     //create patches from system preset colors                                                
        {
            Type type = typeof(System.Windows.Media.Brushes);                           //gets type of object as variable
            PropertyInfo[] properties = type.GetProperties();                           //loads all properties into array (in this case static brushes)
            int indexOfTransp = 0;

            foreach (PropertyInfo property in properties)
            {
                BrushConverter converter = new BrushConverter();                        //needed to convert names back to brushes
                SolidColorBrush colorBrush = converter.ConvertFromString(property.Name) as SolidColorBrush; //string property name is converted to brush

                Rectangle rec = new Rectangle()
                {
                    Fill = colorBrush,
                    Width = 20,
                    Height = 20,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };

                rec.Tag = property.Name;                                                //for later identifcation, corresponding rectangle is tagged with brush name

                patches.Add(rec);                                                       //rectangles are stored in list
                if (colorBrush == Brushes.Transparent)
                {
                    indexOfTransp = patches.Count - 1;                                  //marks index of transparent
                }
            }
            int x = 0;
            int y = 0;
            int incrementX = 0;
            int incrementY = 0;

            Rectangle temp = patches[indexOfTransp];                                    //swap transparent patch with last patch (transparent patch becomes last)
            patches[indexOfTransp] = patches[patches.Count - 1];
            patches[patches.Count - 1] = temp;

            foreach (Rectangle rec in patches)
            {
                Canvas.SetLeft(rec, x - incrementX);                                    //positioning of rectangles
                Canvas.SetTop(rec, y - incrementY);
                x += 20;
                incrementX++;
                if (x % 220 == 0)
                {
                    x = 0;
                    y += 20;
                    incrementX = 0;
                    incrementY++;
                }
            }
        }

        private void ColorPickerCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            ExtractColors();
            OK = false;

            foreach (Rectangle rec in patches)
            {
                ColorPickerCanvas.Children.Add(rec);
                rec.MouseDown += Rec_MouseDown;
            }
            TBoxR.Text = SelectedFillBrush.Color.R.ToString();
            //TBoxR.GetBindingExpression(TextBox.TextProperty).UpdateSource();    //update binding between TextBoxes and Sliders - resolved in control (XAML) binding with UpdateSourceTrigger=PropertyChanged}"
            TBoxG.Text = SelectedFillBrush.Color.G.ToString();
            TBoxB.Text = SelectedFillBrush.Color.B.ToString();
            UpdateSampleFromSliders();                                          //restore previously selected color
            sampleRec.Stroke = SelectedOutBrush;
        }

        private void UpdateSampleFromSliders()                                  //Color of sample rectangle is updated based on Sliders values
        {
            byte r = Convert.ToByte(SliderR.Value);
            byte g = Convert.ToByte(SliderG.Value);
            byte b = Convert.ToByte(SliderB.Value);
            Color color = Color.FromRgb(r, g, b);
            if ((bool)FillRB.IsChecked)
            {
                sampleFillBrush.Color = color;
                sampleRec.Fill = sampleFillBrush;
            }
            if ((bool)OutRB.IsChecked)
            {
                sampleOutBrush.Color = color;
                sampleRec.Stroke = sampleOutBrush;
            }
            ColorDescrTBlock.Text = "Custom";
            patches[prevIndex].StrokeThickness = 1;                                    //deselect preset
            patches[prevIndex].StrokeDashArray = null;
        }

        private void UpdateSlidersFromSample()
        {
            SolidColorBrush brush = null; ;
            if ((bool)FillRB.IsChecked)
            {
                brush = sampleRec.Fill as SolidColorBrush;
            }
            if ((bool)OutRB.IsChecked)
            {
                brush = sampleRec.Stroke as SolidColorBrush;
            }
            byte r = Convert.ToByte(brush.Color.R);
            byte g = Convert.ToByte(brush.Color.G);
            byte b = Convert.ToByte(brush.Color.B);

            TBoxR.Text = r.ToString();
            TBoxG.Text = g.ToString();
            TBoxB.Text = b.ToString();
        }

        private void SliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSampleFromSliders();
        }

        private void SliderG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSampleFromSliders();
        }

        private void SliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSampleFromSliders();
        }

        private void Rec_MouseDown(object sender, MouseButtonEventArgs e)
        {
            patches[prevIndex].StrokeDashArray = null;
            patches[prevIndex].StrokeThickness = 1;
            Rectangle rec = sender as Rectangle;
            rec.StrokeThickness = 3;
            rec.StrokeDashArray = new DoubleCollection() { 1, 1, 1, 1 };
            SolidColorBrush brush = (SolidColorBrush)rec.Fill;
            Color color = brush.Color;
            TBoxR.Text = color.R.ToString();                                //sliders update automatically (see two way biding + UpdateSourceTrigger)
            TBoxG.Text = color.G.ToString();
            TBoxB.Text = color.B.ToString();

            ColorDescrTBlock.Text = rec.Tag.ToString();
            if ((bool)FillRB.IsChecked)
                sampleRec.Fill = brush;
            if ((bool)OutRB.IsChecked)
                sampleRec.Stroke = brush;
            prevIndex = patches.IndexOf(rec);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ColorPickerCanvas.Children.Clear();
            patches[prevIndex].StrokeThickness = 1;
            patches[prevIndex].StrokeDashArray = null;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OK = true;
            SelectedFillBrush = sampleRec.Fill as SolidColorBrush;
            SelectedOutBrush = sampleRec.Stroke as SolidColorBrush;
            Close();
        }

        private void FillRB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
                UpdateSlidersFromSample();
        }

        private void OutRB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
                UpdateSlidersFromSample();
        }
    }
}
