using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floyd {

    private ObjectVertex objectVertex;
    private ArrayList S;
    private GameObject[] A;
    private int[,] p;
    private float[,] d;
    private List<GameObject> lEdge ;

    public Floyd()
    {
        objectVertex = GameObject.FindWithTag("TabVertex").GetComponent<ObjectVertex>();
        S = objectVertex.tabVertex;
        p = new int[S.Count, S.Count];
        d = new float[S.Count, S.Count];
        A = GameObject.FindGameObjectsWithTag("Edge");

        initTabD();
        initTabPrec();
        floydWharshall();
    }

    public void floyd(int idVertexBegin, int idVertexEnd)
    {
        lEdge = new List<GameObject>();
        int i = getPosTab(idVertexBegin);
        int j = getPosTab(idVertexEnd);
        path(i, j);
        TabParcours.putParcours(idVertexBegin, idVertexEnd, lEdge);
    }

    private void path(int i, int j)
    {
        if (p[i,j] == -1)
        {
            InfoVertex sFirst = (InfoVertex)S[i];
            InfoVertex sLast = (InfoVertex)S[j];
            GameObject edge = getEdgeByVertex(sFirst.idVertex, sLast.idVertex);
            lEdge.Add(edge);
        } else
        {
            path(p[i, j], j);
            path(i, p[i, j]);
        }
    }

    private int getPosTab(int id)
    {
        int i = 0;
        foreach(InfoVertex s in S)
        {
            if (s.idVertex == id)
                return i;
            i++;
        }
        return i;
    }

    private void floydWharshall() {
        for (int k = 0; k < S.Count; k++) {
            for (int i = 0; i < S.Count; i++) {
                for (int j = 0; j < S.Count; j++) {
                    if (d[i, k] != int.MaxValue || d[k, j] != int.MaxValue) {
                        if (d[i,j] > (d[i,k] + d[k,j])) {
                            d[i, j] = d[i, k] + d[k, j];
                            p[i, j] = k;
                        }
                    }
                }
            }
        }
    }

    private void initTabD()
    {
        int i = 0, j = 0;
        foreach (InfoVertex s in S) {
            j = 0;
            foreach (InfoVertex s2 in S) {
                if (s.idVertex == s2.idVertex)
                    d[i, j] = 0;
                else {
                    GameObject connexion = getEdgeByVertex(s.idVertex, s2.idVertex);
                    d[i, j] = int.MaxValue;
                    if (connexion != null)
                    {
                        float poid = connexion.GetComponent<ObjectEdge>().longeur;
                        if (d[i,j] == int.MaxValue || d[i,j] < poid)
                            d[i,j] = poid;
                    }
                }
                j++;
            }
            i++;
        }
    }

    private void initTabPrec()
    {

        for (int i = 0; i < S.Count; i++) { 
            for (int j = 0; j < S.Count; j++) {
                if (d[i, j] != 0 && d[i, j] != int.MaxValue) { 
                    p[i, j] = -1;
                }
                else
                    p[i, j] = -1;
            }
        }
    }

    private GameObject getEdgeByVertex(int idVertexFirst, int idVertexLast)
    {
        //List<GameObject> edges = new List<GameObject>();
        foreach (GameObject edge in A)
        {
            ObjectEdge oEdge = edge.GetComponent<ObjectEdge>();
            if (oEdge.nbVoieFT > 0 && oEdge.nbVoieTF == 0)
            {
                if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexFirst && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexLast)
                    return edge;
            }
            else if (oEdge.nbVoieFT == 0 && oEdge.nbVoieTF > 0)
            {
                if (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexLast && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexFirst)
                    return edge;
            }
            else if (oEdge.nbVoieFT > 0 && oEdge.nbVoieTF > 0)
            {
                if ((edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexLast && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexFirst) ||
                    (edge.GetComponent<ObjectEdge>().idVertexFirst == idVertexFirst && edge.GetComponent<ObjectEdge>().idVertexLast == idVertexLast))
                    return edge;
            }
        }
        return null;
    }
}
