using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public string SelectedScene;

    [HideInInspector]
    public string[] SceneList;

    public void ChangeScene(string sceneIndex)
    {
        if(sceneIndex != "")
        {
            SceneManager.LoadScene(SceneList[int.Parse(sceneIndex)]);
        }
        else
        {
            SceneManager.LoadScene(SelectedScene);
        }


    }
}
