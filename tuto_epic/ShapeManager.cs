using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using Path = System.Windows.Shapes.Path;


namespace tuto_epic
{
    class ShapeManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void notify(string propertyName)                                                                  //notification interface                                
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Shape> Shapes { get; set; } = new ObservableCollection<Shape>();                //Shapes collection, bound to Canvas
        public SolidColorBrush ActualFillBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));   //Fill brush
        public SolidColorBrush ActualOutBrush { get; set; } = new SolidColorBrush(Color.FromRgb(0, 0, 0));          //Outline = stroke brush

        public ImageBrush BackgroundGrid                                                                            //grid
        {
            get { return backgroundGrid; }
            set { backgroundGrid = value; notify("BackgroundGrid"); }
        }
        private ImageBrush backgroundGrid;

        public string Status                                                                                        //Shape properties infotext
        {
            get { return status; }
            set { status = value; notify("Status"); }
        }
        private string status;

        private double x, y;
        private Cercle dwc;
        private PolyLine dwpl;
        private Rctgl dwr;
        private Polygone dwplg;
        private Rotation dwrot;
        public Shape SelectedShape { get; set; }

        private Grid grid = new Grid();

        public bool PolyLineMode { get; set; }

        public bool Snap { get; set; } = true;
        public bool DisplayGrid { get; set; } = true;

        public int Clicked                                                                                          //helper variable, serves as notification of Shape being clicked
        {
            get { return clicked; }
            set { clicked = value; OnClickedChanged(); }
        }
        private int clicked;

        public event EventHandler ClickedChanged;

        public void OnClickedChanged()                                                                              //click event is fired (see MainWindow.cs handler]
        {
            if (ClickedChanged != null)
                ClickedChanged(this, EventArgs.Empty);
        }

        private System.Windows.Shapes.Path myPath;
        private double transformX = 0;
        private double transformY = 0;

        private double originX = 0;
        private double originY = 0;

        public List<Shape> CombiPair { get; set; } = new List<Shape>();


        public void UpdateGrid(double cnvWidth, double cnvHeight, int spacing)
        {
            grid.Spacing = spacing;
            grid.Width = cnvWidth;
            grid.Height = cnvHeight;
            BackgroundGrid = grid.UdateGrid(DisplayGrid);
        }

        public void InsertShape(string option)
        {
            Shape s = null;

            if (option == "rectangle" || option == "ellipse" || option == "polyline" || option == "Polygone" || option == "Rotate")  //in case of interactively generated shapes or text, no dimension values are needed
            {
                if (option == "polyline")
                {
                    dwpl = new PolyLine();
                    dwpl.ShowDialog();
                    dwpl.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
                    if (!dwpl.OK)                                     //if values not confirmed
                    {
                        return;
                    }
                }
                if (option == "Polygone")
                {

                    dwplg = new Polygone();     //DimensionWindow is Parent centered
                    dwplg.ShowDialog();
                    dwplg.Owner = Application.Current.MainWindow;

                    if (!dwplg.OK)                                     //if values not confirmed
                    {
                        return;
                    }
                }
                if (option == "ellipse")
                {
                    dwc = new Cercle();
                    dwc.ShowDialog();
                    dwc.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
                    if (!dwc.OK)                                     //if values not confirmed
                    {
                        return;
                    }
                }
                if (option == "rectangle")
                {
                    dwr = new Rctgl();
                    dwr.ShowDialog();
                    dwr.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
                    if (!dwr.OK)                                     //if values not confirmed
                    {
                        return;
                    }
                }
                if (option == "Rotate")
                {
                    //dwrot.RadiusEnable = true;
                    dwrot = new Rotation();
                    dwrot.ShowDialog();
                    dwrot.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
                    if (!dwrot.OK)                                     //if values not confirmed
                    {
                        return;
                    }
                }

            }

            if (option == "Polygone")                  //temporary PolyLine, can result in finished polyline or closed polygon
            {
                Point[] poly = new Point[50];
                PolyDraw.Main.Polygon P = new PolyDraw.Main.Polygon();
                int ab = (int)dwplg.S;
                float ac = (float)dwplg.R;
               // double ad = (double)dwplg.A;
                P.creation(poly, ab, ac);
                Polyline myPolyline = new Polyline();
                myPolyline.Stroke = System.Windows.Media.Brushes.SlateGray;
                int str = (int)dwplg.S;
                myPolyline.StrokeThickness = str;
                myPolyline.FillRule = FillRule.EvenOdd;

                PointCollection myPointCollection2 = new PointCollection();

                int i;

                for (i = 0; i < ab; i++)
                {
                    myPointCollection2.Add(poly[i]);


                }

                poly[i] = poly[0];
                myPointCollection2.Add(poly[i]);
                myPolyline.Points = myPointCollection2;
                s = myPolyline;


            }
            else if (option == "polyline")                  //temporary PolyLine, can result in finished polyline or closed polygon
            {
                Polyline polyLine = new Polyline()
                {
                    Stroke = ActualOutBrush,
                    StrokeThickness = dwpl.S,
                    StrokeLineJoin = PenLineJoin.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round
                };
                s = polyLine;
            }
            else if (option == "rectangle")
            {
                RectangleGeometry r = new RectangleGeometry(new Rect(0, 0, dwr.W, dwr.H));
                r.RadiusX = dwr.R;
                r.RadiusY = dwr.R;

                myPath = new Path();
                myPath.Data = r;
                myPath.Fill = ActualFillBrush;
                myPath.Stroke = ActualOutBrush;
                myPath.StrokeThickness = dwr.S;
                s = myPath;
            }

            else if (option == "ellipse")
            {
                EllipseGeometry e = new EllipseGeometry(new Rect(0, 0, dwc.W, dwc.H));

                myPath = new Path();
                myPath.Data = e;
                myPath.Fill = ActualFillBrush;
                myPath.Stroke = ActualOutBrush;
                myPath.StrokeThickness = dwc.S;
                s = myPath;
            }

            else if (option == "duplicate")
            {
                s = myPath;
                option = SelectedShape.Tag.ToString();        //temporarily store original shape tag to option variable
            }

            else if (option == "Rotate")
            {
                Point[] poly = new Point[50];
                PolyDraw.Main.Polygon P = new PolyDraw.Main.Polygon();
                int ab = (int)dwrot.WR;
                float ac = (float)dwrot.HR;
                double ad = (double)dwrot.RR;
                P.creation(poly, ab, ac);
                P.Rotation(poly, ad);
                Polyline myPolyline = new Polyline();
                myPolyline.Stroke = System.Windows.Media.Brushes.SlateGray;
                int str = (int)dwrot.SR;
                myPolyline.StrokeThickness = str;
                myPolyline.FillRule = FillRule.EvenOdd;

                PointCollection myPointCollection2 = new PointCollection();

                int i;

                for (i = 0; i < ab; i++)
                {
                    myPointCollection2.Add(poly[i]);

                }

                poly[i] = poly[0];
                myPointCollection2.Add(poly[i]);
                myPolyline.Points = myPointCollection2;
                s = myPolyline;

            }

            else                                            //polygon, combination, finished polyline
            {
                s = myPath;
            }

            s.SnapsToDevicePixels = true;
            Shapes.Add(s);                                  //Add shape to observable collection
            Canvas.SetLeft(s, 0);                           //define 0, 0 position on Canvas
            Canvas.SetTop(s, 0);
            SelectedShape = s;
            s.Tag = option;
            if (option == "coller")
            {
                s.Tag = option;      //duplicate shape final tag
            }


            s.KeyDown += duplicate;
            s.MouseLeftButtonDown += S_MouseLeftButtonDown; //attach handlers to events                         
            s.ContextMenu = new ContextMenu();
            s.ContextMenu.Loaded += ContextMenu_Loaded;

            MenuItem item1 = null;
            MenuItem item2 = null;
            if ((string)s.Tag != "Polyline creé avec réussite.")                                                                          //combinable geometries need appropriate context menu items
            {
                item1 = new MenuItem() { Name = "combiAdd", Header = "Ajouter a la combinaison (Ctrl+A)" };
                item1.Click += Item1_Click;
                s.ContextMenu.Items.Add(item1);
                item2 = new MenuItem() { Name = "combiRemove", Header = "Retirer de la combinaison (Ctrl+R)" };
                item2.Click += Item2_Click;
                s.ContextMenu.Items.Add(item2);

            }
            MenuItem item3 = new MenuItem() { Name = "front", Header = "Mettre en avant (Ctrl+F)" };          //for all geomteries
            MenuItem item4 = new MenuItem() { Name = "back", Header = "Mettre en arriere plan (Ctrl+B)" };
            MenuItem item5 = new MenuItem() { Name = "repaint", Header = "Colorer (Ctrl+P)" };
            MenuItem item6 = new MenuItem() { Name = "duplicate", Header = "Dupliquer (Ctrl+D)" };
            MenuItem item7 = new MenuItem() { Name = "delete", Header = "Supprimer (Ctrl+X)" };
            MenuItem item9 = new MenuItem() { Name = "Rotate", Header = "Rotationer (Ctrl+R)" };


            item3.Click += Item3_Click;                                                             //attach handlers to context menu click events
            item4.Click += Item4_Click;
            item5.Click += Item5_Click;
            item6.Click += Item6_Click;
            item7.Click += Item7_Click;
            item9.Click += Item9_Click;

            s.ContextMenu.Items.Add(item3);
            s.ContextMenu.Items.Add(item4);
            s.ContextMenu.Items.Add(item5);
            s.ContextMenu.Items.Add(item6);
            s.ContextMenu.Items.Add(item7);
            s.ContextMenu.Items.Add(item9);

            if (s is Polyline)
            {
                Status = string.Format("{0}, Left: {1} Top: {2}", s.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape));   //info text for PolyLine
            }
            else if (s is Path)
            {
                Path path = s as Path;
                Geometry geo = path.Data;
                Status = string.Format("{0}, Left: {1} Top: {2} BoundsX: {3} BoundsY: {4}", geo.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape), geo.Bounds.X, geo.Bounds.Y);   //for rest
            }
            SelectedShape = null;
        }
        //for the new clicks
        public void duplicate(object sender, KeyEventArgs e)
        {
           
            Path p = SelectedShape as Path;
            Geometry g = p.Data; //as Geometry;
           
            Geometry clonedGeometry = g.Clone();
            myPath = new Path();
            myPath.Data = clonedGeometry;
            myPath.Fill = p.Fill;
            myPath.Stroke = p.Stroke;
            myPath.StrokeThickness = p.StrokeThickness;
            InsertShape("duplicate");

        }
        public void Supprimer(object sender, KeyEventArgs e)

        {
            Shapes.Remove(SelectedShape);
        }
        public void Open(object sender, RoutedEventArgs e)

        {
            
        }
        public void colorer(object sender, KeyEventArgs e)
        {
            Shape s = SelectedShape;
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


    private void S_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)       //on LMB click
        {
            SelectedShape = sender as Shape;                                              //identify shape
            x = e.GetPosition(SelectedShape).X;                                           //get click coordinates within shape (relative to top left corner)
            y = e.GetPosition(SelectedShape).Y;
            Clicked++;                                                                  //by changing Clicked value, MainWindow is notified, that some shape was clicked, so drag can be initiated

            if (sender is Path)                                                         //PolyLine is only temporary shape when creating finished polyline or polygon
            {
                Path path = sender as Path;                                             //identify sender
                Geometry geometry = path.Data;                                          //get his geometry
                Status = string.Format("{0}, Left: {1} Top: {2} BoundsX: {3} BoundsY: {4}", geometry.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape), geometry.Bounds.X, geometry.Bounds.Y);
            }                                                                           //code continues in MainWindow.xaml.cs *
          
        }

        public void DragShape(double X, double Y)                              //Change shape position when dragged. This method is called from MainWindow, if mouse is moved over Canvas after LMB click on Shape
        {                                                                       //X, Y are mouse pointer coordinates on Canvas
            if (SelectedShape == null) { return; }
            if (Snap)                                                           //snap to grid
            {
                Point p = SnapToGrid(X - x, Y - y);
                Canvas.SetLeft(SelectedShape, p.X);
                Canvas.SetTop(SelectedShape, p.Y);
            }
            else
            {
                Canvas.SetLeft(SelectedShape, X - x);                             //when placing Shape on Canvas, mouse position within Shape must be kept (x, y)
                Canvas.SetTop(SelectedShape, Y - y);
            }

            Path path = SelectedShape as Path;
            Geometry geometry = path.Data;
            Status = string.Format("{0}, Left: {1} Top: {2} BoundsX: {3} BoundsY: {4}", geometry.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape), geometry.Bounds.X, geometry.Bounds.Y);//update info text
        }

        public Point SnapToGrid(double X, double Y)                             //Snap to grid method. Works for Shape drag - moving or PolyLine / Polygon points placement
        {
            int boundsCorrectX = 0;
            int boundsCorrectY = 0;
            if (SelectedShape is Path)
            {
                Path path = SelectedShape as Path;
                Geometry geometry = path.Data; //as Geometry;
                boundsCorrectX = Convert.ToInt32(geometry.Bounds.X % grid.Spacing);     //in case Shape of type Path, bounding box letf / top point correction is needed
                boundsCorrectY = Convert.ToInt32(geometry.Bounds.Y % grid.Spacing);
            }

            int xFinal = 0;
            int yFinal = 0;
            int xValue = Convert.ToInt32(X);
            int yValue = Convert.ToInt32(Y);

            int xMod = xValue % grid.Spacing + boundsCorrectX;                          //in other cases bounding box correction is 0
            int yMod = yValue % grid.Spacing + boundsCorrectY;
            if (xMod < grid.Spacing / 2)                                                //snap to previous grid column
            {
                xFinal = xValue - xMod;
            }
            else
            {
                xFinal = xValue - xMod + grid.Spacing;                                  //snap to next grid column
            }
            if (yMod < grid.Spacing / 2)                                                //ditto grid line
            {
                yFinal = yValue - yMod;
            }
            else
            {
                yFinal = yValue - yMod + grid.Spacing;
            }
            Point p = new Point(xFinal, yFinal);                                        //resulting point        
            return p;
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)       //inactivate some context menu items based on combination group status
        {
            ContextMenu menu = sender as ContextMenu;

            if (PolyLineMode)
                menu.Visibility = Visibility.Hidden;                            //when finishing polyline, context menu should not appear on right mouse up, if clicked on some other shape

            Shape s = menu.PlacementTarget as Shape;                            //identify shape
            SelectedShape = s;
            Path path = s as Path;
            Geometry geometry = path.Data;
            Status = string.Format("{0}, Left: {1} Top: {2} BoundsX: {3} BoundsY: {4}", geometry.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape), geometry.Bounds.X, geometry.Bounds.Y);//update info text
            if (s is Polyline)
                return;
            if (CombiPair.Count > 1 || CombiPair.Contains(s))                   //if 2 shapes are already in group or parent shape is already present
            {
                MenuItem item = menu.Items[0] as MenuItem;
                item.IsEnabled = false;                                         //inactivate adding to group
            }
            else
            {
                MenuItem item = menu.Items[0] as MenuItem;
                item.IsEnabled = true;
            }

            if (!CombiPair.Contains(s))
            {
                MenuItem item = menu.Items[1] as MenuItem;                      //if group does not contain shape, shape cannot be removed from group
                item.IsEnabled = false;
            }
            else
            {
                MenuItem item = menu.Items[1] as MenuItem;
                item.IsEnabled = true;
            }
        }

        private void Item1_Click(object sender, RoutedEventArgs e)
        {
            CombiPair.Add(SelectedShape);
        }

        private void Item2_Click(object sender, RoutedEventArgs e)
        {
            CombiPair.Remove(SelectedShape);
        }

        private void Item3_Click(object sender, RoutedEventArgs e)
        {
            Shapes.Remove(SelectedShape);                                        //remove
            Shapes.Add(SelectedShape);                                          //then add to top
        }

        private void Item4_Click(object sender, RoutedEventArgs e)
        {
            Shapes.Remove(SelectedShape);                                       //remove
            Shapes.Insert(0, SelectedShape);                                    //then insert at bottom position
        }

        public void Item5_Click(object sender, RoutedEventArgs e)
        {
            Shape s = SelectedShape;
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

        public void SelectColorMenu_Click(object sender, RoutedEventArgs e)
        {
            Shape s = SelectedShape;
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

        public void Item6_Click(object sender, RoutedEventArgs e)
        {
            Path p = SelectedShape as Path;
            Geometry g = p.Data; //as Geometry;
            Geometry clonedGeometry = g.Clone();
            myPath = new Path();
            myPath.Data = clonedGeometry;
            myPath.Fill = p.Fill;
            myPath.Stroke = p.Stroke;
            myPath.StrokeThickness = p.StrokeThickness;
            InsertShape("duplicate");
        }

        private void Item7_Click(object sender, RoutedEventArgs e)
        {
            Shapes.Remove(SelectedShape);
            if (CombiPair.Contains(SelectedShape))
                CombiPair.Remove(SelectedShape);
        }
        public void dupliquer(object sender, RoutedEventArgs e)
        {
            Path p = SelectedShape as Path;
            Geometry g = p.Data; //as Geometry;
            Geometry clonedGeometry = g.Clone();
            myPath = new Path();
            myPath.Data = clonedGeometry;
            myPath.Fill = p.Fill;
            myPath.Stroke = p.Stroke;
            myPath.StrokeThickness = p.StrokeThickness;
            InsertShape("duplicate");
        }

        public Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X = (float)(cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y = (float)(sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        private double SignedPolygonArea(Point[] polygon)
        {
            // Add the first point to the end.
            int num_points = polygon.Length;
            Point[] pts = new Point[num_points + 1];
            polygon.CopyTo(pts, 0);
            pts[num_points] = polygon[0];

            // Get the areas.
            double area = 0;
            for (int i = 0; i < num_points; i++)
            {
                area += ((pts[i + 1].X - pts[i].X) * (pts[i + 1].Y + pts[i].Y) / 2);
            }

            // Return the result.
            return area;
        }

        private double PolygonArea(Point[] polygon)
        {
            // Return the absolute value of the signed area.
            // The signed area is negative if the polyogn is
            // oriented clockwise.
            return Math.Abs(SignedPolygonArea(polygon));
        }
        public Point FindCentroid(Point[] polygone)
        {
            // Add the first point at the end of the array.
            int num_points = polygone.Length;
            Point[] pts = new Point[num_points + 1];
            polygone.CopyTo(pts, 0);
            pts[num_points] = polygone[0];

            // Find the centroid.
            float X = 0;
            float Y = 0;
            float second_factor;
            for (int i = 0; i < num_points; i++)
            {
                second_factor = (float)(pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y);
                X += (float)(pts[i].X + pts[i + 1].X) * second_factor;
                Y += (float)(pts[i].Y + pts[i + 1].Y) * second_factor;
            }

            // Divide by 6 times the polygon's area.
            double polygon_area = PolygonArea(polygone);
            X /= (float)(6 * polygon_area);
            Y /= (float)(6 * polygon_area);

            // If the values are negative, the polygon is
            // oriented counterclockwise so reverse the signs.
            if (X < 0)
            {
                X = -X;
                Y = -Y;
            }

            return new Point(X, Y);
        }
        public Point[] Rotation(Point[] polygon)
        {

            Point p = FindCentroid(polygon); //centerPoint
            for (int a = 0; a < polygon.Length; a++)
            {
                polygon[a] = RotatePoint(polygon[a], p, 180);
                //Console.WriteLine(Corners1[a].X.ToString() + Corners1[a].Y.ToString());
            }
            return polygon;
        }
        private RotationAll rotall;

        private void Item9_Click(object sender, RoutedEventArgs e)
        {


            rotall = new RotationAll();
            rotall.ShowDialog();
            rotall.Owner = Application.Current.MainWindow;      //DimensionWindow is Parent centered
            if (!rotall.OK)                                     //if values not confirmed
            {
                return;
            }
            Shape s = SelectedShape;

            RotateTransform r = new RotateTransform(rotall.S, s.ActualHeight, s.ActualWidth);
            s.RenderTransform = r;
        }
        public void ConvertPolyLineToGeometry(Polyline polyLine, bool closed) //Convert PolyLine to polygon, if shaped was closed during creation
        {
            PathGeometry pathGeom = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.IsClosed = closed;
            if (closed)                                             //for polygon
            {
                int index = 0;
                foreach (Point point in polyLine.Points)
                {
                    if (index == 0)                                 //define first point of polyline as startpoint
                        figure.StartPoint = point;
                    else if (index == polyLine.Points.Count - 1)    //exclude last point, identical with startpoint
                    {
                        break;
                    }
                    else
                    {
                        figure.Segments.Add(new LineSegment((point), true));    //other points are ok
                    }
                    index++;
                }
                pathGeom.Figures.Add(figure);

                myPath = new Path();
                myPath.Data = pathGeom;
                myPath.Stroke = ActualOutBrush;
                myPath.StrokeThickness = polyLine.StrokeThickness;
                myPath.Fill = ActualFillBrush;
                InsertShape("polygon");
            }
            else
            {                                                       //for finished polyline
                int index = 0;
                foreach (Point point in polyLine.Points)
                {
                    if (index == 0)                                 //define first point of polyline as startpoint
                        figure.StartPoint = point;
                    else
                    {
                        figure.Segments.Add(new LineSegment((point), true));    //other points are ok
                    }
                    index++;
                }
                pathGeom.Figures.Add(figure);

                myPath = new Path();
                myPath.Data = pathGeom;
                myPath.Stroke = ActualOutBrush;
                myPath.StrokeThickness = polyLine.StrokeThickness;
                InsertShape("Polyline creé avec reussite. ");
            }
        }

        public void RecalculateGeometryBounds()                   //recalculate geometry after drag - move. convert Canvas Left and Top to geometry Bounds.X , Y.
        {
            Geometry geometry = null;

            Path path = SelectedShape as Path;
            geometry = path.Data; //as Geometry;
            if (geometry is PathGeometry)                       //polygon or finishedpolyline are calculated point by point
            {
                double deltaX = Canvas.GetLeft(SelectedShape);
                double deltaY = Canvas.GetTop(SelectedShape);
                PathGeometry pathGeometry = geometry as PathGeometry;
                PathFigure figure = pathGeometry.Figures[0];
                Point newStartPoint = new Point(figure.StartPoint.X + deltaX, figure.StartPoint.Y + deltaY);
                PathFigure newFigure = new PathFigure();
                newFigure.StartPoint = newStartPoint;
                if ((string)SelectedShape.Tag == "polygon")
                    newFigure.IsClosed = true;
                foreach (LineSegment segment in figure.Segments)
                {
                    double oldX = segment.Point.X;
                    double oldY = segment.Point.Y;
                    LineSegment newSegment = new LineSegment(new Point(oldX + deltaX, oldY + deltaY), true);
                    newFigure.Segments.Add(newSegment);
                }
                pathGeometry.Figures.Clear();
                pathGeometry.Figures.Add(newFigure);
            }
            else
            {
                transformX = geometry.Bounds.X + Canvas.GetLeft(SelectedShape);
                transformY = geometry.Bounds.Y + Canvas.GetTop(SelectedShape);
                geometry.Transform = new TranslateTransform(transformX, transformY);
            }

            Canvas.SetLeft(SelectedShape, 0);
            Canvas.SetTop(SelectedShape, 0);
            Status = string.Format("{0}, Left: {1} Top: {2} BoundsX: {3} BoundsY: {4}", geometry.ToString(), Canvas.GetLeft(SelectedShape), Canvas.GetTop(SelectedShape), geometry.Bounds.X, geometry.Bounds.Y);
        }

        public void Combine(int selectedIndex)
        {
            if (CombiPair.Count != 2)
            {
                Pile2PolyMin p2p = new Pile2PolyMin();
                p2p.ShowDialog();

                return;
            }
            double x1 = 0;                                                      //Bounding box left / top corner coordinates of combination group members
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;

            int count = 1;                                                      //counter for 2 group members
            SolidColorBrush combinationFill = null;
            SolidColorBrush combinationStroke = null;
            double combinationStrokeThickness = 0;
            CombinedGeometry combination = new CombinedGeometry();
            foreach (Shape s in CombiPair)
            {
                Path path = s as Path;
                Geometry geometry = path.Data as Geometry;// null;           //abstract class, from which PathGeometry and CombinedGeometry are derived (both are possible)

                if (count == 1)
                {
                    combination.Geometry1 = geometry;                       //define 1. member of combined geometry
                    combinationFill = s.Fill as SolidColorBrush;
                    combinationStroke = s.Stroke as SolidColorBrush;
                    combinationStrokeThickness = s.StrokeThickness;
                    x1 = Math.Round(geometry.Bounds.X);                                 //locate bounding box 1
                    y1 = Math.Round(geometry.Bounds.Y);
                }

                if (count == 2)
                {
                    combination.Geometry2 = geometry;                       //define 2. member of combined geometry
                    x2 = Math.Round(geometry.Bounds.X);                                 //locate bounding box 2
                    y2 = Math.Round(geometry.Bounds.Y);
                }
                count++;
            }

            switch (selectedIndex)                                              //set combination mode
            {
                case 0:
                    combination.GeometryCombineMode = GeometryCombineMode.Union;
                    break;
                case 1:
                    combination.GeometryCombineMode = GeometryCombineMode.Intersect;
                    break;
                case 2:
                    combination.GeometryCombineMode = GeometryCombineMode.Exclude;
                    break;
                case 3:
                    combination.GeometryCombineMode = GeometryCombineMode.Xor;
                    break;
            }

            double x = Math.Round(combination.Bounds.X);                                        //check bounding box of resulting geometry
            double y = Math.Round(combination.Bounds.Y);
            if (double.IsNegativeInfinity(x) || double.IsPositiveInfinity(x) || double.IsNaN(x) || double.IsNegativeInfinity(y) || double.IsPositiveInfinity(y) || double.IsNaN(y))
            {
                throw (new ArgumentException("Cette combinaison est impossible"));
            }
            if (x > 0 || y > 0)                                                     //if result is not located at 0, 0 (new Shape must start at 0, 0), repositioning must be done (method first pass)
            {
                originX = x;                                                        //save original combination bounding box position
                originY = y;
                foreach (Shape s in CombiPair)
                {
                    Path p = s as Path;
                    Geometry g = p.Data as Geometry;
                    if ((string)s.Tag == "polygon")
                    {
                        g.Transform = new TranslateTransform(-x, -y);            //reposition original components to get resulting combination geometry at 0, 0. 
                    }                                                               //Polygon requires different code, as it was (likely) not generated at 0, 0, unlike Rectangle or Ellipse, that are always generated at 0, 0. 
                    else
                    {
                        g.Transform = new TranslateTransform(Math.Round(g.Bounds.X - x), Math.Round(g.Bounds.Y - y));   //reposition original components to get resulting combination geometry at 0, 0. NOT WORKING WITH POLYGON
                    }
                }                                                                               //after repositioning, relative distances betwen two members are kept, just whole group bounding box is at 0, 0
                Combine(selectedIndex);                                                         //Whole method is called again with corrected bounds of both members

            }
            else
            {
                foreach (Shape s in CombiPair)                          //in second method pass, resulting combination bounding box should be at 0, 0 now
                {
                    Shapes.Remove(s);                                   //remove originals from Canvas bound collection
                }
                CombiPair.Clear();
                myPath = new Path();
                myPath.Fill = combinationFill;
                myPath.Stroke = combinationStroke;
                myPath.StrokeThickness = combinationStrokeThickness;
                combination.Transform = new TranslateTransform(originX, originY);   //restore combination correct original position
                myPath.Data = combination;
                InsertShape("combination");
            }
        }


    }

}
