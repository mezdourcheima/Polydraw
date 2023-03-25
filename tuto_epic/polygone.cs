using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyDraw.Main
{
    class Polygon : shape
    {
        public int NumSegment;
        public float Radius;



        public Point[] creation(Point[] polygon, int NumSegment, float Radius)
        {
            float a = 200.0F; //Coordonnées du centre de la cercle
            float b = 100.0F;
            int i = 0;
            for (double deg = 0; (deg <= 360); deg = (deg + (360 / NumSegment)))
            {
                float angle = (float)(deg * (Math.PI / 180));
                float x = ((float)((a + (Radius * Math.Cos(angle)))));
                float y = ((float)((b + (Radius * Math.Sin(angle)))));
                polygon[i] = new Point(x, y);
                i++;
            }

            return polygon;
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
        public Point[] Rotation(Point[] polygon , double R)
        {

            Point p = FindCentroid(polygon); //centerPoint
            for (int a = 0; a < polygon.Length; a++)
            {
                polygon[a] = RotatePoint(polygon[a], p, R);
                //Console.WriteLine(Corners1[a].X.ToString() + Corners1[a].Y.ToString());
            }
            return polygon;
        }
    }
}

