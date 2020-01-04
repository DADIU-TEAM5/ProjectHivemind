﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public static MapGenerator mapGen;

    [Required]
    public Tier Tier;
    public bool IsaccesibleFromMiddle;

    [TableMatrix]
    public bool[] OpenEdges = new bool[6];

    [ReadOnly]
    [HideInEditorMode]
    public Hexagon[] Neighbours;
    [ReadOnly]
    [HideInEditorMode]
    public GameObject[] Walls;
    [ReadOnly]
    [HideInEditorMode]
    public GameObject[] Corners;


    private bool _allNeighBoursFound;


    private Vector3[] _cornerPositions;

    public int callbackOrder => 2000;

    [Button("Rotate Tile", ButtonSizes.Large), GUIColor(0.5f, 1f, 0.5f)]
    public void RotateButton() {
        RotateTile();
    }

    private void OnEnable()
    {
        if(Neighbours.Length != 6)
            Neighbours = new Hexagon[6];

        Corners = new GameObject[6];
    }
    void InitilizeCornerPositions()
    {
        _cornerPositions = new Vector3[6];
        _cornerPositions[0] = mapGen.Vertices[3] * 35;
        _cornerPositions[1] = mapGen.Vertices[5] * 35;
        _cornerPositions[2] = mapGen.Vertices[1] * 35;
        _cornerPositions[3] = mapGen.Vertices[2] * 35;
        _cornerPositions[4] = mapGen.Vertices[0] * 35;
        _cornerPositions[5] = mapGen.Vertices[4] * 35;
    }

    public void GetNeighbours()
    {
      //  print("I was called i think");

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

    public void RotateTile(int times = 1)
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

                        RotateTile();
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



    public void RemoveOuterWalls()
    {
        //WallsRemoved = true;
        for (int i = 0; i < Neighbours.Length; i++)
        {
            if(Neighbours[i] == null)
            {
                if(Walls[i] != null)
                Walls[i].SetActive(false);
            }
            else
            {
                if (Neighbours[i].Walls[(i + 3) % 6] != null)
                {
                    if (Neighbours[i].Walls[(i + 3) % 6].activeSelf)
                    {
                        Walls[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void CreateConnectors()
    {
        InitilizeCornerPositions();

        GameObject connector;

        GameObject WallConnector = mapGen.WallConnector;
        GameObject OuterWallConnector = mapGen.SideWallConnector;

        for (int i = 0; i < 6; i++)
        {
            if (HasWallsOnEdges(i, (i+1)%6))
            {
                bool RotateAfter = false;

                if (CornerIsOuter(i, (i + 1) % 6))
                    connector = Instantiate(WallConnector, transform);
                else
                {
                    connector = Instantiate(OuterWallConnector, transform);
                    RotateAfter = true;
                }

                connector.transform.position = connector.transform.position + _cornerPositions[i];
                connector.name = ""+i;
                Corners[i] = connector;

                if (RotateAfter)
                {
                    int closedNeightbour = GetClosedEdge(i, (i + 1) % 6);
                    if(closedNeightbour != i)
                    {
                        connector.transform.LookAt(transform.position + _cornerPositions[(i + 1) % 6]);
                    }
                    else
                    {
                        connector.transform.LookAt(transform.position + _cornerPositions[(i + 5) % 6]);
                    }
                    
                }
            }
        }
        

    }

    int GetClosedEdge(int edge1, int edge2)
    {
        if (Neighbours[edge1] != null)
            return edge1;
        else
            return edge2;
    }

    bool CornerIsOuter(int edge1, int edge2)
    {
        bool bolean = true;

        if(Neighbours[edge1] == null|| Neighbours[edge2] == null)
        {
            bolean = false;
        }

        return bolean;
    }
    bool HasWallsOnEdges(int edge1, int edge2)
    {
        bool bolean = false;
        if(Walls[edge1] != null)
        {
            if (Walls[edge1].activeSelf)
            {
                bolean = true;
            }
        }

        if (Walls[edge2] != null)
        {
            if (Walls[edge2].activeSelf)
            {
                bolean = true;
            }
        }

        return bolean;
    }
}
