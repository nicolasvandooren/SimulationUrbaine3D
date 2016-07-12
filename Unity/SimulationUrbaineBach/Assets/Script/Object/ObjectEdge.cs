using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectEdge : MonoBehaviour {

    public int nbVoieFT;
    /*{
        get
        {
            return nbVoieTF;
        }
        set
        {
            nbVoieTF = value;
        }
    }*/
    public int nbVoieTF;
    public float longeur;
    public int idVertexFirst;
    public int idVertexLast;
    public int vitesse;

    private Transform[] points;

    public void initPoints()
    {
        points = this.transform.GetComponentsInChildren<Transform>();
    }

    public List<Transform> getConnexionFirst()
    {
        List<Transform> tConnect = new List<Transform>();
        //points = this.transform.GetComponentsInChildren<Transform>();
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Vertex");
        foreach (GameObject go in gos)
        {
            if(go.transform.position.x == points[1].position.x && go.transform.position.z == points[1].position.z)
            {
                if (go.transform.parent.name != this.name)
                    tConnect.Add(go.transform.parent);
            }
        }
        return tConnect;
    }

    public List<Transform> getConnexionLast()
    {
        List<Transform> tConnect = new List<Transform>();
        //points = this.GetComponentsInChildren<Transform>();
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Vertex");
        foreach (GameObject go in gos)
        {
            if (go.transform.position.x == points[points.Length-1].position.x && go.transform.position.z == points[points.Length-1].position.z)
            { 
                if (go.transform.parent.name != this.name)
                    tConnect.Add(go.transform.parent);
            }
        }
        return tConnect;
    }

    public List<Transform> getConnexionByPositionFirst()
    {
        Collider[] colliders;
        List<Transform> tConnect = new List<Transform>();
        if ((colliders = Physics.OverlapSphere(points[1].position, 0.1f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
        {
            foreach (var collider in colliders)
            {
                GameObject go = collider.gameObject; //This is the game object you collided with
                if (go.tag == "Vertex" && go.transform.parent.name != this.name)
                    tConnect.Add(go.transform.parent);
                    //Debug.Log(go.transform.parent.name);
            }
        }
        return tConnect;
    }
    public List<Transform> getConnexionByPositionLast()
    {
        Collider[] colliders;
        List<Transform> tConnect = new List<Transform>();
        if ((colliders = Physics.OverlapSphere(points[points.Length-1].position, 0.1f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
        {
            foreach (var collider in colliders)
            {
                GameObject go = collider.gameObject; //This is the game object you collided with
                if (go.tag == "Vertex" && go.transform.parent.name != this.name)
                    tConnect.Add(go.transform.parent);
                //Debug.Log(go.transform.parent.name);
            }
        }
        return tConnect;
    }

}
