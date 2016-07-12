using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Dijkstra 
{

    private int[] d;
    private ArrayList S;
    private GameObject[] A;
    private ObjectVertex objectVertex;
    private ArrayList tabPoid;
    private ArrayList tabPrec;
    public class ObjectTabPoid
    {
        public int idVertex;
        public float poid;
        public bool done;
        public ObjectTabPoid(int idVertex, int poid, bool done)
        {
            this.idVertex = idVertex;
            this.poid = poid;
            this.done = done;
        }
    }

    public class ObjectTabPrec
    {
        public int idVertex;
        public int idVertexPrec;
        public int idEdge;
        public ObjectTabPrec(int idVertex, int idVertexPrec, int idEdge)
        {
            this.idVertex = idVertex;
            this.idVertexPrec = idVertexPrec;
            this.idEdge = idEdge;
        }
    }

    private void initDijkstra(int idVertex)
    {
        objectVertex = GameObject.FindWithTag("TabVertex").GetComponent<ObjectVertex>();
        S = objectVertex.tabVertex;

        A = GameObject.FindGameObjectsWithTag("Edge");
        tabPoid = new ArrayList();
        tabPrec = new ArrayList();
        foreach(InfoVertex s in S)
        {
            if (s.idVertex == idVertex)
                tabPoid.Add(new ObjectTabPoid(s.idVertex, 0, false));
            else
                tabPoid.Add(new ObjectTabPoid(s.idVertex, -1, false));
            tabPrec.Add(new ObjectTabPrec(s.idVertex, -1, -1));
        }
    }

    public List<GameObject> dijkstra(int idVertexBegin, int idVertexEnd)
    {
        List<GameObject> lEdge = new List<GameObject>();
        if ((lEdge = TabParcours.getTabEdge(idVertexBegin, idVertexEnd)) != null)
        {
            return lEdge;
        }

        int idVertex = idVertexBegin;
        initDijkstra(idVertex);
        InfoVertex vertex = getVertex(idVertex);
        List<GameObject> edges = new List<GameObject>();
        //Debug.Log(vertex.idConnect.Count);
        while(idVertex != idVertexEnd && idVertex != -1) { 
            foreach(int idConnect in vertex.idConnect)
            {
                edges = getEdgeByVertex(idVertex, idConnect);
                foreach (GameObject edge in edges)
                {
                    float poid = edge.GetComponent<ObjectEdge>().longeur;
                    if (!idVertexParcouru(idConnect) && (
                        (getPoidByIdVertex(idVertex) + poid) < getPoidByIdVertex(idConnect) || getPoidByIdVertex(idConnect) < 0)) { 
                        setTabPrec(idConnect, idVertex, int.Parse(edge.name));
                        setTabPoid(idConnect, getPoidByIdVertex(idVertex) + poid, false);
                    }
                }
            }
            setTabPoid(idVertex, getPoidByIdVertex(idVertex), true);
            idVertex = getIdLowPoid();
            //Debug.Log(idVertex);
            vertex = getVertex(idVertex);
            //Debug.Log(vertex.idConnect.Count);
        }
        /*
        Debug.Log(idVertex);
        Debug.Log(getPoidByIdVertex(idVertexEnd));

        foreach (ObjectTabPoid t in tabPoid) {
            Debug.Log(t.idVertex + " " + t.poid + " " + t.done);
        }
        foreach (ObjectTabPrec t in tabPrec)
        {
            Debug.Log(t.idVertex + " " + t.idVertexPrec + " " + t.idEdge);
        }

        Debug.Log(idVertex);
        */
        lEdge = getParcours(idVertexBegin,idVertexEnd);
        TabParcours.putParcours(idVertexBegin, idVertexEnd, lEdge);
        return lEdge;

    }

    private List<GameObject> getParcours(int idVertexBegin,int idVertexEnd)
    {
        int idVertex = idVertexEnd;
        //List<int> lEdge = new List<int>();
        List<GameObject> lEdgeGO = new List<GameObject>();
        
        while (idVertex != idVertexBegin && idVertex != -1)
        {
            foreach (ObjectTabPrec prec in tabPrec)
            {
                if (prec.idVertex == idVertex && idVertex != -1) {
                    lEdgeGO.Add(GameObject.Find(prec.idEdge.ToString()));
                    //lEdge.Add(prec.idEdge);
                    idVertex = prec.idVertexPrec;
                    break;
                }
            }
        }
        return lEdgeGO;
    }


    private int getIdLowPoid()
    {
        float tmpPoid = -1;
        int id = -1;
        for (int i = 0; i < tabPoid.Count; i++)
        {
            ObjectTabPoid tPoid = (ObjectTabPoid)tabPoid[i];
            if (!tPoid.done)
            {
                if ((tmpPoid > tPoid.poid || tmpPoid == -1) && tPoid.poid != -1)
                {
                    tmpPoid = tPoid.poid;
                    id = tPoid.idVertex;
                }
            }
        }
        return id;
    }

    private float getPoidByIdVertex(int idVertex)
    {
        for (int i = 0; i < tabPoid.Count; i++)
        {
            ObjectTabPoid tPoid = (ObjectTabPoid)tabPoid[i];
            if (tPoid.idVertex == idVertex)
            {
                return tPoid.poid;
            }
        }
        return 1;
    }
    private bool idVertexParcouru(int idVertex)
    {
        for (int i = 0; i < tabPoid.Count; i++)
        {
            ObjectTabPoid tPoid = (ObjectTabPoid)tabPoid[i];
            if (tPoid.idVertex == idVertex)
            {
                return tPoid.done;
            }
        }
        return false;
    }
    private void setTabPrec(int idVertex, int idVertexPrec, int idEdge)
    {
        for(int i = 0; i < tabPrec.Count; i++)
        {
            ObjectTabPrec tPrec = (ObjectTabPrec)tabPrec[i];
            if (tPrec.idVertex == idVertex) { 
                tPrec.idVertexPrec = idVertexPrec;
                tPrec.idEdge = idEdge;
                tabPrec[i] = tPrec;
                break;
            }
        }
    }

    private void setTabPoid(int idVertex, float poid, bool done)
    {
        for (int i = 0; i < tabPoid.Count; i++)
        {
            ObjectTabPoid tPoid = (ObjectTabPoid)tabPoid[i];
            if (tPoid.idVertex == idVertex)
            {
                tPoid.poid = poid;
                tPoid.done = done;
                tabPoid[i] = tPoid;
                break;
            }
        }
    }
    private List<GameObject> getEdgesWithVertex(int idVertex)
    {
        List<GameObject> edges = new List<GameObject>();
        foreach (GameObject edge in A)
        {
            if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertex || edge.GetComponent<ObjectEdge>().idVertexLast == idVertex)
                edges.Add(edge);
        }
        return edges;
    }

    private List<GameObject> getEdgeByVertex(int idVertexFirst, int idVertexLast) {
        List<GameObject> edges = new List<GameObject>();
        foreach (GameObject edge in A)
        {
            ObjectEdge oEdge = edge.GetComponent<ObjectEdge>();
            if (oEdge.nbVoieFT > 0 && oEdge.nbVoieTF == 0) { 
                if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexFirst && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexLast)
                    edges.Add(edge);
            } else if (oEdge.nbVoieFT == 0 && oEdge.nbVoieTF > 0)
            {
                if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexLast && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexFirst)
                    edges.Add(edge);
            } else if (oEdge.nbVoieFT > 0 && oEdge.nbVoieTF > 0)
            {
                if ((edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexLast && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexFirst) ||
                    (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexFirst && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexLast))
                    edges.Add(edge);
            }
        }
        return edges;
    }


    private InfoVertex getVertex(int idVertex)
    {
        foreach (InfoVertex s in S)
        {
            if (s.idVertex == idVertex)
                return s;
        }
        return null;
    }
}


