using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Hexagon))]
public class HexagonEditor : Editor
{
    Hexagon hex;

    private void OnEnable()
    {
        hex =(Hexagon) target;
    }

    public override void OnInspectorGUI()
    {

        if( GUILayout.Button("Creat Tiles"))
        {
            Hexagon.mapGen.GenrateAroundTile(hex.gameObject);
        }

        if (GUILayout.Button("Rotate Tile"))
        {
            hex.RotateTile(1);
        }

        if (GUILayout.Button("Rotate To Fit"))
        {
            hex.RotateToFitMiddleConnectedNeighbour();
        }
        if (GUILayout.Button("Open Neighbour"))
        {
            hex.OpenAndRotateNeighbour();
        }
        if (GUILayout.Button("Create Connectors"))
        {
            hex.CreateConnectors();
        }
        if (GUILayout.Button("Remove OuterWalls"))
        {
            hex.RemoveOuterWalls();
        }

        DrawDefaultInspector();

        
    }



}
