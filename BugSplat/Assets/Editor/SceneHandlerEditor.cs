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
    private List<SceneAsset> _sceneAssets = new List<SceneAsset>();

    private string[] _sceneList;

    private SceneHandler SceneHandlerVar;

    private bool _loaded = false;

    [Tooltip("Select scene to switch to.")]
    [SerializeField]
    private int _selectedSceneIndex;

    private string _selectedScene;


    private void OnEnable()
    {
        _sceneList = new string[EditorBuildSettings.scenes.Length];

        SceneHandlerVar = (SceneHandler)target;

        SceneHandlerVar.SceneList = new string[EditorBuildSettings.scenes.Length];

        LoadScenes();
    }

    // OnInspector GUI
    public override void OnInspectorGUI()
    {

        GUILayout.Space(20f);

        _selectedSceneIndex = EditorGUILayout.Popup("Select scene:", _selectedSceneIndex, _sceneList, EditorStyles.popup);

        _selectedScene = _sceneList[_selectedSceneIndex];

        SceneHandlerVar.SelectedScene = _selectedScene;

        GUILayout.Space(10f);

        ShowScenes();

        GUILayout.Space(10f);


        /* STUFF IF I NEED TO MAKE OTHER FIELDS!
        
        //GUILayout.Label("Custom Editor Elements", EditorStyles.boldLabel);

        EditorGUILayout.LabelField(_selectedSceneIndex.ToString());

        EditorGUILayout.LabelField(_selectedScene);


        EditorGUILayout.ColorField(Color.cyan);
        EditorGUILayout.ToggleLeft("Toggle", false);
        GUILayout.Button("Button");
        */
    }


    void ShowScenes()
    {
        GUILayout.Label("Else use index of scene in event listener:", EditorStyles.boldLabel);

        for (int i = 0; i < _sceneList.Length-1; i++)
        {
            EditorGUILayout.LabelField(i + ": " + _sceneList[i]);
        }
    }


    void LoadScenes()
    {
        int count = 0;

        for (int i = 0; i < _sceneList.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                EditorBuildSettingsScene tempPath = EditorBuildSettings.scenes[i];
                int lastFolderIndex = tempPath.path.LastIndexOf('/');
                _sceneList[count] = tempPath.path.Remove(0, lastFolderIndex + 1).ToString();

                int fileEndingIndex = _sceneList[count].LastIndexOf('.');
                _sceneList[count] = _sceneList[count].Remove(fileEndingIndex, _sceneList[count].Length - fileEndingIndex);

                SceneHandlerVar.SceneList[count] = _sceneList[count];

                count++;
            }
        }
    }
}
