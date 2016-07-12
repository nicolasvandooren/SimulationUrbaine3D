using UnityEngine;
using System.Collections;

public class Profil {
    public int acceleration;
    public int accelerationPourcentage;
    public int distance;
    public float timeStop;
    //Peut pas être supérieur à 15s
    public float timeWait;

    public Profil(int acceleration, int accelerationPourcentage, int distance, float timeStop, float timeWait)
    {
        this.acceleration = acceleration;
        this.accelerationPourcentage = accelerationPourcentage;
        this.distance = distance;
        this.timeStop = timeStop;
        this.timeWait = timeWait;
    }
}
