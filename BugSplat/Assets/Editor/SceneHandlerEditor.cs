using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// STILL NEEDS TO SERIALIZE THE CHOSEN VALUE FROM THE DROPDOWN LIST!!! - Look at catlikecoding tutorial
// ALSO, RIGHT NOW IF THE DESIGNERS CHOOSE AN INDEX FOR AN EVENT CALL, THIS COULD POTENTIALLY BE BROKEN, IF THE BUILD SETTINGS LIST IS SWITCHED AROUND.
// MOVE THE SCENE LIST INTO A SCRIPTABLE OBJECT, AND DERIVE THE LIST FROM THIS, AND CHECK IF NEW SCENES ARE ADDED OR SOME REMOVED. MAYBE USE GUID?


[CustomEditor(typeof(SceneHandler))]
public class SceneHandlerEditor : Editor
{

    [SerializeField]
    private string[] _sceneList;
    [SerializeField]
    private string[] _guid;
    [SerializeField]
    private string _selectedScene;
    [SerializeField]
    private string _selectedSceneGuid;

    [SerializeField]
    private static SceneHandler SceneHandlerVar;

    [SerializeField]
    private static int _selectedSceneIndex;


    private void OnEnable()
    {
        int sceneCount = EditorBuildSettings.scenes.Length;

        SceneHandlerVar = (SceneHandler)target;

        SceneHandlerVar.SceneList = new string[sceneCount];
        _sceneList = SceneHandlerVar.SceneList;

        SceneHandlerVar.Guid = new string[sceneCount];
        _guid = SceneHandlerVar.Guid;

        _selectedSceneGuid = SceneHandlerVar.SelectedSceneGuid;

        LoadScenes();

    }

    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneListSO"), true);
        //EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneList"), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Guid"), true);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedScene"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedSceneGuid"), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedSceneIndex"), true);

        GUILayout.Space(20f);

        SceneHandlerVar.SelectedSceneIndex = EditorGUILayout.Popup("Select scene:", SceneHandlerVar.SelectedSceneIndex, _sceneList, EditorStyles.popup);
        _selectedSceneIndex = SceneHandlerVar.SelectedSceneIndex;

        _selectedScene = _sceneList[_selectedSceneIndex];
        SceneHandlerVar.SelectedScene = _selectedScene;

        _selectedSceneGuid = _guid[_selectedSceneIndex];
        SceneHandlerVar.SelectedSceneGuid = _selectedSceneGuid;
 
        GUILayout.Space(10f);

        //ShowScenes();

        GUILayout.Space(10f);

        serializedObject.ApplyModifiedProperties();
    }


    void LoadScenes()
    {

        for (int i = 0; i < _sceneList.Length; i++)
        {
            EditorBuildSettingsScene tempPath = EditorBuildSettings.scenes[i];
            if (tempPath.enabled)
            {
                string tempGuid = AssetDatabase.AssetPathToGUID(tempPath.path);

                if (!SceneHandlerVar.SceneListSO.List.Contains(tempGuid))
                {
                    SceneHandlerVar.SceneListSO.List.Add(tempGuid);
                }
            }
        }
        
        for (int k = 0; k < SceneHandlerVar.SceneListSO.List.Count; k++)
        {
            _guid[k] = SceneHandlerVar.SceneListSO.List[k];

            string tempPath = AssetDatabase.GUIDToAssetPath(_guid[k]);

            int lastFolderIndex = tempPath.LastIndexOf('/');
            _sceneList[k] = tempPath.Remove(0, lastFolderIndex + 1).ToString();

            int fileEndingIndex = _sceneList[k].LastIndexOf('.');
            _sceneList[k] = _sceneList[k].Remove(fileEndingIndex, _sceneList[k].Length - fileEndingIndex);
        }
    }


    void Trash()
    {
        /*void ShowScenes()
        {
            GUILayout.Label("Else use index of scene in event listener:", EditorStyles.boldLabel);

            for (int i = 0; i < _sceneList.Length-1; i++)
            {
                EditorGUILayout.LabelField(i + ": " + _sceneList[i]);
            }
        }*/

        /* STUFF IF I NEED TO MAKE OTHER FIELDS!

        GUILayout.Label("Custom Editor Elements", EditorStyles.boldLabel);

        EditorGUILayout.LabelField(_selectedSceneIndex.ToString());

        EditorGUILayout.LabelField(_selectedScene);


        EditorGUILayout.ColorField(Color.cyan);
        EditorGUILayout.ToggleLeft("Toggle", false);
        GUILayout.Button("Button");
        */
    }
}
