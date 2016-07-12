using UnityEngine;
using System.Collections;

public class SafetyDistance : MonoBehaviour {
    private bool reduce = false;
    private Collider dummy;
	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0.5f, 3);
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "traffic_light" )
        {
            this.GetComponentInParent<MoveCar>().lightDetect = true;
            //this.GetComponentInParent<MoveCar>().stopDetect = false;
        }
        if (other.tag == "Stop" && other != dummy)
            this.GetComponentInParent<MoveCar>().lightDetect = true;
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Car" && !other.isTrigger)
            this.GetComponentInParent<MoveCar>().stopDetect = true;
        if (other.tag == "Stop" && other != dummy) { 
            this.GetComponentInParent<MoveCar>().lightDetect = true;
            dummy = other;
            StartCoroutine(waitStop());
        }
    }

    IEnumerator waitStop()
    {
        this.GetComponentInParent<MoveCar>().setBoxStop();
        yield return new WaitForSeconds(2 + this.GetComponentInParent<MoveCar>().pCar.timeStop);
        this.GetComponentInParent<MoveCar>().lightDetect = false;
    }
    

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "traffic_light")
            this.GetComponentInParent<MoveCar>().lightDetect = false;
        if (other.tag == "Car" && !other.isTrigger)
            this.GetComponentInParent<MoveCar>().stopDetect = false;
    }

    public void setBoxSecurity(float distZ)
    {
        if (reduce) { 
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(2.4f, 2, distZ);
        } else
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(4, 2, distZ);
    }

    public void setBoxReduce()
    {
        reduce = true;
    }

    public void unSetBoxReduce()
    {
        reduce = false;
    }
}
