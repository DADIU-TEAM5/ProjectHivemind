using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public static MapGenerator mapGen;

    public bool[] OpenEdges;

    public GameObject[] Walls;

    bool _allNeighBoursFound;

    public bool IsaccesibleFromMiddle;

    public Hexagon[] Neighbours;
    // Start is called before the first frame update
    private void OnEnable()
    {


        Neighbours = new Hexagon[6];
    }
    

    public void GetNeighbours()
    {
        if (!_allNeighBoursFound)
        {
            bool temp = true;
            for (int i = 0; i < Neighbours.Length; i++)
            {
                if (Neighbours[i] != null)
                {

                    //Neighbours[i].Neighbours[(i + 3) % 6] = this;

                    if (Neighbours[i].Neighbours[(i + 3 + 1) % 6] != null)
                    {

                        Neighbours[(i + 5) % 6] = Neighbours[i].Neighbours[(i + 3 + 1) % 6];
                    }
                    if (Neighbours[i].Neighbours[(i + 3 + 5) % 6] != null)
                    {
                        Neighbours[(i + 1) % 6] = Neighbours[i].Neighbours[(i + 3 + 5) % 6];
                    }
                }
                else
                {
                    temp = false;
                }
            }

            _allNeighBoursFound = temp;
        }
    }

    public void RotateTile( int times)
    {
        for (int i = 0; i < times; i++)
        {
            transform.Rotate(0, 60, 0);
            bool temp = OpenEdges[OpenEdges.Length - 1];
            GameObject tempObj = Walls[Walls.Length - 1];
            for (int j = 0; j < OpenEdges.Length; j++)
            {

                GameObject tempObj2 = Walls[j];
                bool temp2 = OpenEdges[j];

                Walls[j] = tempObj;
                OpenEdges[j] = temp;

                tempObj = tempObj2;
                temp = temp2;


            }

        }
        
    }

    public void LabelNeighbourAccesibility()
    {
        if (IsaccesibleFromMiddle)
        {
            for (int i = 0; i < Neighbours.Length; i++)
            {
                if (OpenEdges[i] && Neighbours[i] != null)
                {
                    Neighbours[i].IsaccesibleFromMiddle = (Neighbours[i].OpenEdges[(i + 3) % 6]);
                }
            }
        }
    }
    public void RotateToFitMiddleConnectedNeighbour()
    {
        if (HasAMiddleConnectedNeighbourWithAnOpenSideTowardsThisTile())
        {
            for (int i = 0; i < Neighbours.Length; i++)
            {
                if(Neighbours[i] != null && Neighbours[i].IsaccesibleFromMiddle && Neighbours[i].OpenEdges[(i + 3) % 6])
                {
                    while (!IsaccesibleFromMiddle)
                    {

                        RotateTile(1);
                        Neighbours[i].LabelNeighbourAccesibility();
                        
                    }
                    return;
                }
            }

        }
        else
        {
            print("no possible solutions");
        }

    }
    public bool HasAMiddleConnectedNeighbourWithAnOpenSideTowardsThisTile()
    {
        bool returnBol = false;
        for (int i = 0; i < Neighbours.Length; i++)
        {
            if (Neighbours[i] != null && Neighbours[i].IsaccesibleFromMiddle && Neighbours[i].OpenEdges[(i + 3) % 6]) {

                returnBol = true;
            }
            
        }
        return returnBol;
    }


    public void OpenEdge(int edge)
    {
        Walls[edge].SetActive(false);
        OpenEdges[edge] = true;

        
    }

    public void OpenAndRotateNeighbour()
    {
        for (int i = 0; i < Neighbours.Length; i++)
        {
            if (Neighbours[i] != null && Neighbours[i].IsaccesibleFromMiddle)
            {
                //print("opening edge " + (i + 3) % 6+ " of "+ Neighbours[i].name+ " neigbour "+i );
                Neighbours[i].OpenEdge((i + 3) % 6);
                RotateToFitMiddleConnectedNeighbour();
                return;
            }
                 
        }
        print("no availble neighbours to open");
    }

   
}
