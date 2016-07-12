using UnityEngine;
using System.Collections;

public class CreateCompteur : MonoBehaviour {
    public Transform cpt;
	public void createCompteur()
    {
        Instantiate(cpt, new Vector3(507.8266f, 0.42f, 29.5078f), cpt.rotation);
    }
}
