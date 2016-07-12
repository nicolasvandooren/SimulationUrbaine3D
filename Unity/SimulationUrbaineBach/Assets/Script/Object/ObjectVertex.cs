using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectVertex: MonoBehaviour {
    public ArrayList tabVertex;
    
}
public class InfoVertex
{
    public int idVertex;
    public List<int> idConnect;
    public InfoVertex(int idVertex, List<int> idConnect)
    {
        this.idVertex = idVertex;
        this.idConnect = idConnect;
    }
}

