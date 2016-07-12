using UnityEngine;
using System.Collections;

public static class TabFlux {
    public static ArrayList tabFluxEntrant = new ArrayList();
    public static ArrayList tabFluxSortant = new ArrayList();

    public static void putFlux(int idVertex, int idEdge, int count, bool fluxIn)
    {
        DataFlux f = new DataFlux(idVertex, idEdge, count);
        if (fluxIn)
            tabFluxEntrant.Add(f);
        else
            tabFluxSortant.Add(f);
    }

    public static void avgFlux()
    {
        int countEntrant = getNbFluxEntrant();
        int countSortant = getNbFluxSortant();
        int countTot = 0, countTmp = 0;
        foreach (DataFlux f in tabFluxEntrant) 
        {
            f.count = (int)((double)f.count / (double)countEntrant * Config.nbCar);
            if (countTmp == 0 || countTmp < f.count) { 
                countTmp = f.count;
            }
            countTot += f.count;
        }
        miseAEchelleEntrant(countTot, countTmp);

        countTmp = 0;
        countTot = 0;
        foreach (DataFlux f in tabFluxSortant)
        {
            f.count = (int)((double)f.count / (double)countSortant * Config.nbCar);
            if (countTmp == 0 || countTmp < f.count)
            {
                countTmp = f.count;
            }
            countTot += f.count;
        }
        miseAEchelleSortant(countTot, countTmp);
    }

    public static int getIdFluxSortant(DataFlux fluxBegin) {
        int count = 0;
        int r = (int)Random.Range(1f, Config.nbCar);
        foreach(DataFlux f in tabFluxSortant)
        {
            count += f.count;
            if (r < count && f.count != 0 && f.idEdge != fluxBegin.idEdge)
            {
           //     f.count -= 1;
                return f.idVertex;
            } 
        }

        return -1;
    }

    private static void miseAEchelleEntrant(int countTot ,int countTmp)
    {
        if (countTot < Config.nbCar)
        {
            foreach (DataFlux f in tabFluxEntrant)
            {
                if (f.count == countTmp)
                {
                    f.count += Config.nbCar - countTot;
                    break;
                }
            }
        }
        else if (countTot > Config.nbCar)
        {
            foreach (DataFlux f in tabFluxEntrant)
            {
                if (f.count == countTmp)
                {
                    f.count += Config.nbCar - countTot;
                    break;
                }
            }
        }
    }

    private static void miseAEchelleSortant(int countTot, int countTmp)
    {
        if (countTot < Config.nbCar)
        {
            foreach (DataFlux f in tabFluxSortant)
            {
                if (f.count == countTmp)
                {
                    f.count += Config.nbCar - countTot;
                    break;
                }
            }
        }
        else if (countTot > Config.nbCar)
        {
            foreach (DataFlux f in tabFluxSortant)
            {
                if (f.count == countTmp)
                {
                    f.count += Config.nbCar - countTot;
                    break;
                }
            }
        }
    }

    private static int getNbFluxEntrant()
    {
        int count = 0;
        foreach(DataFlux f in tabFluxEntrant)
        {
            count += f.count;
        }
        return count;
    }

    private static int getNbFluxSortant()
    {
        int count = 0;
        foreach (DataFlux f in tabFluxSortant)
        {
            count += f.count;
        }
        return count;
    }



}

public class DataFlux {
    public int idVertex;
    public int idEdge;
    public int count;

    public DataFlux(int idVertex, int idEdge, int count)
    {
        this.idVertex = idVertex;
        this.idEdge = idEdge;
        this.count = count;
    }
}
