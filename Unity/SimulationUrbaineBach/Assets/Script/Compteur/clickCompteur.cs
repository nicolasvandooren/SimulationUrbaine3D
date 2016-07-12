using UnityEngine;
using System.Collections;

public class clickCompteur : MonoBehaviour {

    private bool active = false;
    private float timeStart;
    private float debit = 0;
    private int cpt = 0;
    void Start()
    {
        timeStart = Time.time;
    }
    void OnMouseDown()
    {
        if (active == false)
        {
            active = true;
            gameObject.transform.Find("Camera1").GetComponent<Camera>().enabled = true;
            gameObject.transform.Find("Camera1").GetComponent<Camera>().gameObject.SetActive(true);
            
        }
    }

    // Update is called once per frame
    void Update () {
        if ((Input.GetKeyDown(KeyCode.Space)) && active)
        {
            gameObject.transform.Find("Camera1").GetComponent<Camera>().gameObject.SetActive(false);
            gameObject.transform.Find("Camera1").GetComponent<Camera>().enabled = false;
            active = false;
        }
        if (((int)(Time.time - timeStart)) % 60 == 0)
            debit = (cpt / (Time.time - timeStart)) * 3600;
    }


    void OnGUI() // OnGUI is called twice per frame
    {
        if (active)
        {
            showCarInfo();
        }
    }

    void showCarInfo()
    {
        cpt = gameObject.GetComponent<calculCompteur>().cptCar;
        
        GUI.Box(new Rect(50, 50, 300, 25), "Comptage voiture : " + cpt.ToString() + " en " + (int)(Time.time - timeStart) + " secondes");
        GUI.Box(new Rect(50, 75, 300, 25), "Débit voiture : " + debit + " Voitures/Heures");
        /*GUI.Label(new Rect(50, 100, 200, 25), "Nombre Voie FT : " + edge.nbVoieFT.ToString());
        GUI.Label(new Rect(50, 125, 200, 25), "Nombre Voie TF : " + edge.nbVoieTF.ToString());
        GUI.Label(new Rect(50, 150, 200, 25), "Longueur Voie : " + ((int)edge.longeur).ToString() + " mètres");*/
    }
}
