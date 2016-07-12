using UnityEngine;
using System.Collections;

public class ClickCamera : MonoBehaviour
{
    private bool active = false;
    private Color oldColor;
	// Use this for initialization
	void Start () {

    }
    void OnMouseDown()
    {
        //if ()
        if (active == false ) { 
            active = true;
            gameObject.transform.Find("CamCar").GetComponent<Camera>().enabled = true;
            gameObject.transform.Find("CamCar").GetComponent<Camera>().gameObject.SetActive(true);
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
            {
                if (t.name == "SkyCarBody") {
                    oldColor = t.GetComponent<Renderer>().material.color;
                    t.GetComponent<Renderer>().material.color = new Color(0.95f, 0, 0);
                    break;
                } 
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if ((Input.GetKeyDown(KeyCode.Space)) && active)
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
            {
                if (t.name == "SkyCarBody")
                {
                    t.GetComponent<Renderer>().material.color = oldColor;
                    break;
                }
            }
            gameObject.transform.Find("CamCar").GetComponent<Camera>().gameObject.SetActive(false);
            gameObject.transform.Find("CamCar").GetComponent<Camera>().enabled = false;
            active = false;
        }
    }

    void OnGUI() // OnGUI is called twice per frame
    {
        if (active)
        {
            showSpeed();
            showEdge();
        }
    }

    void showSpeed()
    {
        int vitesse = (int)(gameObject.GetComponent<MoveCar>().acceleration * 3.6);
        if (vitesse == 29 || vitesse == 30 || vitesse == 31)
            GUI.Label(new Rect(50, 50, 200, 25), "Vitesse Voiture : " + 30.ToString() + "km/h");
        else if (vitesse == 49 || vitesse == 50 || vitesse == 51)
            GUI.Label(new Rect(50, 50, 200, 25), "Vitesse Voiture : " + 50.ToString() + " km/h");
        else
            GUI.Label(new Rect(50, 50, 200, 25), "Vitesse Voiture : " + vitesse.ToString() + " km/h");
    }

    void showEdge()
    {
        ObjectEdge edge = gameObject.GetComponent<MoveCar>().objectVoie;
        GUI.Label(new Rect(50, 75, 200, 25), "Vitesse Voie : " + edge.vitesse.ToString() + " km/h");
        GUI.Label(new Rect(50, 100, 200, 25), "Nombre Voie FT : " + edge.nbVoieFT.ToString());
        GUI.Label(new Rect(50, 125, 200, 25), "Nombre Voie TF : " + edge.nbVoieTF.ToString());
        GUI.Label(new Rect(50, 150, 200, 25), "Longueur Voie : " + ((int)edge.longeur).ToString() + " mètres");
    }
}
