using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class MoveCar : MonoBehaviour {
    public int idBegin;
    public int idEnd;
    public bool FT;
    public Profil pCar;

    public int currentWayPoint = 1;
    private int currentEdge;
    private Transform targetWayPoint;
    private float vitesseVoie;
    public ObjectEdge objectVoie;
    private float posSizeVoieX = 0;
    private float posSizeVoieZ = 0;
    public Transform[] parcours;
    private List<GameObject> parcoursArete = new List<GameObject>();

    private float sizeVoie = 0;
    public float acceleration = 0;
    private bool accident = false;
    public bool stopDetect = false;
    private float timeStart = 0;
    private bool doneBox = false;
    private float waitTime = 0;
    private Collider dummy;
    public bool bouger = false;
    public bool lightDetect = false;

    private BlindSpot bSpot;
    private SafetyDistance sDist;

    public int nbTriggerEnter = 0;

    // Use this for initialization
    void Start () {
        gameObject.tag = "Car";
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.name == "SkyCarBody" ) {
                t.GetComponent<Renderer>().material.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                break;
            }
        }
        pCar = new Profil(0, 100, 0, 0, 0);
        bSpot = gameObject.GetComponentInChildren<BlindSpot>();
        sDist = gameObject.GetComponentInChildren<SafetyDistance>();

        parcoursArete = TabParcours.getTabEdge(idBegin, idEnd);

        init();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Car")
        {
            if (!collision.collider.isTrigger && accident == false ) {
                if (!this.gameObject.GetComponent<Rigidbody>().isKinematic) { 
                    if (!collision.gameObject.GetComponent<Rigidbody>().isKinematic) { 
                        ObjectCount.countCarAccident++;
                        accident = true;
                        StartCoroutine(timeDestroy(Random.Range(Config.timeAccidentMin, Config.timeAccidentMax)));
                    }
                }
            }
        }
    }

    //http://answers.unity3d.com/questions/542680/ontriggerexit-is-not-called-whthen-object-inside-i.html
    void OnTriggerEnter(Collider other)
    {
        /*if (other.tag == "Car" && other.isTrigger && this.GetComponent<Rigidbody>().isKinematic && !other.GetComponent<Rigidbody>().isKinematic)
            this.GetComponent<Rigidbody>().isKinematic = false;*/
        if (other.tag == "Car" && !other.isTrigger)
        {
            nbTriggerEnter++;
            sDist.unSetBoxReduce();
            dummy = other;
            waitTime = Random.Range(10f, 25f);
            /*if (!this.GetComponent<Rigidbody>().isKinematic && !this.GetComponent<Rigidbody>().isKinematic)
                stopDetect = true;*/
            stopDetect = true;
            timeStart = Time.time;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Dummy check si objet détruit ou pas
        if ((dummy != null) && stopDetect == true && other.tag == "Car" && !other.isTrigger)
        {
            //stopDetect = true;
            //Check bloquer dans file d attente
            if ((other.gameObject.transform.eulerAngles.y+15 > transform.eulerAngles.y) && (other.gameObject.transform.eulerAngles.y - 15 < transform.eulerAngles.y))
                bouger = false;
             else
            {
                if ((Time.time - timeStart + pCar.timeWait) > waitTime)
                {
                    if (!other.GetComponent<MoveCar>().bouger) { 
                        bouger = true;
                    //if (dummy.transform.position != other.transform.position) { 
                        sDist.setBoxReduce();
                        bSpot.setBoxAngleNul();
                        sDist.setBoxSecurity(5);
                        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
                        if (Time.time - timeStart > waitTime + 5)
                        {
                            ObjectCount.countCarGame--;
                            Destroy(this.gameObject);
                        }
                    }
                    //}
                }
            }
        } 
        else if (dummy == null) {
            /*if (nbTriggerEnter > 1)
                nbTriggerEnter--;
            if (nbTriggerEnter == 0)*/
                stopDetect = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Car" ) {
            if (dummy == other)
                dummy = null;
           if (!other.isTrigger) {
                /*if (nbTriggerEnter > 0)
                    nbTriggerEnter--;
                if (nbTriggerEnter == 0)*/
                    stopDetect = false;
            }
        }
    }

    IEnumerator timeDestroy(float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        ObjectCount.countCarGame--;
        Destroy(this.gameObject);
    }

    void init()
    {
        currentEdge = parcoursArete.Count - 1;
        objectVoie = parcoursArete[currentEdge].GetComponent<ObjectEdge>();

        parcours = parcoursArete[currentEdge].GetComponentsInChildren<Transform>();
        vitesseVoie = objectVoie.vitesse / 3.6f;
        if (idBegin == objectVoie.idVertexFirst)
            FT = true;
        else if (idBegin == objectVoie.idVertexLast)
            FT = false;
        if (FT && targetWayPoint == null)
            currentWayPoint = 1;
        if (!FT && targetWayPoint == null)
            currentWayPoint = parcours.Length - 1;
        if (targetWayPoint == null) { 
            targetWayPoint = parcours[currentWayPoint];
            changePosVoie(parcours[currentWayPoint]);
        }
    }

    void Update()
    {
        if (!accident) {
            move();
        } else
        {
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
            {
                if (t.name == "SkyCarBody")
                {
                    t.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                    break;
                }
            }
            acceleration = 0;
        }
    }


    void move()
    {
        setBoxPriority();
        sDist.setBoxSecurity(DistCarSecu(acceleration));
        if (!doneBox)
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        
        if (!stopDetect && !lightDetect) { 
            //Mouvement dans la direction du point
            if (acceleration > 0) { 
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetWayPoint.position.x + posSizeVoieX, targetWayPoint.position.y, targetWayPoint.position.z + posSizeVoieZ), acceleration * Time.deltaTime);
                transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(targetWayPoint.position.x - transform.position.x + posSizeVoieX, 0, targetWayPoint.position.z - transform.position.z + posSizeVoieZ), acceleration * Time.deltaTime, 0.0f);
            }
            //float dist = Vector2.Distance(new Vector2(targetWayPoint.position.x + posSizeVoieX, targetWayPoint.position.z + posSizeVoieZ), new Vector2(transform.position.x, transform.position.z));
            //if (Vector2.Distance(new Vector2(targetWayPoint.position.x + posSizeVoieX, targetWayPoint.position.z + posSizeVoieZ), new Vector2(transform.position.x, transform.position.z)) < 0.5f) { 
            //Check pour si la voiture est sur le point ou non
                if (!FT)
                {
                    if (((targetWayPoint.position.x ) <= (transform.position.x - posSizeVoieX + 0.3)) && ((targetWayPoint.position.x) >= (transform.position.x - posSizeVoieX - 0.3)))
                        changeWayPointTF();
                }
                if (FT)
                {
                    if ((targetWayPoint.position.x) <= (transform.position.x - posSizeVoieX + 0.3) && (targetWayPoint.position.x) >= (transform.position.x - posSizeVoieX - 0.3))
                        changeWayPointFT();
                }
            //}
        }

        //Detection voiture ou feu
        if (stopDetect || lightDetect )
        {
            freins();
        }
        else
        {
            if (vitesseVoie > acceleration)
                acceleration += 0.1f;
            else if (acceleration - 0.1f > 1f)
                acceleration -= 0.1f;
        }
        
        


    }

    //Changement de parcours
    void changeParcours()
    {
        int id = 0;
        if (FT)
            id = objectVoie.idVertexLast;
        if (!FT)
            id = objectVoie.idVertexFirst;
        doneBox = false;
        currentEdge--;
        if (currentEdge >= 0)
        {
            //TROP LOUD .... DONC UTILISATION parcoursArete Initialiser au debut
            //arete = GameObject.Find(parcoursEdge[currentEdge].ToString());
            objectVoie = parcoursArete[currentEdge].GetComponent<ObjectEdge>();
            vitesseVoie = objectVoie.vitesse / 3.6f;
            parcours = parcoursArete[currentEdge].GetComponentsInChildren<Transform>();
            
            if (id == objectVoie.idVertexFirst)
            {
                FT = true;
                currentWayPoint = 2;
            } else if (id == objectVoie.idVertexLast)
            {
                FT = false;
                currentWayPoint = parcours.Length - 2;
            }
            targetWayPoint = parcours[currentWayPoint];
            changePosVoie(parcours[currentWayPoint]);
        } else
        {
            ObjectCount.countCarGame--;
            Destroy(gameObject);
        }
    }

    //On créer la box de priorité si la voiture tourne à droite ou à gauche
    void setBoxPriority()
    {
        int lastPoint;
        int i = currentEdge;
        i--;
        if (FT) 
            lastPoint = parcours.Length - 1;
           // setBox(i, lastPoint);
        else
            lastPoint = 1;
           // setBox(i, lastPoint);
        
        //Si la voiture est à 15 mètre du "Carrefour"(WaypointFinal, donc point au milieu du carrefour)
        if (Vector3.Distance(transform.position, parcours[lastPoint].transform.position + new Vector3(posSizeVoieX, 0, posSizeVoieZ)) < 20 && !doneBox){
            if (angleTwoWaypoints(i) <= 10  && angleTwoWaypoints(i) > -70)
                setBoxRight();
            else if (angleTwoWaypoints(i) > 10)
                setBoxLeft();
            
            doneBox = true;
        }
    }
    //Identifiant du tableau parcours de la prochaine arete
    float angleTwoWaypoints(int nextEdge)
    {
        Transform tempTransform;
        float inversTemp = 1; float inversCurrent = 1;

        if (nextEdge >= 0)
        {
            int idFirstTemp = parcoursArete[nextEdge].GetComponent<ObjectEdge>().idVertexFirst;
            if (idFirstTemp == objectVoie.idVertexFirst || idFirstTemp == objectVoie.idVertexLast)
            { //FT
                tempTransform = parcoursArete[nextEdge].GetComponentsInChildren<Transform>()[1];
            }
            else
            {
                tempTransform = parcoursArete[nextEdge].GetComponentsInChildren<Transform>()[parcoursArete[nextEdge].GetComponentsInChildren<Transform>().Length - 1];
                inversTemp = -1;
            }
            if (!FT)
                inversCurrent = -1;
            float angle = Vector3.Angle(targetWayPoint.forward * inversCurrent, tempTransform.forward * inversTemp);
            Vector3 cross = Vector3.Cross(targetWayPoint.forward * inversCurrent, tempTransform.forward * inversTemp);
            if (cross.y > 0) angle = -angle;
            return angle;
            
        }
        return 0;
    }


    void setBoxRight()
    {
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(3, 0.5f, 9);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(8, 2, 16);
    }

    void setBoxLeft()
    {
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(-2, 0.5f, 11);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(20, 2, 20);
    }
    
    public void setBoxStop()
    {
        int tmp = currentEdge;
        float angle = angleTwoWaypoints(tmp - 1);
        if (angle < 25)
            setBoxRight();
        else if (angle > 25)
            setBoxLeft();
        else
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(2.4f, 2, 6);

    }

    public int randomPosNeg()
    {
        int r = (int)Random.Range(0, 2);
        if (r == 0)
            return -1;
        return r; 
    }

    void changeWayPointFT()
    {
        currentWayPoint++;
        if (currentWayPoint < parcours.Length-1)
        {
            targetWayPoint = parcours[currentWayPoint];
            setSizeXZ(parcours[currentWayPoint].eulerAngles.y, sizeVoie);
        } else
        {
            changeParcours();
        }
    }
    void changeWayPointTF()
    {
        currentWayPoint--;
        if (currentWayPoint > 1)
        {
            targetWayPoint = parcours[currentWayPoint];
            setSizeXZ(parcours[currentWayPoint].eulerAngles.y, sizeVoie);
        } else
        {
            changeParcours();
        }
    }

    void freins()
    {
        if ((acceleration - 2f) > 0)
        {
            acceleration -= 2F;
        }
        else
        {
            acceleration = 0;
        }
    }

    /**
     * methods DistCarSecu calcul the distance of security
     * @return float the distance of security.
     */
    float DistCarSecu(float vitesse)
    {
        return 5f / 9f * vitesse + 4f;
    }


    //Set la taille x, z de la voiture par rapport à la voie.
    void setSizeXZ(float angle, float posSizeVoieY)
    {
        if (angle <= 180)
        {
            if (angle > 90)
            {
                angle = 180 - angle;
                posSizeVoieZ = Mathf.Sin((angle * Mathf.PI) / 180) * posSizeVoieY;
                posSizeVoieX = Mathf.Cos((angle * Mathf.PI) / 180) * posSizeVoieY;
            }
            else
            {
                posSizeVoieZ = Mathf.Sin((angle * Mathf.PI) / 180) * posSizeVoieY;
                posSizeVoieX = Mathf.Cos((angle * Mathf.PI) / 180) * posSizeVoieY * -1;
            }
        }
        else if (angle > 180)
        {
            if (angle > 270)
            {
                angle = 360 - angle;
                posSizeVoieZ = Mathf.Sin((angle * Mathf.PI) / 180) * posSizeVoieY * -1;
                posSizeVoieX = Mathf.Cos((angle * Mathf.PI) / 180) * posSizeVoieY * -1;
            }
            else
            {
                angle = 270 - angle;
                posSizeVoieZ = Mathf.Sin((angle * Mathf.PI) / 180) * posSizeVoieY * -1;
                posSizeVoieX = Mathf.Cos((angle * Mathf.PI) / 180) * posSizeVoieY;
            }
        }
    }

    //Changement de voie
    void changePosVoie(Transform tWayPoint)
    {
        float tmpSizeVoie = sizeVoie;
        if (FT)
        {
            int tmp = currentEdge;
            if (objectVoie.nbVoieFT > 1 && objectVoie.nbVoieTF == 0)
            {
                int size = setSizeVoieFT();

                float angle = angleTwoWaypoints(tmp-1);
                if (angle < -20)
                    sizeVoie = (Config.size_voieCar * size) * -1;
                else if (angle > 20)
                    sizeVoie = (Config.size_voieCar * size);
                else
                    sizeVoie = (Config.size_voieCar * size) * randomPosNeg();

                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
            else if (objectVoie.nbVoieFT == 1 && objectVoie.nbVoieTF == 0)
            {
                posSizeVoieX = 0;
                posSizeVoieZ = 0;
                sizeVoie = 0;
            }
            else if (objectVoie.nbVoieFT > 0 && objectVoie.nbVoieTF > 0)
            {
                float angle = angleTwoWaypoints(tmp - 1);
                if (angle < -20)
                    sizeVoie = (-Config.size_voieCar * objectVoie.nbVoieFT);
                else if (angle > 20)
                    sizeVoie = (-Config.size_voieCar);
                else
                    sizeVoie = -Config.size_voieCar * Random.Range(1, objectVoie.nbVoieFT + 1);
                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
            else
            {
                sizeVoie = -Config.size_voieCar;
                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
        }
        if (!FT)
        {
            int tmp = currentEdge;
            if (objectVoie.nbVoieFT == 0 && objectVoie.nbVoieTF > 1)
            {
                int size = setSizeVoieTF();
                float angle = angleTwoWaypoints(tmp-1);
                if (angle < -20)
                    sizeVoie = (Config.size_voieCar * size) ;
                else if (angle > 20)
                    sizeVoie = (Config.size_voieCar * size) * -1;
                else
                    sizeVoie = (Config.size_voieCar * size) * randomPosNeg();

                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
            else if (objectVoie.nbVoieFT == 0 && objectVoie.nbVoieTF == 1)
            {
                posSizeVoieX = 0;
                posSizeVoieZ = 0;
                sizeVoie = 0;
            }
            else if (objectVoie.nbVoieFT > 0 && objectVoie.nbVoieTF > 0)
            {
                float angle = angleTwoWaypoints(tmp - 1);
                if (angle < -20)
                    sizeVoie = (Config.size_voieCar * objectVoie.nbVoieFT);
                else if (angle > 20)
                    sizeVoie = (Config.size_voieCar);
                else
                    sizeVoie = Config.size_voieCar * Random.Range(1, objectVoie.nbVoieFT + 1);
                //A définir la préséléction (Changer sizeVoie)
                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
            else
            {
                sizeVoie = Config.size_voieCar;
                setSizeXZ(tWayPoint.transform.eulerAngles.y, sizeVoie);
            }
        }

        if (tmpSizeVoie > sizeVoie)
            bSpot.setBoxAngleDroite();
        if (tmpSizeVoie < sizeVoie)
            bSpot.setBoxAngleGauche();
    }

    int setSizeVoieFT()
    {
        int size;
        if (objectVoie.nbVoieFT % 2 == 0)
        {
            size = Random.Range(0, objectVoie.nbVoieFT / 2);
            size += 1;
        }
        else
            size = Random.Range(0, objectVoie.nbVoieFT / 2 + 1);
        return size;
    }

    int setSizeVoieTF()
    {
        int size;
        if (objectVoie.nbVoieTF % 2 == 0)
        {
            size = Random.Range(0, objectVoie.nbVoieTF / 2);
            size += 1;
        }
        else
            size = Random.Range(0, objectVoie.nbVoieTF / 2 + 1);
        return size;
    }

    /**
     * methods FindClosestCar Check if a car is in the detect zone
     *arg : float distance, the distance we want make the detect zone
     *      int angle, it's the angle of the detect zone
     * @return bool true if a car is in the detect zone.
     */
    bool FindClosestCar(float distance, float angle)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject go in gos)
        {
            //The instance id of an object is always guaranteed to be unique.
            if (!GetHashCode().Equals(go.GetHashCode()))
            {
                //Calcule de distance entre les 2 objets
                float dist = Vector3.Distance(transform.position, go.transform.position);
                //Si la voiture est en face
                if (dist != 0F)
                {
                    float calc_angle = Vector3.Angle(transform.forward, go.transform.position - transform.position);
                    if (dist < (distance+1) && (calc_angle) < angle)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Vector3 GetPosLastPoint()
    {
        if (FT)
            return parcours[parcours.Length - 1].transform.position;
        if (!FT)
            return parcours[1].transform.position;
        return Vector3.zero;
    }

}
