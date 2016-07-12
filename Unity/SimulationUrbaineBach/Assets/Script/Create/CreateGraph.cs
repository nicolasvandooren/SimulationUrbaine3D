using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;

public class CreateGraph : MonoBehaviour {
    public Transform wayPoint;
    public Transform parent;
    public GameObject gameObjectEdge;

    private GameObject pEdge;
    private ObjectEdge objetEdge;
    private ObjectVertex objetVertex;
    //private int[] tabVertex;

    public void createGraph()
    {
        /////////////////////////////INIT VERTEX ///////////////////////////////
        JSONNode jsonVertex = SimpleJSON.JSON.Parse(ReadFile(Config.pathVertex));
        Transform papa = Instantiate(parent);
        papa.tag = "TabVertex";
        objetVertex = papa.GetComponent<ObjectVertex>();
        objetVertex.tabVertex = new ArrayList();
        for(int i = 0; i < jsonVertex["vertex"].Count; i++) {
            List<int> idConnects = new List<int>();
            for(int j = 0; j < jsonVertex["vertex"][i]["connect"].Count; j++)
            {
                idConnects.Add(jsonVertex["vertex"][i]["connect"][j]["id_connect"].AsInt);
            }
            objetVertex.tabVertex.Add(new InfoVertex(jsonVertex["vertex"][i]["id"].AsInt, idConnects));
            
        }

        /////////////////////////////INIT EDGE ///////////////////////////////
        JSONNode jsonEdges = SimpleJSON.JSON.Parse(ReadFile(Config.pathEdge));
        for (int i = 0; i < jsonEdges["edges"].Count; i++)
        {
            pEdge = Instantiate(gameObjectEdge);
            pEdge.tag = "Edge";
            objetEdge = pEdge.GetComponent<ObjectEdge>();
            objetEdge.idVertexFirst = jsonEdges["edges"][i]["idVertexFirst"].AsInt;
            objetEdge.idVertexLast = jsonEdges["edges"][i]["idVertexLast"].AsInt;
            objetEdge.nbVoieFT = jsonEdges["edges"][i]["nbVoieFT"].AsInt;
            objetEdge.nbVoieTF = jsonEdges["edges"][i]["nbVoieTF"].AsInt;
            objetEdge.vitesse = jsonEdges["edges"][i]["vitesse"].AsInt;
            pEdge.name = jsonEdges["edges"][i]["id"];
            pEdge.transform.parent = papa;
            int nbPoint = jsonEdges["edges"][i]["point"].Count;
            Transform[] points = new Transform[nbPoint];
            float tmp_x = 0, tmp_z = 0;
            float dist = 0;
            float distTest = 0;
            for (int j = 0; j < nbPoint; j++)
            {
                float x, y, z;
                x = (float)(jsonEdges["edges"][i]["point"][j]["x"].AsDouble - Config.x_0);
                y = 0;
                z = (float)(jsonEdges["edges"][i]["point"][j]["z"].AsDouble - Config.z_0);

                if (x != tmp_x && z != tmp_z && j != 0)
                {
                    dist += getDist(x, z, tmp_x, tmp_z);
                    distTest = getDist(x, z, tmp_x, tmp_z);
                    float angle = getAngle(x, z, tmp_x, tmp_z);
                    transform.eulerAngles = new Vector3(0, angle, 0);
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                    //Ajout des points
                    if (distTest > 10)
                    {
                        //Gérer cas ou c'est le derniers
                        if (j == 1) {

                            /*
                            if (points[j - 1].transform.position.x < x)
                                xTest = points[j - 1].transform.position.x - Mathf.Cos((angle * Mathf.PI) / 180) * 5;
                            else
                                xTest = points[j - 1].transform.position.x + Mathf.Cos((angle * Mathf.PI) / 180) * 5;
                            
                            if (points[j - 1].transform.position.z < z)
                                zTest = points[j - 1].transform.position.z - Mathf.Sin((angle * Mathf.PI) / 180) * 5 ;
                            else
                                zTest = points[j - 1].transform.position.z + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                            */
                            Vector3 testVector = points[j - 1].transform.position + (new Vector3(x, y, z) - points[j - 1].transform.position) * 6 * (1 / distTest);
                            /*
                            float xTest = points[j - 1].transform.position.x + ((x - points[j - 1].transform.position.x) / 8);
                            float zTest = points[j - 1].transform.position.z + ((z - points[j - 1].transform.position.z) / 8);
                            */
                            //Transform test = Instantiate(wayPoint, new Vector3(xTest, y, zTest), transform.rotation) as Transform;

                            Transform test = Instantiate(wayPoint, testVector, transform.rotation) as Transform;
                            test.name = "PointAdd";
                            test.tag = "Point";
                            test.parent = pEdge.transform;
                        }
                        if (j == nbPoint - 1)
                        {
                            Vector3 testVector;
                            /*
                            float xTest = x + (( points[j - 1].transform.position.x - x) / 8);
                            float zTest = z + (( points[j - 1].transform.position.z - z) / 8);
                            testVector = new Vector3(xTest, y, zTest);
                            */
                            testVector = new Vector3(x, y, z) - (new Vector3(x, y, z) - points[j - 1].transform.position) * 6 * (1 / distTest);
                            /*float newAngle = 0;
                            Quaternion testAngle;
                            if (transform.rotation.y > points[j - 1].transform.rotation.y)
                            {
                                newAngle = points[j - 1].transform.rotation.y + (transform.rotation.y - points[j - 1].transform.rotation.y)/2;
                                testAngle = Quaternion.AngleAxis(newAngle, Vector3.up);
                            }
                            else if (transform.rotation.y < points[j - 1].transform.rotation.y)
                            {
                                if (pEdge.name == "4804")
                                {
                                    Debug.Log(transform.rotation.y);
                                    Debug.Log(points[j - 1].transform.rotation.y);
                                }
                                newAngle = transform.rotation.y + (points[j - 1].transform.rotation.y - transform.rotation.y)/2;
                                testAngle = Quaternion.AngleAxis(newAngle, Vector3.up);
                            }
                            else
                            {
                                testAngle = transform.rotation;
                            }*/
                            Transform test = Instantiate(wayPoint, testVector, transform.rotation) as Transform;
                            test.name = "PointAdd";
                            test.tag = "Point";
                            test.parent = pEdge.transform;
                        }
                    }
                    if (j == 1) { 
                        points[0].transform.eulerAngles = new Vector3(0, angle, 0);
                        points[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                    }
                    tmp_x = x; tmp_z = z;
                }

                points[j] = Instantiate(wayPoint, new Vector3(x, y, z), transform.rotation) as Transform;
                points[j].transform.parent = pEdge.transform;
                points[j].tag = "Point";

                if (j == 0)
                {
                    points[j].tag = "Vertex";
                    tmp_x = x;
                    tmp_z = z;
                }
                if (j == points.Length-1)
                    points[j].tag = "Vertex";
                
            }
            objetEdge.longeur = dist;
            objetEdge.initPoints();
        }
    }

    float getDist(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2) - (x1), 2) + Mathf.Pow((y2) - (y1), 2));
    }
    //http://gamedev.stackexchange.com/questions/69679/convert-atan2-value-to-standard-360-degree-system-value
    float getAngle(float x1, float y1, float x2, float y2)
    {
        float pi = 4 * Mathf.Atan(1);
        float w = (Mathf.Atan2((y2 - y1),(x2 - x1)) + pi / 2) * 180 / pi;
        /*if (w < 0)
            return (w + 360);
        */
        //w = (w + 720) % 360;
        return w * -1;
    }

    string ReadFile(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string content = sr.ReadToEnd();
        sr.Close();
        return content;
    }
}
