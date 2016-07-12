using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System;
public class CreateRoad : MonoBehaviour {

    public Transform parentRoads;
    
    public void createRoad()
    {
        //http://wiki.unity3d.com/index.php?title=CreatePlane
        JSONNode json = JSON.Parse(ReadFile(Config.pathEdge));
        float y = (float)Config.y_0;
        
        Transform parent = Instantiate(parentRoads);
        for (int j = 0; j < json["edges"].Count; j++) { 
            Vector3 firstPosition = Vector3.zero;
            float nbVoie = json["edges"][j]["nbVoieFT"].AsFloat + json["edges"][j]["nbVoieTF"].AsFloat;
            for (int i = 0; i < json["edges"][j]["point"].Count; i++)
            {
                double x = Convert.ToDouble(json["edges"][j]["point"][i]["x"].AsFloat) - Config.x_0;
                double z = Convert.ToDouble(json["edges"][j]["point"][i]["z"].AsFloat) - Config.z_0;
                if (i == 0)
                    firstPosition = new Vector3((float)x, y, (float)z);

                Vector3 lastPosition = new Vector3((float)x, y, (float)z);

                if (firstPosition != lastPosition) {
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

                    Vector3 tmp = new Vector3((firstPosition.x + lastPosition.x) / 2, y, (firstPosition.z + lastPosition.z) / 2);
                    plane.transform.position = tmp;

                    float lenght = getDist(firstPosition.x, firstPosition.z, lastPosition.x, lastPosition.z);
                    plane.transform.localScale = new Vector3(nbVoie * Config.size_voie, 1, lenght / 5);

                    float angle = getAngle(firstPosition.x, firstPosition.z, lastPosition.x, lastPosition.z);
                    plane.transform.eulerAngles = new Vector3(0,angle,0);

                    firstPosition = lastPosition;
                    plane.transform.parent = parent;
                    plane.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
                }
            }
        }
    }
    float getDist(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2) - (x1), 2) + Mathf.Pow((y2) - (y1), 2));
    }

    //tan(α)=y1−y2/x2−x1
    //http://www.developpez.net/forums/d542849/general-developpement/algorithme-mathematiques/mathematiques/calcul-l-angle-d-vecteur/
    float getAngle(float x1, float y1, float x2, float y2)
    {
        float pi = 4 * Mathf.Atan(1);
        float w = (Mathf.Atan2((y2-y1), (x2-x1))+pi/2) * 180/pi;
        return w * -1;
    }
    string ReadFile(string path)
    {
        /*
        Debug.Log(path);
        TextAsset file = Resources.Load(path) as TextAsset;
        Debug.Log(file);
        string content = file.ToString();
        */
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string content = sr.ReadToEnd();
        sr.Close();
        
        return content;
    }
}
