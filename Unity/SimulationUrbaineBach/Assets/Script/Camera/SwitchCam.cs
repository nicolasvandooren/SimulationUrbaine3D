using UnityEngine;
using System.Collections;

public class SwitchCam : MonoBehaviour
{
    private bool active = false;
    public Camera[] tab_cam;
    private int indexCameraCourante;
   
    void OnMouseDown()
    {
        if (active == false)
            active = true;
        
    }
    void Start()
    {

    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && active)
            active = false;
    }
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.B) && active)
        {
            gameObject.transform.Find("Camera1").GetComponent<Transform>().transform.LookAt(transform);
            gameObject.transform.Find("Camera1").GetComponent<Transform>().transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * 5.0f);
        } else if (Input.GetKey(KeyCode.N) && active)
        {
            gameObject.transform.Find("Camera1").GetComponent<Transform>().transform.LookAt(transform);
            gameObject.transform.Find("Camera1").GetComponent<Transform>().transform.RotateAround(transform.position, new Vector3(0.0f, -1.0f, 0.0f), 20 * Time.deltaTime * 5.0f);
        }
    }
}