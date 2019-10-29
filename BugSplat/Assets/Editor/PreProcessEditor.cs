using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PreProcess))]
public class PreProcessEditor : Editor
{
    private PreProcess _preProcess;

    void OnEnable()
    {
        _preProcess = (PreProcess)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Get Trajectory"))
        {
            _preProcess.PreProcessTrajectory();
        }
    }
}
