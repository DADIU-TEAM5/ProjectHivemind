using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallSceneWorker : MonoBehaviour
{
    static GameObject singleton;

    public static void LoadScene(string sceneName)
    {
        //Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.UnloadScene(SceneManager.GetActiveScene());

        SceneManager.LoadScene(sceneName);


    }



}
