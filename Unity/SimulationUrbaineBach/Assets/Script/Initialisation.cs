using UnityEngine;
using System.Data;
using System.Collections.Generic;

//http://forum.unity3d.com/threads/tutorial-how-to-integrate-sqlite-in-c.192282/
//http://answers.unity3d.com/questions/743400/database-sqlite-setup-for-unity.html

public class Initialisation : MonoBehaviour {
    public Transform create_roadsigns;
    public Transform create_road;
    public Transform create_flux;
    public Transform create_graph;
    public Transform create_cpt;

    private Bdd myBdd;
    

    private Transform t_RoadSign;
    private CreateRoadSign roadSign;

    private Transform t_Road;
    private CreateRoad road;

    private Transform t_Graph;
    private CreateGraph graph;

    private Transform t_Flux;
    private CreateCar fluxCar;

    private Transform t_Compteur;
    private CreateCompteur compteur;

    // Use this for initialization
    void Start () {
        /*
        List<Edge> edges = new List<Edge>();
        List<Vertex> vertex = new List<Vertex>();
        List<RoadSign> roadsigns = new List<RoadSign>();
        List<Flux> flux = new List<Flux>();
        myBdd = new Bdd(Application.dataPath + Config.pathBDD);
        edges = myBdd.getAllEdges();
        vertex = myBdd.getAllVertex();
        roadsigns = myBdd.getAllRoadSigns();
        flux = myBdd.getAllFlux();
        myBdd.closeConnection();
        

        
        DataJson json = new DataJson(Application.dataPath + "/JSONFile");
        json.insertEdge(edges);
        json.insertVertex(vertex);
        json.insertRoadSign(roadsigns);
        json.insertFlux(flux);
        */
        
        /*
        my_parcours = Instantiate(create_parcours);
        waypoints = my_parcours.GetComponent<CreateParcours>();
        wayPointList = waypoints.createParcours(4681, wayPointList);
        Debug.Log(waypoints.getSpeedByID(4681));
        Debug.Log(waypoints.getSpeedByID(4682));
        Debug.Log(waypoints.getNbFT(4683));
        Debug.Log(waypoints.getNbTF(4683));
        */
        
        t_RoadSign = Instantiate(create_roadsigns);
        roadSign = t_RoadSign.GetComponent<CreateRoadSign>();
        roadSign.createAllRoadSign();
        Destroy(t_RoadSign.gameObject);
        //Debug.Log("RoadSign OK ");

        t_Road = Instantiate(create_road);
        road = t_Road.GetComponent<CreateRoad>();
        road.createRoad();
        Destroy(t_Road.gameObject);
        //Debug.Log("Road OK ");

        t_Graph = Instantiate(create_graph);
        graph = t_Graph.GetComponent<CreateGraph>();
        graph.createGraph();
        Destroy(t_Graph.gameObject);
        //Debug.Log("Graph OK ");
        t_Compteur = Instantiate(create_cpt);
        compteur = t_Compteur.GetComponent<CreateCompteur>();
        compteur.createCompteur();
        Destroy(t_Compteur.gameObject);

        t_Flux = Instantiate(create_flux);
        fluxCar = t_Flux.GetComponent<CreateCar>();
        fluxCar.createCar();
        //Destroy(flux.gameObject);
        //Floyd f = new Floyd();
        //f.floyd(7217, 7221);
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
