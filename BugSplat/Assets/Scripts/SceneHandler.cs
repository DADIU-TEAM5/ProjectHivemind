using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField]
    private ObjectList _scene;

    [SerializeField]
    private int _selectedScene;


    public void ChangeScene(Scene scene)
    {
        string sceneName = scene.name;

        SceneManager.LoadScene(sceneName);
    }
}
