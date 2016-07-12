using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
//Lire et écrire dans un fichier
using System.IO;

public class DataJson {
    private string path;
	public DataJson(string dataPath)
    {
        path = dataPath;
    }

    public void insertEdge(List<Edge> edge)
    {
        string pathEdge = path + "/edges.json";
        
        StreamWriter sw = new StreamWriter(pathEdge);
        insertDataEdge(sw, edge);
        sw.Close();
        Debug.Log("DONE EDGE");
    }

    //http://answers.unity3d.com/questions/1083933/simplejson-and-c-how-to-write-json-from-multiple-o.html
    private void insertDataEdge(StreamWriter sw, List<Edge> edge)
    {
        JSONNode nodeTitle = new JSONClass();
        JSONArray arrayEdge = new JSONArray();
        foreach (Edge e in edge)
            arrayEdge.Add(e.SaveToJSON());
        nodeTitle["edges"] = arrayEdge;
        string json = nodeTitle.ToString();
        sw.WriteLine(json);
    }

    public void insertVertex(List<Vertex> vertex)
    {
        string pathVertex = path + "/vertex.json";
        StreamWriter sw = new StreamWriter(pathVertex);
        insertDataVertex(sw, vertex);
        sw.Close();
        Debug.Log("DONE VERTEX");
    }

    private void insertDataVertex(StreamWriter sw, List<Vertex> vertex)
    {
        JSONNode nodeTitle = new JSONClass();
        JSONArray arrayVertex = new JSONArray();
        foreach (Vertex v in vertex)
            arrayVertex.Add(v.SaveToJSON());
        nodeTitle["vertex"] = arrayVertex;
        string json = nodeTitle.ToString();
        sw.WriteLine(json);
    }

    public void insertRoadSign(List<RoadSign> roadSigns)
    {
        string pathRoadSign = path + "/roadsign.json";
        StreamWriter sw = new StreamWriter(pathRoadSign);
        insertDataRoadSign(sw, roadSigns);
        sw.Close();
        Debug.Log("DONE ROAD SIGN");
    }

    private void insertDataRoadSign(StreamWriter sw, List<RoadSign> roadSigns)
    {
        JSONNode nodeTitle = new JSONClass();
        JSONArray arrayRoadSign = new JSONArray();
        foreach (RoadSign r in roadSigns)
            arrayRoadSign.Add(r.SaveToJSON());
        nodeTitle["roadsigns"] = arrayRoadSign;
        string json = nodeTitle.ToString();
        sw.WriteLine(json);
    }

    public void insertFlux(List<Flux> flux)
    {
        string pathFlux = path + "/flux.json";
        StreamWriter sw = new StreamWriter(pathFlux);
        insertDataFlux(sw, flux);
        sw.Close();
        Debug.Log("DONE ROAD SIGN");

    }
    
    private void insertDataFlux(StreamWriter sw, List<Flux> flux)
    {
        JSONNode nodeTitle = new JSONClass();
        JSONArray arrayFlux = new JSONArray();
        foreach (Flux f in flux)
            arrayFlux.Add(f.SaveToJSON());
        nodeTitle["flux"] = arrayFlux;
        string json = nodeTitle.ToString();
        sw.WriteLine(json);
    }

}

