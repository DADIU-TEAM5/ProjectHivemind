using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(EnemyCheckOff))]
public class CheckListEditor : Editor
{
    EnemyCheckOff checkOff;
    bool[] oldChecks;

    

    private void OnEnable()
    {
        checkOff = (EnemyCheckOff)target;

            

            oldChecks = checkOff.Checks;



            checkOff.Checks = new bool[checkOff.Enemies.SpawnableEnemies.Length];


            for (int i = 0; i < oldChecks.Length; i++)
            {
                if (i < checkOff.Checks.Length)
                {
                    checkOff.Checks[i] = oldChecks[i];
                }
            }

        
    }


    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        EditorUtility.SetDirty(target);






        for (int i = 0; i < checkOff.Checks.Length; i++)
        {
            checkOff.Checks[i] = GUILayout.Toggle(checkOff.Checks[i], checkOff.Enemies.SpawnableEnemies[i].name);
        }







    }


}
