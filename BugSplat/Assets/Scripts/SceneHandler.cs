using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[System.Serializable]
public class SceneHandler : MonoBehaviour
{
    [SerializeField]
    public StringList SceneListSO;

    [SerializeField]
    public int SelectedSceneIndex;

    [SerializeField]
    public string[] SceneList;

    [SerializeField]
    public string SelectedScene;

    [SerializeField]
    public string SelectedSceneGuid;

    private void OnEnable()
    {
        int _sceneCount = EditorBuildSettings.scenes.Length;

        SceneList = new string[_sceneCount];
    }


    public void ChangeScene(string sceneGuid)
    {
        if(sceneGuid != "")
        {
            string sceneName = "";

            string tempPath = AssetDatabase.GUIDToAssetPath(sceneGuid);

            int lastFolderIndex = tempPath.LastIndexOf('/');
            sceneName = tempPath.Remove(0, lastFolderIndex + 1).ToString();

            int fileEndingIndex = sceneName.LastIndexOf('.');
            sceneName = sceneName.Remove(fileEndingIndex, sceneName.Length - fileEndingIndex);

            
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(SelectedScene);
        }


    }
}
