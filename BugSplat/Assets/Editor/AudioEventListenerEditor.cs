using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(AudioEventListener))]
public class AudioEventListenerEditor : Editor
{
    private SerializedObject _listener;
    // Start is called before the first frame update
    void OnEnable()
    {
        _listener = serializedObject;
    }

    public override void OnInspectorGUI() {
        var eventListenerSP = serializedObject.FindProperty("EventListeners");
        var deletes = new List<int>();

        if (eventListenerSP == null) return;

        var arraySize = eventListenerSP.arraySize;
        if (GUILayout.Button("Add new")) {
            eventListenerSP.InsertArrayElementAtIndex(0);
        }

        for (int i = 0; i < arraySize; i++) {
            GUILayout.BeginHorizontal();

            var singlelistener = eventListenerSP.GetArrayElementAtIndex(i);

            EditorGUILayout.PropertyField(singlelistener.FindPropertyRelative("Response"));
            if (GUILayout.Button("Delete")) {
                deletes.Add(i); 
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(singlelistener.FindPropertyRelative("Event"));
            GUILayout.EndVertical();

        }

        foreach (var index in deletes) {
            eventListenerSP.DeleteArrayElementAtIndex(index);
        }

        

        _listener.ApplyModifiedProperties();
    }

}
