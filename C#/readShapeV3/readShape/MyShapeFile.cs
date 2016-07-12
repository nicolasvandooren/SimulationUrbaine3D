using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Catfood.Shapefile;
using System.Windows.Forms;
using System.Drawing;

namespace readShape
{
    class MyShapeFile
    {
        public Bdd bdd;
        private Shapefile shapefile;
        private string path;
        private ProgressBar progressBar = null;
        private WindingNumber wn;
        public MyShapeFile(string pathShape)
        {
            bdd = new Bdd();
            path = pathShape;
            shapefile = new Shapefile(pathShape);
            wn = new WindingNumber();
        }

        public MyShapeFile(string pathShape, ProgressBar pb)
        {
            bdd = new Bdd();
            path = pathShape;
            shapefile = new Shapefile(pathShape);
            progressBar = pb;
            initProgressBar();
            wn = new WindingNumber();
        }

        public void closeBddShape()
        {
            bdd.closeConnection();
        }

        public void showBounding()
        {
            //ZONE
            Console.WriteLine("Bounds: {0},{1} -> {2},{3}",
                    shapefile.BoundingBox.Left,
                    shapefile.BoundingBox.Top,
                    shapefile.BoundingBox.Right,
                    shapefile.BoundingBox.Bottom);
            Console.WriteLine();
        }

        public ShapeType getShapeType()
        {
            return shapefile.Type;
        }

        public int getNumberShapes()
        {
            return shapefile.Count;
        }

        public int insertRoadGraph()
        {
            foreach (Shape shape in shapefile)
            {
                if (progressBar != null)
                    showProgressBar();

                if ((insertDataRoadGraph(shape)) < 0)
                    return -1;
            }
                return 0;
        }

        public int insertSpeed()
        {
            foreach (Shape shape in shapefile)
            {
                if (progressBar != null)
                    showProgressBar();

                if (insertDataSpeed(shape) < 0)
                    return -1;
            }
            return 0;
        }

        public int insertRoadSign()
        {
            foreach (Shape shape in shapefile)
            {
                if (progressBar != null)
                    showProgressBar();

                if (insertDataRoadSign(shape) < 0)
                    return -1;
            }

            return 0;
        }

        public int insertFlux(double xHaut, double xBas, double yGauche, double yDroite)
        {
            List<int> lEdgeOut = bdd.getAllIdEdgeOut(xHaut,yGauche,xBas,yDroite);
            foreach (Shape shape in shapefile)
            {
                if (progressBar != null)
                    showProgressBar();
                //showCoordByType(shape);
                
                if (insertDataFlux(shape, lEdgeOut) < 0)
                    return -1;
                
            }
            return 0;
        }

        public int readMyShape()
        {
            if (checkError(path) < 0)
            {
                return -1;
            }
            
            foreach (Shape shape in shapefile)
            {
                showCoordByType(shape);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine();
            }
            
            Console.WriteLine("Done");
            Console.WriteLine();
            return 0;
        }

        

        private void initProgressBar()
        {
            progressBar.Maximum = shapefile.Count;
            progressBar.Step = 1;
            progressBar.Value = 0;
        }

        private void showProgressBar()
        {
            progressBar.Refresh();
            int percent = (int)(((double)progressBar.Value / (double)progressBar.Maximum) * 100);
            progressBar.Value += 1;
            progressBar.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressBar.Width / 2 - 10, progressBar.Height / 2 - 7));
            
        }
        
       

        private bool checkRoadSign(Shape shape)
        {
            List<string> refRoadSigns = new List<string>();
            refRoadSigns.Add("3x200");
            refRoadSigns.Add("3x300");
            foreach(string refRoadSign in refRoadSigns)
            {
                if (shape.GetMetadata("type_feux").Contains(refRoadSign) )
                {
                    return true;
                }
            }
            return false;
        }

        private int insertDataRoadGraph(Shape shape)
        {
            if (shape.GetMetadata("nb_voie_tf") != null || shape.GetMetadata("nb_voie_ft") != null)
            {
                List<int> lIdVertex = new List<int>();
                int idEdge;
                Point pointFirst = new Point();
                Point pointLast = new Point();
                idEdge = insertGrapheRoutier(shape);
                switch (shape.Type)
                {
                    case ShapeType.PolyLine:
                        ShapePolyLine shapePolyline = shape as ShapePolyLine;
                        addCoordEdge(shapePolyline.Parts, idEdge);
                        break;
                    default:
                        return -1;
                }

                pointFirst = bdd.getFirstPointsByEdge(idEdge);
                pointLast = bdd.getLastPointsByEdge(idEdge);
                lIdVertex.Add(insertVertex(idEdge, pointFirst));
                lIdVertex.Add(insertVertex(idEdge, pointLast));

                int i = 0;
                foreach (int idVertex in lIdVertex)
                {
                    if (idVertex > 0 && i == 0)
                    {
                        bdd.insertConnexionVertex(idVertex, bdd.getIdVertexByPoint(pointLast.x, pointLast.y));
                    }
                    else if (idVertex > 0 && i == 1)
                    {
                        bdd.insertConnexionVertex(bdd.getIdVertexByPoint(pointFirst.x, pointFirst.y), idVertex);
                    }
                    i++;
                }
                return idEdge;
            }
            return -1;
        }

        private int insertDataRoadSign(Shape shape)
        {
            if(shape.GetMetadata("type_feux") != null) {
                string[] metadataNames = shape.GetMetadataNames();
                if (metadataNames != null && checkRoadSign(shape))
                {
                    //getCoordByType(shape, 0);
                    switch (shape.Type)
                    {
                        case ShapeType.Point:
                            ShapePoint shapePoint = shape as ShapePoint;
                            bdd.insertSignalisation(shape.GetMetadata("type_feux"), shapePoint.Point.X, shapePoint.Point.Y, Convert.ToDouble(shape.GetMetadata("angle")));
                            break;
                        default:
                            return -1;
                    }
                }
                return 0;
            }
            return -1;
        }

        private int insertDataSpeed(Shape shape) {
            if (shape.GetMetadata("Etat") != null || shape.GetMetadata("limite_vit") != null)
            {
                string[] metadataNames = shape.GetMetadataNames();
                if (metadataNames != null)
                {
                    switch (shape.Type)
                    {
                        case ShapeType.Polygon:
                            ShapePolygon shapePolygon = shape as ShapePolygon;
                            List<Point> pointsPolygone = getCoord(shapePolygon.Parts);
                            List<Edge> allEdges = bdd.getAllEdges();
                            foreach (Edge edge in allEdges)
                            {
                                List<Point> points = new List<Point>();
                                points = bdd.getPointsByIdEdge(edge.id);
                                foreach (Point point in points)
                                {
                                    if (Convert.ToInt32(shape.GetMetadata("limite_vit")) < bdd.getSpeedByIdEdge(edge.id))
                                        addSpeed(pointsPolygone, point, edge, shape);
                                }
                            }
                            break;

                        default:
                            return -1;
                    }
                }
                return 0;
            }
            return -1;
        }

        private int insertDataFlux(Shape shape, List<int>lEdgeOut)
        {
            if (shape.GetMetadata("tf_temps_8") != null)
            {
                string[] metadataNames = shape.GetMetadataNames();
                if (metadataNames != null)
                {
                    switch (shape.Type)
                    {
                        case ShapeType.PolyLine:
                            ShapePolyLine shapePolyline = shape as ShapePolyLine;
                            List<Point> pointsPolyline = getCoord(shapePolyline.Parts);
                            foreach(int idEdge in lEdgeOut)
                            {
                                Point pointFirst = bdd.getFirstPointsByEdge(idEdge);
                                Point pointLast = bdd.getLastPointsByEdge(idEdge);
                                
                                if (((Double.Equals(Math.Round(Convert.ToDouble(pointsPolyline[0].x), 3), Math.Round(pointFirst.x, 3))) && (Double.Equals(Math.Round(Convert.ToDouble(pointsPolyline[pointsPolyline.Count-1].x), 3), Math.Round(pointLast.x, 3)))) ||
                                    ((Double.Equals(Math.Round(Convert.ToDouble(pointsPolyline[0].x), 3), Math.Round(pointLast.x, 3)))  && (Double.Equals(Math.Round(Convert.ToDouble(pointsPolyline[pointsPolyline.Count-1].x), 3), Math.Round(pointFirst.x, 3)))))
                                {
                                    bdd.insertFlux(Convert.ToInt32(shape.GetMetadata("ft_temps_8")), Convert.ToInt32(shape.GetMetadata("tf_temps_8")), idEdge);
                                }
                            }
                            break;
                        
                        default:
                            return -1;
                    }
                }
                return 0;
            }
            return -1;
        }

        /*private bool doubleEquals(double double1, double double2)
        {
            double difference = Math.Abs(double1 * .00001);

            // Compare the values
            // The output to the console indicates that the two values are equal
            if (Math.Abs(double1 - double2) <= difference)
                return true;
            else
                return false;
        }*/

        private bool pointIn(List<Point> points1, List<Point> points2)
        {
            foreach(Point p1 in points1)
            {
                foreach(Point p2 in points2)
                {
                    
                }
            }
            return false;
        }

        public int insertDataFlux(Shape shape, List<Vertex> vertexOut)
        {
            if (shape.GetMetadata("ppm") != null || shape.GetMetadata("tjom") != null)
            {
                double dist = 0;
                Vertex vFinal = new Vertex();
                List<Point> lPoints = new List<Point>();
                if (shape.Type == ShapeType.PolyLine)
                {
                    ShapePolyLine shapePolyline = shape as ShapePolyLine;
                    lPoints = getCoord(shapePolyline.Parts);
                } else
                {
                    return -1;
                }

                string[] metadataNames = shape.GetMetadataNames();
                if (metadataNames != null && lPoints.Count() != 0)
                {
                    foreach (Vertex v in vertexOut)
                    {
                        double tmp = getMinDist(v, lPoints);
                        if (tmp < dist || dist == 0) { 
                            dist = tmp;
                            vFinal = v;
                        }
                    }
                    if (dist < 100) {
                        foreach(Point p in lPoints)
                            Console.WriteLine("X flux : " + p.x + " Y flux : " + p.y);
                        double angle = Math.Tan((lPoints[1].y - lPoints[0].y)/ (lPoints[1].x - lPoints[0].x));
                        Console.WriteLine("Angle : " + angle);
                        Console.WriteLine("Id " + vFinal.id + " Coordonnées X / Y : " + vFinal.x + " / " + vFinal.y + " plus petite distance : " + dist + " PPM : " + shape.GetMetadata("ppm"));
                    }
                }
                return 0;
            }
            return -1;
        }

        private double getMinDist(Vertex v, List<Point> points)
        {
            //List<double> infoPoints = new List<double>();
            //Point point = new Point();
            double tmp = 0;
            double dist = 0;
            foreach(Point p in points)
            {
                if (dist == 0) { 
                    dist = distance(v.x, v.y, p.x, p.y);
                }
                tmp = distance(v.x, v.y, p.x, p.y);
                if (tmp < dist)
                {
                    dist = tmp;
                }
            }
            return dist;
        }

        private double distance(double xA, double yA, double xB, double yB) {
            return Math.Sqrt(Math.Pow(xB - xA, 2) + Math.Pow(yB - yA, 2));    
        }

        private int insertVertex(int idEdge, Point point)
        {
            int idVertex;
            idVertex = bdd.getIdVertexByPoint(point.x, point.y);
            if (idVertex < 0)
            {
                idVertex = bdd.insertVertexByEdge(idEdge, point.x, point.y);
                return idVertex;
            }
            else
            {
                bdd.insertConnexionVertexEdge(idEdge, idVertex);
                return idVertex;
            }
        }

        private int insertGrapheRoutier(Shape shape)
        {
            int idRecord = shape.RecordNumber;
            int nbVoieFT = 0;
            int nbVoieTF = 0;
            string hierarchie;
            string nomRue;
            nbVoieFT = Convert.ToInt32(shape.GetMetadata("nb_voie_ft"));
            nbVoieTF = Convert.ToInt32(shape.GetMetadata("nb_voie_tf"));
            hierarchie = shape.GetMetadata("hierarchie");
            nomRue = shape.GetMetadata("nomvoie");
            return bdd.insertEdges(nbVoieFT, nbVoieTF, 50, hierarchie, nomRue);
        }

        private void addSpeed(List<Point> points, Point point, Edge edge, Shape shape)
        {
            if (wn.wm_PnPoly(point, points) < 0)
            {
                Console.WriteLine(shape.GetMetadata("nom_zone"));
                Console.WriteLine("Inside : " + edge.id);
                bdd.updateVitesseByIdEdge(edge.id, Convert.ToInt32(shape.GetMetadata("limite_vit")));
            }
        }

        private void addCoordEdge(List<PointD[]> parts, int id)
        {
            foreach (PointD[] part in parts)
            {
                foreach (PointD point in part)
                {
                    bdd.insertPointsEdge(point.X, point.Y, id);
                }
            }
        }

        private List<Point> getCoord(List<PointD[]> parts)
        {
            List<Point> points = new List<Point>();
            foreach (PointD[] part in parts)
            {
                //Console.WriteLine("Number of points : " + part.Count<PointD>());
                foreach (PointD point in part)
                {
                    Point newPoint = new Point();
                    newPoint.x = point.X;
                    newPoint.y = point.Y;
                    points.Add(newPoint);
                    //Console.WriteLine("{0}, {1}", point.X, point.Y);
                }
                //Console.WriteLine();
            }
            return points;
        }

        private void showCoordByType(Shape shape)
        {
            // cast shape based on the type
            switch (shape.Type)
            {
                case ShapeType.Point:
                    // a point is just a single x/y point
                    ShapePoint shapePoint = shape as ShapePoint;
                    Console.WriteLine("Point={0},{1}", shapePoint.Point.X, shapePoint.Point.Y);
                    break;

                case ShapeType.Polygon:
                    // a polygon contains one or more parts - each part is a list of points which
                    // are clockwise for boundaries and anti-clockwise for holes 
                    // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                    ShapePolygon shapePolygon = shape as ShapePolygon;
                    Console.WriteLine("Polygone Point:");
                    foreach (PointD[] part in shapePolygon.Parts)
                    {
                        foreach (PointD point in part)
                        {
                            Console.WriteLine("Coordonnée x, y: " + point.X + ", " + point.Y);
                        }
                    }
                    break;

                case ShapeType.PolyLine:
                    ShapePolyLine shapePolyline = shape as ShapePolyLine;
                    Console.WriteLine("Polyline Point:");
                    foreach (PointD[] part in shapePolyline.Parts)
                    {
                        foreach (PointD point in part)
                        {
                            Console.WriteLine("Coordonnée x, y: " + point.X + ", " + point.Y);
                        }
                    }

                    break;

                case ShapeType.MultiPoint:
                    ShapeMultiPoint shapeMultipoint = shape as ShapeMultiPoint;
                    Console.WriteLine("MultiPoint Point:");
                    Console.WriteLine("Number of points : " + shapeMultipoint.Points.Count<PointD>());
                    foreach (PointD point in shapeMultipoint.Points)
                    {
                        Console.WriteLine("{0}, {1}", point.X, point.Y);
                    }
                    Console.WriteLine();
                    break;

                default:
                    // and so on for other types...
                    break;
            }
        }

        private int checkError(string path)
        {
            String extension = Path.GetExtension(path);
            if (path.Length == 0 || !File.Exists(path) || String.Compare(extension, ".shp") != 0)
            {
                Console.WriteLine("Your path is not valable");
                return -1;
            }
            return 0;
        }
    }

}
