using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(cannonFodder))]
public class CannonfodderEditor : Editor
{
    cannonFodder _cannonFodder;

    void OnEnable()
    {
        Debug.Log("editor script enabled");
        _cannonFodder = (cannonFodder)target;
        
    }
    private void OnSceneGUI()
    {
        Handles.DrawWireArc(_cannonFodder.transform.position, Vector3.up, Vector3.forward, _cannonFodder.stats.AttackAngle, _cannonFodder.stats.AttackRange);
    }
}
