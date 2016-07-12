using UnityEngine;
using System.Collections;

public class ShowInfoMainCam : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() // OnGUI is called twice per frame
    {
        //GUI.color = Color.yellow;
        //GUI.Box(new Rect(1550, 75, 220, 25), "Nombre de voiture en jeu : " + ObjectCount.countCarGame);

        showCount();
    }
    
    void showCount()
    {
        GUI.Box(new Rect(Screen.width-250, 50, 220, 25), "Nombre de voiture générée : " + ObjectCount.countCar);
        GUI.Box(new Rect(Screen.width - 250, 75, 220, 25), "Nombre de voiture en jeu : " + ObjectCount.countCarGame);
        GUI.Box(new Rect(Screen.width - 250, 100, 220, 25), "Nombre de voiture accidentée : " + ObjectCount.countCarAccident);
    }
}
