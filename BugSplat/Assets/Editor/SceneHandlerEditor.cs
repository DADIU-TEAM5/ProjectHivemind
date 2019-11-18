using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SceneHandler))]
public class SceneHandlerEditor : Editor
{
    [SerializeField]
    private static SceneHandler SceneHandlerVar;

    private int _sceneCount;


    private void OnEnable()
    {
        SceneHandlerVar = (SceneHandler)target;

        SceneHandlerVar.SelectedSceneIndex = 0;

        _sceneCount = EditorBuildSettings.scenes.Length;

        LoadScenes();

    }

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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LastSceneSO"), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("SelectedSceneIndex"), true);

        GUILayout.Space(20f);

        SceneHandlerVar.SelectedSceneIndex = EditorGUILayout.Popup("Select scene:", SceneHandlerVar.SelectedSceneIndex, SceneHandlerVar.SceneList, EditorStyles.popup);

        SceneHandlerVar.SelectedScene = SceneHandlerVar.SceneList[SceneHandlerVar.SelectedSceneIndex];

        SceneHandlerVar.SelectedSceneGuid = SceneHandlerVar.SceneListSO.List[SceneHandlerVar.SelectedSceneIndex];
 
        GUILayout.Space(10f);

        if (GUILayout.Button("Load Scenes"))
        {
            _sceneCount = EditorBuildSettings.scenes.Length;

            SceneHandlerVar.SceneList = new string[_sceneCount];

            LoadScenes();
        }

        //ShowScenes();

        GUILayout.Space(10f);

        serializedObject.ApplyModifiedProperties();
    }


    void LoadScenes()
    {

        SceneHandlerVar.SceneListSO.List.Clear();

        for (int i = 0; i < _sceneCount; i++)
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

        SceneHandlerVar.SceneList = new string[_sceneCount];

        for (int k = 0; k < SceneHandlerVar.SceneListSO.List.Count; k++)
        {
            string tempPath = AssetDatabase.GUIDToAssetPath(SceneHandlerVar.SceneListSO.List[k]);

            int lastFolderIndex = tempPath.LastIndexOf('/');
            SceneHandlerVar.SceneList[k] = tempPath.Remove(0, lastFolderIndex + 1).ToString();

            int fileEndingIndex = SceneHandlerVar.SceneList[k].LastIndexOf('.');
            SceneHandlerVar.SceneList[k] = SceneHandlerVar.SceneList[k].Remove(fileEndingIndex, SceneHandlerVar.SceneList[k].Length - fileEndingIndex);
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
