using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using System;
using System.Collections.Generic;

public class CreateCar : MonoBehaviour {
    
    public GameObject objCar;

    public void createCar()
    {
        JSONNode jsonFlux = JSON.Parse(ReadFile(Config.pathFlux));
        createFlux(jsonFlux);
        TabFlux.avgFlux();
        //Calcul de flux par un algo.
        //Dijkstra algo = new Dijkstra();
        Floyd algo = new Floyd();
        foreach (DataFlux fEntrante in TabFlux.tabFluxEntrant)
        {
            foreach(DataFlux fSortante in TabFlux.tabFluxSortant)
            {
                if (fEntrante.idEdge != fSortante.idEdge)
                {
                    //algo.dijkstra(fEntrante.idVertex, fSortante.idVertex);
                    algo.floyd(fEntrante.idVertex, fSortante.idVertex);
                }
            }
        }

        foreach(DataFlux fEntrante in TabFlux.tabFluxEntrant)
        {
            StartCoroutine(createFluxCar(fEntrante));
        }
    }


    //Flux entrant....
    IEnumerator createFluxCar(DataFlux flux)
    {
        JSONNode jsonEdge = JSON.Parse(ReadFile(Config.pathEdge));
        int i;
        bool ft = true;
        for (i = 0; i < jsonEdge["edges"].Count; i++)
        {
            if (jsonEdge["edges"][i]["id"].AsInt == flux.idEdge)
                break;
        }
        Vector3 positions = new Vector3(0,0,0);
        if (jsonEdge["edges"][i]["idVertexFirst"].AsInt == flux.idVertex)
        {
            positions = new Vector3(jsonEdge["edges"][i]["point"][0]["x"].AsFloat - (float)Config.x_0, jsonEdge["edges"][i]["point"][0]["y"].AsFloat, jsonEdge["edges"][i]["point"][0]["z"].AsFloat - (float)Config.z_0);
            ft = true;
        } else if (jsonEdge["edges"][i]["idVertexLast"].AsInt == flux.idVertex)
        {
            int nbPoint = jsonEdge["edges"][i]["point"].Count - 1;
            positions = new Vector3(jsonEdge["edges"][i]["point"][nbPoint]["x"].AsFloat - (float)Config.x_0, jsonEdge["edges"][i]["point"][nbPoint]["y"].AsFloat, jsonEdge["edges"][i]["point"][nbPoint]["z"].AsFloat - (float)Config.z_0);
            ft = false;
        }
        for (int j = 0; j < flux.count; j++)
        {
            //Mise en place de file d'attente
            while (FindClosestCar(positions))
            {
                yield return new WaitForSeconds(0.1f);
            }
            GameObject car = (GameObject)Instantiate(objCar, positions, transform.rotation);
            car.GetComponent<MoveCar>().FT = ft;
            car.GetComponent<MoveCar>().idBegin = flux.idVertex;
            car.GetComponent<MoveCar>().idEnd = TabFlux.getIdFluxSortant(flux);
            while (car.GetComponent<MoveCar>().idEnd == -1)
                car.GetComponent<MoveCar>().idEnd = TabFlux.getIdFluxSortant(flux);
            ObjectCount.countCar++;
            ObjectCount.countCarGame++;
            yield return new WaitForSeconds((float)flux.count/60f);
        }
    }

    bool FindClosestCar(Vector3 positions)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject go in gos)
        {
            //Calcule de distance entre les 2 objets
            float dist = Vector3.Distance(new Vector3(positions.x, positions.y, positions.z), go.transform.position);
            if (dist < 10f)
            {
                return true;
            }
        }
        return false;
    }
    private void createFlux(JSONNode json)
    {
        for (int i = 0; i < json["flux"].Count; i++)
        {
            ObjectEdge gEdge = GameObject.Find(json["flux"][i]["idEdge"]).GetComponent<ObjectEdge>();
            if (gEdge.nbVoieFT > 0 && gEdge.nbVoieTF > 0)
            {
                if (connexionEdgesWithVertex(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name)) < connexionEdgesWithVertex(gEdge.idVertexLast, Convert.ToInt32(gEdge.name)))
                {
                    if (json["flux"][i]["countTF"].AsInt != 0)
                        TabFlux.putFlux(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name), json["flux"][i]["countTF"].AsInt, true);

                    if (json["flux"][i]["countFT"].AsInt != 0)
                        TabFlux.putFlux(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name), json["flux"][i]["countFT"].AsInt, false);
                }
                else
                {
                    if (json["flux"][i]["countTF"].AsInt != 0)
                        TabFlux.putFlux(gEdge.idVertexLast, Convert.ToInt32(gEdge.name), json["flux"][i]["countTF"].AsInt, false);
                    if (json["flux"][i]["countFT"].AsInt != 0)
                        TabFlux.putFlux(gEdge.idVertexLast, Convert.ToInt32(gEdge.name), json["flux"][i]["countFT"].AsInt, true);
                }
            }
            else if (gEdge.nbVoieFT > 0 && gEdge.nbVoieTF == 0)
            {
                if (json["flux"][i]["countFT"].AsInt != 0) { 
                    if (connexionEdgesWithVertex(gEdge.idVertexLast, Convert.ToInt32(gEdge.name)) > 0)
                        TabFlux.putFlux(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name), json["flux"][i]["countFT"].AsInt, true);
                    else
                        TabFlux.putFlux(gEdge.idVertexLast, Convert.ToInt32(gEdge.name), json["flux"][i]["countFT"].AsInt, false);
                }
                //TabFlux.putFlux(gEdge.idVertexLast, Convert.ToInt32(gEdge.name), json["flux"][i]["countTF"].AsInt, false);
            }
            else if (gEdge.nbVoieFT == 0 && gEdge.nbVoieTF > 0)
            {
                //TabFlux.putFlux(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name), json["flux"][i]["countFT"].AsInt, false);
                if (json["flux"][i]["countTF"].AsInt != 0) { 
                    if (connexionEdgesWithVertex(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name)) > 0)
                        TabFlux.putFlux(gEdge.idVertexLast, Convert.ToInt32(gEdge.name), json["flux"][i]["countTF"].AsInt, true);
                    else
                        TabFlux.putFlux(gEdge.idVertexFirst, Convert.ToInt32(gEdge.name), json["flux"][i]["countTF"].AsInt, false);
                }
            }
        }
    }

    private int connexionEdgesWithVertex(int idVertex, int idEdge)
    {
        GameObject[] A = GameObject.FindGameObjectsWithTag("Edge");
        int count = 0;
        foreach (GameObject edge in A)
        {
            if (Convert.ToInt32(edge.name) != idEdge)
            {
                if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertex || edge.GetComponent<ObjectEdge>().idVertexLast == idVertex)
                    count++;
            }
        }
        return count;
    }

    string ReadFile(string path)
    {
        StreamReader sr = new StreamReader(Application.dataPath + path);
        string content = sr.ReadToEnd();
        sr.Close();
        return content;
    }
}
