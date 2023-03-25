using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyDraw.Main
//Cette class permet de creer un rectangle
//----------------------------------------
{
    class Rectangle : shape
    {
        private float length;
        private float width;

        //------------------------------------Constructeur-----------------------------------------
        //-----------------------------------------------------------------------------------------

        public Rectangle(String Name, Point[] polygon, float length, float width) : base(Name, polygon)
        {
            this.length = length;
            this.width = width;
        }


        //---------------------------------------------methode de creation d'un rectangle--------------------------------------
        //---------------------------------------------------------------------------------------------------------------------

        public Point[] Creation(float length, float width)
        {
            float x = 0.0F;
            float y = 0.0F;
            Point p1 = new Point((float)x, (float)y);
            polygon[0] = p1;
            Point p2 = new Point((float)(x + length), (float)y);
            polygon[1] = p2;
            Point p3 = new Point((float)x, (float)(y + width));
            polygon[2] = p3;
            Point p4 = new Point((float)(x + length), (float)(y + width));
            polygon[3] = p4;
            return polygon;
        }

    }
}
