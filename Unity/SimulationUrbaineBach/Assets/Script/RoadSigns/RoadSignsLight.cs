using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadSignsLight : MonoBehaviour
{
    public GameObject lightRed;
    public GameObject lightYellow;
    public GameObject lightGreen;
    public int nbVoie;
    public List<float> timeLight;
    // Use this for initialization
    void Start ()
    { 
        if (timeLight.Count == 0) { 
            timeLight.Add(0);
            timeLight.Add(0);
            timeLight.Add(10);
            timeLight.Add(0);
            timeLight.Add(0);
        }
        switch (nbVoie)
        {
            case 1:
                setBox1Voie();
                break;
            case 2:
                setBox2Voie();
                break;
            case 3:
                setBox3Voie();
                break;
            case 4:
                setBox4Voie();
                break;
        }
        StartCoroutine(colorLight());
    }

    private void setBox4Voie()
    {
        this.GetComponent<BoxCollider>().center = new Vector3(-40, 0, -10);
        this.GetComponent<BoxCollider>().size = new Vector3(70, 2, 1);
    }
    private void setBox2Voie()
    {
        this.GetComponent<BoxCollider>().center = new Vector3(-20, 0, -10);
        this.GetComponent<BoxCollider>().size = new Vector3(30, 2, 1);
    }
    private void setBox3Voie()
    {
        this.GetComponent<BoxCollider>().center = new Vector3(-10, 0, -10);
        this.GetComponent<BoxCollider>().size = new Vector3(12, 2, 1);
    }
    private void setBox1Voie()
    {
        this.GetComponent<BoxCollider>().center = new Vector3(-10, 0, -10);
        this.GetComponent<BoxCollider>().size = new Vector3(12, 2, 1);
    }

    IEnumerator colorLight()
    {
        while (true)
        {
            int i = 0;
            foreach(float time in timeLight)
            {
                // %4 pour ajouter..
                switch(i % 3)
                {
                    //Rouge
                    case 0:
                        if (time != 0) { 
                            lightGreen.GetComponent<Renderer>().material.color = Color.white;
                            lightYellow.GetComponent<Renderer>().material.color = Color.white;
                            lightRed.GetComponent<Renderer>().material.color = Color.red;
                            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(this.gameObject.GetComponent<BoxCollider>().center.x, 0, this.gameObject.GetComponent<BoxCollider>().center.z);
                            yield return new WaitForSeconds(time);
                        }
                        break;
                    //jaune
                    case 1:
                        if (time != 0)
                        {
                            lightGreen.GetComponent<Renderer>().material.color = Color.white;
                            lightYellow.GetComponent<Renderer>().material.color = Color.yellow;
                            lightRed.GetComponent<Renderer>().material.color = Color.white;
                            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(this.gameObject.GetComponent<BoxCollider>().center.x, -5, this.gameObject.GetComponent<BoxCollider>().center.z);
                            yield return new WaitForSeconds(time);
                        }
                        break;
                    //Vert
                    case 2:
                        if (time != 0)
                        {
                            lightGreen.GetComponent<Renderer>().material.color = Color.green;
                            lightYellow.GetComponent<Renderer>().material.color = Color.white;
                            lightRed.GetComponent<Renderer>().material.color = Color.white;
                            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(this.gameObject.GetComponent<BoxCollider>().center.x, -5, this.gameObject.GetComponent<BoxCollider>().center.z);
                            yield return new WaitForSeconds(time);
                        }
                        break;
                    case 3:
                        if (time != 0)
                        {
                            lightGreen.GetComponent<Renderer>().material.color = Color.white;
                            lightYellow.GetComponent<Renderer>().material.color = Color.yellow;
                            lightRed.GetComponent<Renderer>().material.color = Color.white;
                            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(this.gameObject.GetComponent<BoxCollider>().center.x, -5, this.gameObject.GetComponent<BoxCollider>().center.z);
                            yield return new WaitForSeconds(time);
                        }
                        break;
                }
                i++;
            }

        }
        //yield return new WaitForSeconds(0.1f);

    }
}

