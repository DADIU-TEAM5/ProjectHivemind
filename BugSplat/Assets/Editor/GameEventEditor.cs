using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var gameEvent = (GameEvent) target;
        if (GUILayout.Button("Raise Event!")) {
            gameEvent.Raise(); 
        }
    }
}
