using System.Collections.Generic;
using SimpleJSON;
public class Vertex
{
    public int id;
    public List<int> idConnect;
    public Point point;
    public Vertex(int id, List<int> idConnect, Point point)
    {
        this.id = id;
        this.idConnect = idConnect;
        this.point = point;
    }
    public JSONNode SaveToJSON()
    {
        JSONNode node = new JSONClass();
        node["id"] = this.id.ToString();
        JSONArray arrayIdConnect = new JSONArray();
        foreach (int idC in this.idConnect)
        {
            JSONNode newNode = new JSONClass();
            newNode["id_connect"] = idC.ToString();
            arrayIdConnect.Add(newNode);
        }
        node["connect"] = arrayIdConnect;
        node["position"] = this.point.SaveToJSON();
        return node;
    }
}

public class Point
{
    public double x;
    public double y;
    public double z;

    public Point(double x, double y,double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Point(JSONNode aNode)
    {
        this.x = double.Parse(aNode["x"].Value);
        this.y = double.Parse(aNode["y"].Value);
        this.z = double.Parse(aNode["z"].Value);

    }
    public JSONNode SaveToJSON()
    {
        JSONNode node = new JSONClass();
        node["x"] = this.x.ToString();
        node["y"] = this.y.ToString();
        node["z"] = this.z.ToString();
        return node;
    }
}

public class Edge
{
    public int id;
    public List<Point> points;
    public int vitesse;
    public int nbVoieFT;
    public int nbVoieTF;
    public int idVertexInFirstPoint;
    public int idVertexInLastPoint;

    public Edge(int idArete, int vitesse, List<Point> points, int nbVoieFT, int nbVoieTF, int idVertexInFirstPoint, int idVertexInLastPoint)
    {
        this.id = (idArete);
        this.vitesse = (vitesse);
        this.points = points;
        this.nbVoieFT = nbVoieFT;
        this.nbVoieTF = nbVoieTF;
        this.idVertexInFirstPoint = idVertexInFirstPoint;
        this.idVertexInLastPoint = idVertexInLastPoint;
    }

    public Edge(JSONNode aNode)
    {
        this.id =int.Parse( aNode["id"].Value);
        this.vitesse = int.Parse(aNode["vitesse"].Value);
        
    }
    public JSONNode SaveToJSON()
    {
        JSONNode node = new JSONClass();
        node["id"] = this.id.ToString();
        JSONArray arrayPoints = new JSONArray();
        foreach(Point point in this.points)
            arrayPoints.Add(point.SaveToJSON());
        node["point"] = arrayPoints;
        node["vitesse"] = this.vitesse.ToString();
        node["nbVoieFT"] = this.nbVoieFT.ToString();
        node["nbVoieTF"] = this.nbVoieTF.ToString();
        node["idVertexFirst"] = this.idVertexInFirstPoint.ToString();
        node["idVertexLast"] = this.idVertexInLastPoint.ToString();
        return node;
    }
}

public class Flux
{
    public int id;
    public int idEdge;
    public int countFT;
    public int countTF;

    public Flux(int idFlux, int countFT, int countTF, int idConnection)
    {
        this.id = idFlux;
        this.countFT = countFT;
        this.countTF = countTF;
        this.idEdge = idConnection;
    }

    public JSONNode SaveToJSON()
    {
        JSONNode node = new JSONClass();
        node["id"] = this.id.ToString();
        node["idEdge"] = this.idEdge.ToString();
        node["countFT"] = this.countFT.ToString();
        node["countTF"] = this.countTF.ToString();
        return node;
    }
}

public class RoadSign
{
    public int id;
    public double angle;
    public Point points;
    public string type;

    public RoadSign(int idRoadSign, double angle, Point points, string type)
    {
        this.id = idRoadSign;
        this.angle = angle;
        this.points = points;
        this.type = type;
    }
    public JSONNode SaveToJSON()
    {
        JSONNode node = new JSONClass();
        node["id"] = this.id.ToString();
        node["position"] = this.points.SaveToJSON();
        node["angle"] = this.angle.ToString();
        node["type"] = this.type;
        return node;
    }
}