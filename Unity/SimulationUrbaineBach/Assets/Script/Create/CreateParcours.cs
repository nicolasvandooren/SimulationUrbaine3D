using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;

public class CreateParcours : MonoBehaviour {
    

    public Transform wayPoint;
    public Transform parent;
    private Transform ex_papa;
    
    
    public Transform[] createParcours(int idEdge, Transform[] ex_parcours, bool FT)
    {
        JSONNode jsonEdges = SimpleJSON.JSON.Parse(ReadFile(Config.pathEdge));
        int indexEdges = getIndexEdge(jsonEdges, idEdge);
        int nbPoints = jsonEdges["edges"][indexEdges]["point"].Count;
        Transform[] points = new Transform[nbPoints];
        if (ex_papa != null || ex_parcours != null)
            Destroy(ex_papa.gameObject);
        Transform papa = Instantiate(parent);
        if (FT) { 
            points = setPointFT(points, jsonEdges["edges"][indexEdges], papa);
        } else
        {
            points = setPointTF(points, jsonEdges["edges"][indexEdges], papa);
        }
        ex_papa = papa;
        return points;
    }
    private Transform[] setPointFT(Transform[] points, JSONNode jsonEdges, Transform papa)
    {
        float x, y, z;
        for (int i = 0; i < jsonEdges["point"].Count; i++)
        {
            x = jsonEdges["point"][i]["x"].AsFloat - (float)Config.x_0 + Config.size_voie*2;
            y = jsonEdges["point"][i]["y"].AsFloat;
            z = jsonEdges["point"][i]["z"].AsFloat - (float)Config.z_0;
            points[i] = Instantiate(wayPoint, new Vector3(x, y, z), transform.rotation) as Transform;
            points[i].parent = papa;

        }
        return points;
    }

    private Transform[] setPointTF(Transform[] points, JSONNode jsonEdges, Transform papa)
    {
        float x, y, z;
        int j = 0;
        for (int i = jsonEdges["point"].Count-1; i >= 0 ; i--)
        {
            x = jsonEdges["point"][i]["x"].AsFloat - (float)Config.x_0 + Config.size_voie*2;
            y = jsonEdges["point"][i]["y"].AsFloat;
            z = jsonEdges["point"][i]["z"].AsFloat - (float)Config.z_0 ;
            points[j] = Instantiate(wayPoint, new Vector3(x, y, z), transform.rotation) as Transform;
            points[j].parent = papa;
            j++;
        }
        return points;
    }
    private int getIndexEdge(JSONNode node, int idEdge)
    {
        
        for(int i = 0; i < node["edges"].Count; i++)
        {
            if (node["edges"][i]["id"].AsInt == idEdge)
                return i;
        }
        return -1;
    }

    string ReadFile(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string content = sr.ReadToEnd();
        sr.Close();
        return content;
    }

    public int getNbPoints(int idEdge)
    {
        JSONNode json = SimpleJSON.JSON.Parse(ReadFile(Config.pathEdge));
        int id = getIndexEdge(json, idEdge);
        return json["edges"][id]["point"].Count;
    }

    public float getSpeedByID(int idEdge)
    {
        JSONNode json = SimpleJSON.JSON.Parse(ReadFile(Config.pathEdge));
        int id = getIndexEdge(json, idEdge);
        return json["edges"][id]["vitesse"].AsFloat;
    }

    public int getNbFT(int idEdge)
    {
        JSONNode json = JSON.Parse(ReadFile(Config.pathEdge));
        int id = getIndexEdge(json, idEdge);
        if (id < 0)
        {
            return -1;
        } else { 
            return json["edges"][id]["nbVoieFT"].AsInt;
        }
    }

    public int getNbTF(int idEdge)
    {
        JSONNode json = JSON.Parse(ReadFile(Config.pathEdge));
        int id = getIndexEdge(json, idEdge);
        if (id < 0)
        {
            return -1;
        }
        else
        {
            return json["edges"][id]["nbVoieTF"].AsInt;
        }
    }
}
