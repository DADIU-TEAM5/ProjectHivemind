﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class PrimarySceneLoader : ScriptableObject
{
    public Dictionary<string, string> ScenePathDictionary;

    public void AddScene(string name, string path) {
        if (!ScenePathDictionary.ContainsKey(name)) {
            ScenePathDictionary.Add(name, path);
        }
    }

    public void LoadScene(string name) {
        if (ScenePathDictionary.ContainsKey(name)) {

            var scenePath = ScenePathDictionary[name]; 

            SceneManager.LoadScene(scenePath);
        }

        Debug.LogError("Scene name was not in dictionary");
    }

    public void RedoDictionary() {
        ScenePathDictionary = new Dictionary<string, string>();

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