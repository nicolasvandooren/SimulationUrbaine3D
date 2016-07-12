using UnityEngine;
using System.Collections;

public class BlindSpot : MonoBehaviour {
    private Vector3 lastPoint;
    private Vector3 myPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && !other.isTrigger && !this.GetComponentInParent<MoveCar>().stopDetect) {
            lastPoint = this.GetComponentInParent<MoveCar>().GetPosLastPoint();
            myPoint = this.GetComponentInParent<Transform>().parent.position;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Car" && !other.isTrigger) { 
            float distOther = Vector3.Distance(other.transform.position, lastPoint);
            float dist = Vector3.Distance(myPoint, lastPoint);
            if (dist < distOther && this.GetComponentInParent<MoveCar>().stopDetect) {
                this.GetComponentInParent<MoveCar>().stopDetect = false;
            }
        }
    }

    public void setBoxAngleNul()
    {
        this.gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
        this.gameObject.GetComponent<BoxCollider>().size = Vector3.zero;
    }

    public void setBoxAngleDroite()
    {
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(2, 1, -1f);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1.5f, 2, 4);
    }

    public void setBoxAngleGauche()
    {
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(-2, 1, -1f);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1.5f, 2, 4);

    }
}
