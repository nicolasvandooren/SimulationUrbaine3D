using UnityEngine;
using System.Collections;

public class calculCompteur : MonoBehaviour {
    public int cptCar = 0;
    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && !other.isTrigger)
            cptCar++;
    }
    
}
