using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyDraw.Main;

namespace tuto_epic.Operations_Booleenes
{
    class intersect:shape 
    {
        protected String Nom;
        protected PointF[] Sommets;

        public intersect(String Nom, PointF[] Sommets)
        {
            this.Nom = Nom;
            this.Sommets = Sommets;
        }

        //Dans cette algorithme la soustraction va se faire dans le          
        //sense poly1 - poly2

        public PointF[] SOUS1(intersect poly1, intersect poly2)
        {
            List<PointF> clippedCorners = new List<PointF>();
            Boolean inter = Inter(poly1, poly2);
            //   if (inter == true)   //if (il y a une intersection entre les 2 polygones )
            // {
            for (int i = 0, next = 1; i < poly1.Sommets.Length; i++, next = (i + 1 == poly1.Sommets.Length) ? 0 : i + 1)
            {
                if (IsPointInsidePoly(poly1.Sommets[i], poly2) == false)
                {
                    AddPoints(clippedCorners, new PointF[] { poly1.Sommets[i] });
                }
            }
            //Ajouter les côtes du poly2 qui se trouvent à l'intérieur de poly1      
            for (int i = 0; i < poly2.Sommets.Length; i++)
            {
                if (IsPointInsidePoly(poly2.Sommets[i], poly1))
                    AddPoints(clippedCorners, new PointF[] { poly2.Sommets[i] });
            }
            //Ajouter les points d’intersections entre segment de poly1 et poly2
            for (int i = 0, next = 1; i < poly1.Sommets.Length; i++, next = (i + 1 == poly1.Sommets.Length) ? 0 : i + 1)
            {
                AddPoints(clippedCorners, GetIntersectionPoints(poly1.Sommets[i], poly1.Sommets[next], poly2));
            }
            //Après la récupération des points d’intersection
            //récupération des sommets du poly1 si le sommet n’est pas à l'intérieur du poly2

            return OrderClockwise(clippedCorners.ToArray());


        }

        //Dans cette algorithme la soustraction va se faire dans le          
        //sense poly2 - poly1

        public PointF[] SOUS2(intersect poly1, intersect poly2)
        {
            List<PointF> clippedCorners = new List<PointF>();
            Boolean inter = Inter(poly1, poly2);
            //  if (inter == true)   //if (il y a une intersection entre les 2 polygones )
            // {
            //Ajouter les côtes du poly1 qui se trouvent à l'intérieur de poly2      
            for (int i = 0; i < poly1.Sommets.Length; i++)
            {
                if (IsPointInsidePoly(poly1.Sommets[i], poly2))
                    AddPoints(clippedCorners, new PointF[] { poly1.Sommets[i] });
            }
            //Ajouter les points d’intersections entre segment de poly2 et poly1
            for (int i = 0, next = 1; i < poly2.Sommets.Length; i++, next = (i + 1 == poly2.Sommets.Length) ? 0 : i + 1)
            {
                AddPoints(clippedCorners, GetIntersectionPoints(poly2.Sommets[i], poly2.Sommets[next], poly1));
            }
            //Après la récupération des points d’intersection
            //récupération des sommets du poly2 si le sommet n’est pas à l'intérieur du poly1
            for (int i = 0, next = 1; i < poly2.Sommets.Length; i++, next = (i + 1 == poly2.Sommets.Length) ? 0 : i + 1)
            {
                if (IsPointInsidePoly(poly2.Sommets[i], poly2) == false)
                {
                    AddPoints(clippedCorners, new PointF[] { poly2.Sommets[i] });
                }
            }
            return OrderClockwise(clippedCorners.ToArray());
            // }


        }

        //Verifier s'il y a une intersection entre les deux polygones
        public Boolean Inter(intersect poly1, intersect poly2)
        {
            List<PointF> clippedCorners = new List<PointF>();
            //Verifier s'il y a des côtes du poly1 qui se trouvent à l'intérieur de poly2
            Boolean intersec = false;
            for (int i = 0; i < poly1.Sommets.Length; i++)
            {
                if (IsPointInsidePoly(poly1.Sommets[i], poly2))
                {
                    intersec = true;
                }
            }
            //Verifier s'il y a des côtes du poly2 qui se trouvent à l'intérieur de poly1      
            for (int i = 0; i < poly2.Sommets.Length; i++)
            {
                if (IsPointInsidePoly(poly2.Sommets[i], poly1))
                {
                    intersec = true;
                }
            }
            return intersec;


        }
        // public void DrawObjet(Pen pen , Point[] sommets)
        //{ }

        public bool onSegment(PointF p, PointF q, PointF r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;
            return false;
        }

        public PointF GetIntersectionPoint(PointF l1p1, PointF l1p2, PointF l2p1, PointF l2p2)
        {
            float A1 = l1p2.Y - l1p1.Y;
            float B1 = l1p1.X - l1p2.X;
            float C1 = A1 * l1p1.X + B1 * l1p1.Y;

            float A2 = l2p2.Y - l2p1.Y;
            float B2 = l2p1.X - l2p2.X;
            float C2 = A2 * l2p1.X + B2 * l2p1.Y;
            //lignes sont parallèles, donc il n ' y a pas d’intersection
            float det = A1 * B2 - A2 * B1;
            if (det == 0)
            {
                var defaultVal = default(PointF);
                return defaultVal;
            }
            else
            {
                float x = (B2 * C1 - B1 * C2) / det;
                float y = (A1 * C2 - A2 * C1) / det;
                PointF p = new PointF(x, y);
                bool online1 = onSegment(l1p1, p, l1p2);
                bool online2 = onSegment(l2p1, p, l2p2);
                if (online1 && online2)
                    return p; // new PointF(x,y)
            }
            var defaultVal2 = default(PointF);
            return defaultVal2;


        }
        public void AddPoints(List<PointF> pool, PointF[] newpoints)
        {
            foreach (PointF np in newpoints)
            {
                bool found = false;
                foreach (PointF p in pool)
                {
                    if (Object.Equals(p.X, np.X) && Object.Equals(p.Y, np.Y))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) pool.Add(np);
            }
        }


        public bool IsPointInsidePoly(PointF test, intersect poly)
        {
            int i;
            int j;
            bool result = false;
            for (i = 0, j = poly.Sommets.Length - 1; i < poly.Sommets.Length; j =
            i++)
            {
                if ((poly.Sommets[i].Y > test.Y) != (poly.Sommets[j].Y > test.Y) && (test.X < (poly.Sommets[j].X - poly.Sommets[i].X) *
                (test.Y - poly.Sommets[i].Y) / (poly.Sommets[j].Y - poly.Sommets[i].Y) + poly.Sommets[i].X))
                {
                    result = !result;
                }
            }
            return result;

        }

        public PointF[] GetIntersectionPoints(PointF l1p1, PointF l1p2, intersect poly)
        {
            List<PointF> intersectionPoints = new List<PointF>();
            for (int i = 0; i < poly.Sommets.Length; i++)
            {
                int next = (i + 1 == poly.Sommets.Length) ? 0 : i + 1;
                PointF ip = GetIntersectionPoint(l1p1, l1p2, poly.Sommets[i], poly.Sommets[next]);
                if (ip != null) intersectionPoints.Add(ip);
            }
            return intersectionPoints.ToArray();
        }

        public PointF[] OrderClockwise(PointF[] sommets)
        {


            double mX = 0;
            double my = 0;
            foreach (PointF P in sommets)
            {
                mX += P.X;
                my += P.Y;
            }
            mX /= sommets.Length;
            my /= sommets.Length;

            return sommets.OrderBy(v => Math.Atan2(v.Y - my, v.X - mX)).ToArray();



        }
    }
}
