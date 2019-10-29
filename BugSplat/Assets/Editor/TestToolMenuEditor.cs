using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TestToolMenu))]
[CanEditMultipleObjects]
public class TestToolMenuEditor : Editor
{
    public TestToolMenu TTM;
    bool playerStatsIsOpen;

    private void OnEnable()
    {
        TTM = (TestToolMenu)target;
    }

    public override void OnInspectorGUI()
    {

        playerStatsIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(playerStatsIsOpen, "Player Stats");

        if (playerStatsIsOpen)
        {
            for (int i = 0; i < TTM.Floatvariables.Count; i++)
            {
                TTM.Floatvariables[i].Value = EditorGUILayout.FloatField(TTM.Floatvariables[i].name, TTM.Floatvariables[i].Value);
            }
            //player.movespeed = EditorGUILayout.Slider("Move Speed", player.movespeed, 1f, 50f);
            //EditorGUILayout.Space();
            //player.rotationspeed = EditorGUILayout.Slider("Rotation Speed", player.rotationspeed, 1f, 50f);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        base.OnInspectorGUI();
        //TTM.UpdateFloats();


    }

}
