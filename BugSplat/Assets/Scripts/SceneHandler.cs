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

    [SerializeField]
    public StringVariable LastSceneSO;

    private void OnEnable()
    {
        int _sceneCount = SceneManager.sceneCount;

        SceneList = new string[_sceneCount];
    }


    public void ChangeScene(string sceneName)
    {
        LastSceneSO.Value = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitApplication() {
        Application.Quit();
    }
}
