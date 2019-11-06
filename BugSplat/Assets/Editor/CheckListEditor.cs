using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(EnemyCheckOff))]
public class CheckListEditor : Editor
{
    EnemyCheckOff checkOff;



    private void OnEnable()
    {
        checkOff = (EnemyCheckOff)target;
    }


    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if(checkOff.Checks.Length != checkOff.Enemies.SpawnableEnemies.Length)
        {
            checkOff.Checks = new bool[checkOff.Enemies.SpawnableEnemies.Length];
        }

        for (int i = 0; i < checkOff.Checks.Length; i++)
        {
            GUILayout.Toggle(checkOff.Checks[i], checkOff.Enemies.SpawnableEnemies[i].name);
        }

        






    }


}
