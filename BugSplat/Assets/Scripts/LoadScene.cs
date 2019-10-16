using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string SelectedSceneName;

    public bool Additive;
    
    public void SceneLoad()
    {
        SceneManager.LoadScene(SelectedSceneName);
    }

}
