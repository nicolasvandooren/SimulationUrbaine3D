using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TabParcours
{
    public static ArrayList tabParcours = new ArrayList();

    public static void putParcours(int idBegin, int idEnd, List<GameObject> tabEdge)
    {
        ObjectParcours o = new ObjectParcours(idBegin, idEnd, tabEdge);
        tabParcours.Add(o);
    }

    public static List<GameObject> getTabEdge(int idBegin, int idEnd)
    {
        foreach (ObjectParcours oDijkstra in tabParcours)
        {
            if (oDijkstra.idBegin == idBegin && oDijkstra.idEnd == idEnd)
            {
                return oDijkstra.tabEdge;
            }
        }
        return null;
    }

}
public class ObjectParcours
{
    public int idBegin;
    public int idEnd;
    public List<GameObject> tabEdge;

    public ObjectParcours()
    {
        //tabDijkstra = new ArrayList();
    }

    public ObjectParcours(int idBegin, int idEnd, List<GameObject> tabEdge)
    {
        this.idBegin = idBegin;
        this.idEnd = idEnd;
        this.tabEdge = tabEdge;

    }
}