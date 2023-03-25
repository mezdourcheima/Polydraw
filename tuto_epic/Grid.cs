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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tuto_epic
{
    class Grid
    {
        public int Spacing { get; set; } = 50;
        public double Width { get; set; }
        public double Height { get; set; }
        private DrawingGroup drawingGroup = new DrawingGroup();
        private Pen pen = new Pen(Brushes.LightBlue, 1);
        ImageBrush image;


        public ImageBrush UdateGrid(bool visible)//ImageBrush
        {
            drawingGroup.Children.Clear();
            if (visible)
            {
                pen.Brush = Brushes.LightBlue;
            }
            else
            {
                pen.Brush = Brushes.Transparent;
            }
            for (int x = 0; x < Width + Spacing; x += Spacing)
            {
                LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, Height));
                GeometryDrawing lineDrawing = new GeometryDrawing(Brushes.Transparent, pen, line);
                drawingGroup.Children.Add(lineDrawing);
            }
            for (int y = 0; y < Height + Spacing; y += Spacing)
            {
                LineGeometry line = new LineGeometry(new Point(0, y), new Point(Width, y));
                GeometryDrawing lineDrawing = new GeometryDrawing(Brushes.Transparent, pen, line);
                drawingGroup.Children.Add(lineDrawing);
            }
            image = new ImageBrush(new DrawingImage(drawingGroup));
            image.Stretch = Stretch.None;
            image.TileMode = TileMode.None;
            image.AlignmentX = AlignmentX.Left;
            image.AlignmentY = AlignmentY.Top;
            return image;
        }
    }
}
