using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;

public class Bdd {
    
    public IDbConnection dbconn;

    public Bdd(string path)
    {
        string conn = "URI=file:" + path;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        
    }

    public void closeConnection()
    {
        dbconn.Close();
        dbconn = null;
    }

    // Use this for initialization
    public List<Edge> getAllEdges()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<Edge> edges = new List<Edge>();
        string sqlQuery = "SELECT * FROM aretes ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            List<Point> points = new List<Point>();
            int id = reader.GetInt32(0);
            int vitesse = reader.GetInt32(3);
            int nbVoieFT = reader.GetInt32(1);
            int nbVoieTF = reader.GetInt32(2);
            /*
            string hierarchie = reader.GetString(4);
            string nomRue = reader.GetString(5);*/
            points = getPointsByIdEdge(id);

            Edge newEdge = new Edge(id, vitesse, points, nbVoieFT, nbVoieTF,getVertexFirstByIdEdge(id), getVertexLastByIdEdge(id));
            edges.Add(newEdge);
            //Debug.Log("id= " + id + "  nbVoieFT =" + nbVoieFT + "  nbVoieTF =" + nbVoieTF + " Vitesse =" + vitesse + " Hierarchie =" + hierarchie + " nomRue =" + nomRue);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return edges;
    }
    
    public int getVertexFirstByIdEdge(int idEdge)
    {
        Point point = new Point(0, 0, 0);
        point = getFirstPointsByEdge(idEdge);
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT sommets.idsommet FROM sommets " +
            "WHERE sommets.posX LIKE " + point.x + " AND sommets.posY LIKE " + point.z;

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            return reader.GetInt32(0);
        }
        return -1;
    }

    public Point getFirstPointsByEdge(int idEdge)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        Point point = new Point(0, 0, 0);
        string sqlQuery = "SELECT points.x, points.y FROM aretes JOIN points ON aretes.idaretes = points.idaretes " +
            "WHERE aretes.idaretes LIKE " + idEdge + " LIMIT 1";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            point.x = reader.GetDouble(0);
            point.z = reader.GetDouble(1);
        }
        return point;
    }

    public int getVertexLastByIdEdge(int idEdge)
    {
        Point point = new Point(0, 0, 0);
        point = getLastPointsByEdge(idEdge);
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT sommets.idsommet FROM sommets " +
            "WHERE sommets.posX LIKE " + point.x + " AND sommets.posY LIKE " + point.z;

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            return reader.GetInt32(0);
        }
        return -1;
    }

    public Point getLastPointsByEdge(int idEdge)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        Point point = new Point(0, 0, 0);
        string sqlQuery = "SELECT points.x, points.y FROM aretes JOIN points ON aretes.idaretes = points.idaretes " +
            "WHERE aretes.idaretes LIKE " + idEdge + " ORDER BY points.idpoints DESC LIMIT 1";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            point.x = reader.GetDouble(0);
            point.z = reader.GetDouble(1);
        }
        return point;
    }

    public List<Point> getPointsByIdEdge(int idEdge)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<Point> points = new List<Point>();
        string sqlQuery = "SELECT * FROM points WHERE points.idaretes =  " + idEdge;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            Point newPoint = new Point(reader.GetDouble(1), 0.75,reader.GetDouble(2));
            points.Add(newPoint);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return points;
    }

    public List<Vertex> getAllVertex()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<Vertex> vertex = new List<Vertex>();
        string sqlQuery = "SELECT * FROM sommets";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            List<int> idConnection = getVertexConnectionByIdVertex(reader.GetInt32(0));
            Point point = new Point(reader.GetDouble(1),0, reader.GetDouble(2));
            Vertex newVertex = new Vertex(reader.GetInt32(0), idConnection, point);
            vertex.Add(newVertex);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return vertex;
    }

    public List<int> getVertexConnectionByIdVertex(int idVertex)
    {
        int i = 0;
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<int> idConnect = new List<int>();
        string sqlQuery = "SELECT * FROM connexionSommets " + 
            "WHERE connexionSommets.idsommet = " + idVertex + 
            " OR connexionSommets.idsommetConnexion = " + idVertex;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            if (idVertex == reader.GetInt32(0))
                idConnect.Add(reader.GetInt32(1));
            if (idVertex == reader.GetInt32(1))
                idConnect.Add(reader.GetInt32(0));
            i++;
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return idConnect;
    }

    public List<RoadSign> getAllRoadSigns()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<RoadSign> roadsigns = new List<RoadSign>();
        string sqlQuery = "SELECT * FROM signalisations ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            double angle = reader.GetDouble(4);
            Point point = new Point(reader.GetDouble(2), 1.30, reader.GetDouble(3));
            string type = reader.GetString(1);
            RoadSign newRoadSign = new RoadSign(id,angle,point,type);
            roadsigns.Add(newRoadSign);
            //Debug.Log("id= " + id + "  nbVoieFT =" + nbVoieFT + "  nbVoieTF =" + nbVoieTF + " Vitesse =" + vitesse + " Hierarchie =" + hierarchie + " nomRue =" + nomRue);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return roadsigns;
    }

    public List<Flux> getAllFlux()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        List<Flux> flux = new List<Flux>();
        string sqlQuery = "SELECT * FROM fluxEntree ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int countFT = reader.GetInt32(1);
            int countTF = reader.GetInt32(2);
            int idConnection = reader.GetInt32(3);
            Flux newFlux = new Flux(id, countFT, countTF, idConnection);
            flux.Add(newFlux);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        return flux;
    }
}


