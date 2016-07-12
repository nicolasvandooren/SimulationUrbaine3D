using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System;

public class CreateRoadSign : MonoBehaviour
{
    public GameObject objLight;
    public GameObject objCarrefour;
    public Transform parentRoadSigns;
    
    private GameObject carrefour;
    private Transform parent;

    public void createAllRoadSign()
    {
        JSONNode jsonRoadSign = JSON.Parse(ReadFile(Config.pathRoadSign));
        JSONNode jsonVertex = JSON.Parse(ReadFile(Config.pathVertex));
        Vector3 positions;
        parent = Instantiate(parentRoadSigns);
        for (int i = 0; i < jsonRoadSign["roadsigns"].Count; i++)
        {
            int id = 0;
            double xVertex = 0;
            double zVertex = 0;
            float dist = 0;
            double x = Convert.ToDouble(jsonRoadSign["roadsigns"][i]["position"][0]) - Config.x_0 ;
            double y = Convert.ToDouble(jsonRoadSign["roadsigns"][i]["position"][1]);
            double z = Convert.ToDouble(jsonRoadSign["roadsigns"][i]["position"][2]) - Config.z_0;

            for (int j = 0; j < jsonVertex["vertex"].Count; j++)
            {
                float tmp_dist = 0;
                tmp_dist = getDist(x, z, (Convert.ToDouble(jsonVertex["vertex"][j]["position"]["x"]) - Config.x_0), (Convert.ToDouble(jsonVertex["vertex"][j]["position"]["z"].AsFloat) - Config.z_0));
                
                if (dist == 0 || tmp_dist <= dist)
                {
                    dist = tmp_dist;
                    id = jsonVertex["vertex"][j]["id"].AsInt;
                    
                    xVertex = jsonVertex["vertex"][j]["position"]["x"].AsFloat - Config.x_0;
                    zVertex = jsonVertex["vertex"][j]["position"]["z"].AsFloat - Config.z_0;
                                        
                }

            }
            
            carrefour = GameObject.Find(id.ToString());
            if (carrefour == null)
            {
                carrefour = Instantiate(objCarrefour);
                carrefour.name = id.ToString();
                carrefour.transform.parent = parent;
                carrefour.transform.localPosition = new Vector3((float)xVertex, 0, (float)zVertex);
            }

            positions = new Vector3((float)x, (float)y, (float)z);
            
            GameObject newlight = (GameObject)Instantiate(objLight);
            newlight.name = jsonRoadSign["roadsigns"][i]["id"];
            newlight.transform.localPosition = positions;
            newlight.transform.localRotation = Quaternion.AngleAxis(jsonRoadSign["roadsigns"][i]["angle"].AsFloat, Vector3.up);
            if (jsonRoadSign["roadsigns"][i]["time"] != null)
            {
                for (int j = 0; j < jsonRoadSign["roadsigns"][i]["time"].Count; j++)
                    newlight.GetComponent<RoadSignsLight>().timeLight.Add(jsonRoadSign["roadsigns"][i]["time"][j].AsFloat);
                newlight.GetComponent<RoadSignsLight>().nbVoie = jsonRoadSign["roadsigns"][i]["NbVoie"].AsInt;
            }
            newlight.transform.parent = carrefour.transform;
        }
    }


    float getDist(double x1, double y1, double x2, double y2)
    {
        return Mathf.Sqrt(Mathf.Pow((float)(x2) - (float)(x1), 2) + Mathf.Pow((float)(y2) - (float)(y1), 2));
    }

    string ReadFile(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string content = sr.ReadToEnd();
        sr.Close();
        return content;
    }
}
