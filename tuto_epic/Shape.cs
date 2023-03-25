using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyDraw.Main
{
    class shape
    {
        protected String Name;
        protected Point[] polygon = new Point[200];
        public shape()
        {

        }
        public shape(string Name, Point[] polygon)
        {
            this.Name = Name;
            this.polygon = polygon;
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
    }
}

