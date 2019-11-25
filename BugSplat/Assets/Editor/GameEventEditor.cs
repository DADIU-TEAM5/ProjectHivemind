using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public GameObject Target;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var gameEvent = (GameEvent) target;

        Target = (GameObject) EditorGUILayout.ObjectField(Target, typeof(GameObject), allowSceneObjects: true);
        if (GUILayout.Button("Raise Event!")) {
            gameEvent.Raise(Target); 
        }
    }
}
