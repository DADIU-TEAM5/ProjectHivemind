using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

//[CustomEditor(typeof(SceneHandler))]
public class SceneHandlerEditor : Editor
{
    /*[SerializeField]
    private ObjectList _scene;
    
    [SerializeField]
    private Object _selectedScene;

    [SerializeField]
    private string[] _sceneList;*/

    int windowID = 1234;
    Rect windowRect;

    private void OnEnable()
    {
       /* _sceneList = new string[_scene.Value.Count];

        for (int i = 0; i < _scene.Value.Count; i++)
        {
            _sceneList[i] = _scene.Value[i].name;
        }*/
    }

    // OnInspector GUI
    public override void OnInspectorGUI() //2
    {
        //EditorGUILayout.Popup(0, _sceneList);

        GUILayout.Space(20f); //2
        GUILayout.Label("Custom Editor Elements", EditorStyles.boldLabel);
        //EditorGUILayout.


        EditorGUILayout.LabelField("Label");
        EditorGUILayout.ColorField(Color.cyan);
        EditorGUILayout.ToggleLeft("Toggle", false);
        GUILayout.Button("Button");
    }
}
