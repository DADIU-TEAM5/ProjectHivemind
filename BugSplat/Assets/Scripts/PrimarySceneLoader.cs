using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class PrimarySceneLoader : ScriptableObject
{
    public Dictionary<string, string> ScenePathDictionary;

    public StringVariable LastScene;

    public string MainMenuScene;

    public void AddScene(string name, string path) {
        if (!ScenePathDictionary.ContainsKey(name)) {
            ScenePathDictionary.Add(name, path);
            Debug.Log("Scene added: " + name);
        }
    }

    public void LoadScene(string name) {
        LastScene.Value = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(name);
    }

    public void LoadMainMenu() {
        LastScene.Value = "_PreloadScene";

        SceneManager.LoadScene(MainMenuScene);
    }

    public void RedoDictionary() {
        ScenePathDictionary = new Dictionary<string, string>();
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        for (var i = 0; i < SceneManager.sceneCount; i++) {
            var scene = SceneManager.GetSceneAt(i);

            ScenePathDictionary.Add(scene.name, scene.path);
        }
    }

    void OnEnable() {
        if (ScenePathDictionary == null || ScenePathDictionary.Count == 0) {
            RedoDictionary();
        }
    }
}
