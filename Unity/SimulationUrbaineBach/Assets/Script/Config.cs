using UnityEngine;
using System.Collections;

public static class Config
{
    public static double x_0 = 2500495.7902;
    public static double y_0 = 0;
    public static double z_0 = 1116111.3762;
    public static float size_voie = 0.75f;
    public static string pathEdge = "/JSONFile/edges.json";
    public static string pathFlux = "/JSONFile/flux.json";
    public static string pathRoadSign = "/JSONFile/roadsign.json";
    public static string pathVertex = "/JSONFile/vertex.json";
    public static string pathBDD = "/DB/bddV2.db";
    public static int nbCar = 1000;
    public static float size_voieCar = 4f;
    public static float timeAccidentMin = 10f;
    public static float timeAccidentMax = 20f;
}
