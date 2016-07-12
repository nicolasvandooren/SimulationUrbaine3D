using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readShape
{
	//http://geomalgorithms.com/a03-_inclusion.html
    class WindingNumber
    {
        public WindingNumber() { }
        public double isLeft(Point P0, Point P1, Point P2)
        {
            return ((P1.x - P0.x) * (P2.y - P0.y) - (P2.x - P0.x) * (P1.y - P0.y));
        }
        //===================================================================

        // cn_PnPoly(): crossing number test for a point in a polygon
        //      Input:   P = a point,
        //               V[] = vertex points of a polygon V[n+1] with V[n]=V[0]
        //      Return:  0 = outside, 1 = inside
        // This code is patterned after [Franklin, 2000]
        public int cn_PnPoly(Point P, List<Point> V)
        {
            int cn = 0;
            for(int i = 0; i < V.Count()-1; i++)
            {
                if (((V[i].y <= P.y) && (V[i+1].y < P.y)) || ((V[i].y > P.y) && (V[i + 1].y <= P.y))) { 
                    double vt = (P.y - V[i].y) / (V[i + 1].y - V[i].y);
                    if (P.x < V[i].x + vt * (V[i + 1].x - V[i].x)) // P.x < intersect
                        cn++;   // a valid crossing of y=P.y right of P.x
                }
            }
            return (cn & 1); //Si pair (Out), si impaire (in)
        }
        //===================================================================

        // wn_PnPoly(): winding number test for a point in a polygon
        //      Input:   P = a point,
        //               V[] = vertex points of a polygon V[n+1] with V[n]=V[0]
        //    
        public int wm_PnPoly(Point P, List<Point> V)
        {
            int wn = 0; //The winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < V.Count-1; i++)
            {   // edge from V[i] to  V[i+1]
                if (V[i].y <= P.y)
                {          // start y <= P.y
                    if (V[i + 1].y > P.y)      // an upward crossing
                        if (isLeft(V[i], V[i + 1], P) > 0)  // P left of  edge
                            ++wn;            // have  a valid up intersect
                }
                else
                {                        // start y > P.y (no test needed)
                    if (V[i + 1].y <= P.y)     // a downward crossing
                        if (isLeft(V[i], V[i + 1], P) < 0)  // P right of  edge
                            --wn;            // have  a valid down intersect
                }
            }

            return wn;
        }
    }
}
