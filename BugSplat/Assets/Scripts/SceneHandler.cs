using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField]
    private ObjectList _scene;

    [SerializeField]
    private int _selectedSceneIndex;

    [SerializeField]
    private string[] _sceneList;

    private void OnEnable()
    {
        _sceneList = new string[_scene.Value.Count];

        for (int i = 0; i < _scene.Value.Count; i++)
        {
            _sceneList[i] = _scene.Value[i].name;
        }
    }


    public void ChangeScene()
    {
        string sceneName = _scene.Value[_selectedSceneIndex].name;

        SceneManager.LoadScene(sceneName);
    }
}
