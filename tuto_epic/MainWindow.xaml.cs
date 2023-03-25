using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Microsoft.Win32;


namespace tuto_epic
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShapeManager manager;

        private Int32Collection gridSpacingData = new Int32Collection() { 5, 10, 15, 20, 25, 30, 35, 40, 50, 60, 70, 80, 100 };


        private Polyline polyLine;
        private Line tempLine;
        private Line oldLine;

        // Zoom
        private Double zoomMax = 5;
        private Double zoomMin = 0.5;
        private Double zoomSpeed = 0.001;
        private Double zoom = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void depX(object sender, MouseButtonEventArgs e)
        {

            TextBox textbox = sender as TextBox;
            double lrg = manager.SelectedShape.ActualHeight;
            textbox.Text = lrg.ToString("0.0");
        }
        private void depY(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            double lrg = manager.SelectedShape.ActualWidth;
            textbox.Text = lrg.ToString("0.0");
        }

        /***************LINK AIDE********************/
      
        private void Link_Click (object sender, RoutedEventArgs e)
        {
            Aide add = new Aide();
            add.ShowDialog();
            if (add.DialogResult == true)
            System.Diagnostics.Process.Start("https://yara201.github.io/polydraw/");
        }


        /***************LINK Probleme********************/
        private void SignaleProb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://yara201.github.io/polydraw/");
        }

        /********************ZOOM***************************/
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            var scaler = Itc.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                // Currently no zoom, so go instantly to max zoom.
                Itc.LayoutTransform = new ScaleTransform(1.5, 1.5);
            }
            else
            {
                double curZoomFactor = scaler.ScaleX;

                if (scaler.HasAnimatedProperties)
                {

                    scaler.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    scaler.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                }

                if (curZoomFactor < 2.0)
                {
                    scaler.ScaleX += 0.5;
                    scaler.ScaleY += 0.5;
                }

            }
        }
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {

                Enregistrer enreg = new Enregistrer();
                enreg.ShowDialog();
                if (enreg.DialogResult == false) //si on ne veut pas enregistrer, on supprime tous
                {
                manager.Shapes.Clear();
                manager.CombiPair.Clear();
                manager.SelectedShape = null;
                manager.Status = "";

            }
                //sinon on enregistre puis on supprime
                else if (enreg.DialogResult == true)
                {
                    Save();
                manager.Shapes.Clear();
                manager.CombiPair.Clear();
                manager.SelectedShape = null;
                manager.Status = "";

            }
                else
                {
                    enreg.Close();
                }

        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            var scaler = Itc.LayoutTransform as ScaleTransform;

            if (scaler == null)
            {
                // Currently no zoom, so go instantly to max zoom.
                Itc.LayoutTransform = new ScaleTransform(1.5, 1.5);
            }
            else
            {
                double curZoomFactor = scaler.ScaleX;


                if (scaler.HasAnimatedProperties)
                {


                    scaler.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    scaler.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                }

                if (curZoomFactor > 0.0)
                {
                    scaler.ScaleX -= 0.2;
                    scaler.ScaleY -= 0.2;
                }
            }
        }

        // Zoom on Mouse wheel
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += zoomSpeed * e.Delta; // Ajust zooming speed (e.Delta = Mouse spin value )
            if (zoom < zoomMin) { zoom = zoomMin; } // Limit Min Scale
            if (zoom > zoomMax) { zoom = zoomMax; } // Limit Max Scale

            Point mousePos = e.GetPosition(Itc);

            if (zoom > 1)
            {
                Itc.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y); // transform Canvas size from mouse position
            }
            else
            {
                Itc.RenderTransform = new ScaleTransform(zoom, zoom); // transform Canvas size
            }
        }


        /************************************************************/

        private void Polygone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void duplicate(object sender, RoutedEventArgs e)
        {
            manager.dupliquer(sender, e);
        }
        public void SelectColorMenu_Click(object sender, RoutedEventArgs e)
        {
            Shape s = manager.SelectedShape;
            ColorPickerWindow cpw = new ColorPickerWindow();
            if (s.Fill != null)                                                    //for finished polyline
                cpw.SelectedFillBrush = s.Fill as SolidColorBrush;                //restore last selection
            cpw.SelectedOutBrush = s.Stroke as SolidColorBrush;
            cpw.Owner = Application.Current.MainWindow;
            cpw.ShowDialog();
            if (cpw.OK)
            {
                if (s.Fill != null)
                    s.Fill = cpw.SelectedFillBrush;
                s.Stroke = cpw.SelectedOutBrush;
            }
        }
        private void K_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.D:
                        manager.duplicate(sender, e);
                        break;
                    case Key.X:
                        manager.Supprimer(sender, e);
                        break;
                    case Key.P:
                        manager.colorer(sender, e);
                        break;
                    case Key.S:
                        Save_Click(sender, e);
                        break;
                }
            }
        }
        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)                               //exit application
        {
            Quitterpop quit = new Quitterpop();// add text here
            quit.ShowDialog();
            if (quit.DialogResult == true) //si on veut quitter on doit enregistrer
            {
                Enregistrer enreg = new Enregistrer();
                enreg.ShowDialog();
                if (enreg.DialogResult == false) //si on ne veut pas enregistrer, on quitte
                {
                    Close();
                }
                //sinon on enregistre puis on quitte
                else if (enreg.DialogResult == true)
                {
                    Save();
                    Close();
                }
                else //sinon on ne quite pas -annuler-
                {
                    enreg.Close();
                    quit.Close();
                }
            }


        }
        private void Aide_Click(object sender, RoutedEventArgs e)                               //exit application
        {
            Aide aid = new Aide();// add text here
            aid.ShowDialog();

        }
        private void mm_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            manager = new ShapeManager();
            DataContext = manager;
            manager.ClickedChanged += Manager_ClickedChanged;       //attach event handler to Clicked property changed event in manager
            Itc.MouseLeftButtonUp += Itc_MouseLeftButtonUp;
           // SpacingCombo.ItemsSource = gridSpacingData;
           // SpacingCombo.SelectedIndex = 3;
          //  manager.UpdateGrid(Itc.ActualWidth, Itc.ActualHeight, (int)SpacingCombo.SelectedItem);

        }

        private void Manager_ClickedChanged(object sender, EventArgs e)// * If shape was Clicked in manager event handler: initiate drag - move
        {
            if (manager.PolyLineMode)                                  //if PolyLine is being drawn, do not interrupt
                return;
            Itc.MouseMove += Itc_MouseMove;                             //otherwise start observing mouse movement **                                         
            Itc.Cursor = Cursors.Hand;
        }

        private void Itc_MouseMove(object sender, MouseEventArgs e)         // **
        {
            if (manager.PolyLineMode && tempLine != null)                   //if in polyline mode
            {
                Point point = new Point();                                  //then draw temporary lines of polyline
                if (manager.Snap)
                {
                    point = manager.SnapToGrid(e.GetPosition(Itc).X, e.GetPosition(Itc).Y);     //snap to grid
                }
                else
                {
                    point.X = e.GetPosition(Itc).X;                                                 //no snap
                    point.Y = e.GetPosition(Itc).Y;
                }
                tempLine.X2 = point.X;
                tempLine.Y2 = point.Y;
                double distance = Point.Subtract(new Point(tempLine.X1, tempLine.Y1), new Point(tempLine.X2, tempLine.Y2)).Length;
                manager.Status = distance.ToString();
            }
            else if (!manager.PolyLineMode)                                          //else drag - move selected shape
            {
                manager.Status = "Drag active";
                manager.DragShape(e.GetPosition(Itc).X, e.GetPosition(Itc).Y);      //initiate drag - move process
            }

        }

        private void Itc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (manager.PolyLineMode)
                return;
            Itc.MouseMove -= Itc_MouseMove;                                 //finish drag - move, drop shape on position
            Itc.Cursor = Cursors.Arrow;

            if (manager.SelectedShape is System.Windows.Shapes.Path)
            {
                manager.RecalculateGeometryBounds();
            }
        }


        private void RectMenu_Click(object sender, RoutedEventArgs e)       //Insert rectangle
        {
            manager.InsertShape("rectangle");
        }

        private void EllMenu_Click(object sender, RoutedEventArgs e)        //Insert ellipse
        {
            manager.InsertShape("ellipse");
        }

        private void PolyLineMenu_Click(object sender, RoutedEventArgs e)   //start drawing polyLine
        {
            manager.InsertShape("polyline");
            int count = manager.Shapes.Count;
            if (count > 0 && manager.Shapes[count - 1] is Polyline)
            {
                manager.PolyLineMode = true;
                polyLine = manager.Shapes[count - 1] as Polyline;           //polyline   starting
                Itc.Cursor = Cursors.Cross;
                Itc.MouseLeftButtonDown += Itc_MouseLeftButtonDown;             //attach handler to left mouse button click (add point to polyLine)
                Itc.MouseRightButtonUp += Itc_MouseRightButtonUp;                   //attach handler to right mouse button click event (add last point to polyLine)    
            }
        }
        private void PolygonMenu_Click(object sender, RoutedEventArgs e)   //start drawing polyLine
        {
            manager.InsertShape("Polygone");
            int count = manager.Shapes.Count;
            if (count > 0 && manager.Shapes[count - 1] is Polyline)
            {
                manager.PolyLineMode = true;
                polyLine = manager.Shapes[count - 1] as Polyline;           //polyline   starting
                Itc.Cursor = Cursors.Cross;

                Itc.MouseRightButtonUp += Itc_MouseRightButtonUp;                   //attach handler to right mouse button click event (add last point to polyLine)    
            }

        }
        private void RotateMenu_Click(object sender, RoutedEventArgs e)   //start drawing polyLine
        {
            manager.InsertShape("Rotate");
            int count = manager.Shapes.Count;
            if (count > 0 && manager.Shapes[count - 1] is Polyline)
            {
                manager.PolyLineMode = true;
                polyLine = manager.Shapes[count - 1] as Polyline;           //polyline   starting
                Itc.Cursor = Cursors.Cross;

                Itc.MouseRightButtonUp += Itc_MouseRightButtonUp;                   //attach handler to right mouse button click event (add last point to polyLine)    
            }

        }
        private void Itc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Itc.MouseMove -= Itc_MouseMove;
            if (oldLine != null)
            {
                manager.Shapes.Remove(oldLine);                                             //delete temporary line
            }

            Point point = new Point();
            if (manager.Snap)
            {
                point = manager.SnapToGrid(e.GetPosition(Itc).X, e.GetPosition(Itc).Y);     //snap to grid
            }
            else
            {
                point = new Point(e.GetPosition(Itc).X, e.GetPosition(Itc).Y);                                                 //no snap
            }
            polyLine.Points.Add(point);
            tempLine = new Line() { Stroke = Brushes.LightPink, StrokeThickness = 1 };       //new temporary line
            manager.Shapes.Add(tempLine);
            tempLine.X1 = point.X;
            tempLine.Y1 = point.Y;
            tempLine.X2 = point.X;                                                  //initially endpoint of temporary line = startpoint, on mouse move endpoint position updates
            tempLine.Y2 = point.Y;
            Itc.MouseMove += Itc_MouseMove;
            oldLine = tempLine;
        }

        private void Itc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Itc.MouseLeftButtonDown -= Itc_MouseLeftButtonDown;                                 //finish polyline creation
            Itc.MouseRightButtonUp -= Itc_MouseRightButtonUp;
            Itc.MouseMove -= Itc_MouseMove;
            Itc.Cursor = Cursors.Arrow;
            if (oldLine != null)
            {
                manager.Shapes.Remove(oldLine);
            }
            manager.SelectedShape = null;                                                               //prevent conflict with drag
            manager.PolyLineMode = false;
            Point firstPoint = polyLine.Points[0];
            int lastIndex = polyLine.Points.Count - 1;
            Point endPoint = polyLine.Points[lastIndex];
            if (firstPoint == endPoint)
            {
                Creation crt = new Creation();
                crt.ShowDialog();
                manager.Shapes.Remove(polyLine);
                manager.ConvertPolyLineToGeometry(polyLine, true);
            }
            else
            {
                CreePolyLine crpl = new CreePolyLine();
                crpl.ShowDialog();
                manager.Shapes.Remove(polyLine);
                manager.ConvertPolyLineToGeometry(polyLine, false);
            }
        }



        private void ShowButton_Click(object sender, RoutedEventArgs e)                             //preview visual brush in separate window
        {
            VisualBrush visualBrush = new VisualBrush();
            visualBrush.Visual = Itc;
            BrushWindow bw = new BrushWindow();
            bw.BackgroundBrush = visualBrush;
            bw.ShowDialog();
        }
        private void DeselectMenu_Click(object sender, RoutedEventArgs e)
        {
            manager.SelectedShape = null;
            manager.Status = "";

        }

        private void DeleteAllMenu_Click(object sender, RoutedEventArgs e)                          //delete all shapes
        {
            Supprimer supp = new Supprimer();
            supp.ShowDialog();
            if (supp.DialogResult == true)
            {
                manager.Shapes.Clear();
                manager.CombiPair.Clear();
                manager.SelectedShape = null;
                manager.Status = "";
            }
            else
            {
                supp.Close();

            }
        }

        private void Delete(object sender, RoutedEventArgs e)                          //delete shape
        {
            Supprimer supp = new Supprimer();
            supp.ShowDialog();
            if(supp.DialogResult==true)
            {
                manager.Shapes.Remove(manager.SelectedShape);

            }
            else
            {
                supp.Close();
            }

        }
        private void Cut(object sender, RoutedEventArgs e)                          //Cut shape
        {
            manager.Shapes.Remove(manager.SelectedShape);
            manager.dupliquer(sender, e);


        }
        public void Saveas(object sender, RoutedEventArgs e)       // enregistrer sous ( png / jpg )
        {
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Jpg (*.jpg)|*.jpg |Png (*.png)|*.png";

            bool? result = sfd.ShowDialog();
            if (result == true)
            {
                string path = sfd.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)ActualWidth,
                    (int)Itc.ActualWidth, 1 / 96, 1 / 96, PixelFormats.Pbgra32);
                bmp.Render(Itc);
                BitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
                fs.Close();
            }
        }
    public void Save()           //this save t3amer un fichier .poly berk sans l'ouvrir 
                                                                 // lazem bitmapdecoder
        {
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Poly (*.poly)|*.poly";

            bool? result = sfd.ShowDialog();
            if (result == true)
            {
              
                string path = sfd.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)ActualWidth,
                    (int)Itc.ActualWidth, 1 / 96, 1 / 96, PixelFormats.Pbgra32);
                bmp.Render(Itc);
                BitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
                
                fs.Close();

            }
        }
        public void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void import_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog op = new OpenFileDialog();
            ImageBrush brush = new ImageBrush();  //1
            op.Title = "Select a picture";
            op.Filter = "*.jpg,.png,.jpeg|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              " (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                brush.ImageSource = new BitmapImage(new Uri(op.FileName, UriKind.Relative)); //2
                Itc.Background = brush;//3

            }

        }

        public void Load(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Poly (*.poly)|*.poly";
            BitmapSource resultImage = null; //1

            bool? result = sfd.ShowDialog();
            if (result == true)
            {
                string path = sfd.FileName;
                using (Stream imSource = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BitmapDecoder decoder = new TiffBitmapDecoder(imSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    resultImage = decoder.Frames[0];
                }
                //Itc.Background = resultImage;


            }
        }

        private void UnionMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.Combine(0);
            }
            catch
            {
                ComboImpossible cmbimp = new ComboImpossible();
                cmbimp.ShowDialog();
                manager.CombiPair.Clear();
            }

        }

        private void IntersectMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.Combine(1);
            }
            catch
            {
                ComboImpossible cmbimp = new ComboImpossible();
                cmbimp.ShowDialog();
                manager.CombiPair.Clear();
            }
        }

        private void ExcludeMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.Combine(2);
            }
            catch
            {
                ComboImpossible cmbimp = new ComboImpossible();
                cmbimp.ShowDialog();
                manager.CombiPair.Clear();
            }
        }

        private void XorMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.Combine(3);
            }
            catch
            {
                ComboImpossible cmbimp = new ComboImpossible();
                cmbimp.ShowDialog();
                manager.CombiPair.Clear();
            }
        }

        private void CombiClearMenu_Click(object sender, RoutedEventArgs e)
        {
            manager.CombiPair.Clear();
            manager.CombiPair.Clear();
        }

        private void PreviewMenu_Click(object sender, RoutedEventArgs e)
        {
            manager.DisplayGrid = false;
            VisualBrush visualBrush = new VisualBrush();
            visualBrush.Visual = Itc;
            BrushWindow bw = new BrushWindow();
            bw.BackgroundBrush = visualBrush;
            manager.DisplayGrid = true;
            bw.ShowDialog();
        }
        private RotationAll rotall;

        private void RotateAllMenu_Click(object sender, RoutedEventArgs e)
        {
            rotall = new RotationAll();
            rotall.ShowDialog();
            rotall.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
            if (!rotall.OK)                                     //if values not confirmed
            {
                return;
            }
            Shape s = manager.SelectedShape;

            RotateTransform r = new RotateTransform(rotall.S, s.ActualHeight, s.ActualWidth);
            s.RenderTransform = r;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

   
