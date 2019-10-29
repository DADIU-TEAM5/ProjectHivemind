using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[System.Serializable]
public class SceneHandler : MonoBehaviour
{
    public StringList SceneListSO;

    [SerializeField]
    public int SelectedSceneIndex;

    public string[] Guid;
    public string[] SceneList;

    public string SelectedScene;
    public string SelectedSceneGuid;

    public void ChangeScene(string sceneGuid)
    {
        if(sceneGuid != "")
        {
            string sceneName = "";

            for (int i = 0; i < Guid.Length; i++)
            {
                if (Guid[i] == sceneGuid)
                {
                    sceneName = SceneList[i];
                }
            }
            
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(SelectedScene);
        }


    }
}
