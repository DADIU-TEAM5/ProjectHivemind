using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class AssetBundleSceneLoader : MonoBehaviour
{
    public string AssetBundleURL;

    public string SceneName;

    public uint Version;

    public GameEvent LoadedScene;

    public PrimarySceneLoader PrimarySceneLoader;

    protected AssetBundle _loadedBundle;

    IEnumerator Start() {
        DontDestroyOnLoad(transform.root.gameObject);

        yield return LoadBundle();

        var sceneNames = _loadedBundle.GetAllScenePaths();


        PrimarySceneLoader.AddScene(SceneName, sceneNames[0]);

        LoadedScene?.Raise(gameObject);
    }

    public IEnumerator LoadBundle() {
        var path = Path.Combine(Application.persistentDataPath, SceneName + ".unity3d");
        if (File.Exists(path)) {
            Debug.Log("Loading from file");
            var bundle = AssetBundle.LoadFromFileAsync(path);
            yield return bundle;

            _loadedBundle = bundle.assetBundle;
        } else {
            var request = UnityWebRequest.Get(AssetBundleURL);
            var handler = request.downloadHandler;

            yield return request.SendWebRequest();
            
            File.WriteAllBytes(path, handler.data);

            LoadBundle();
        }
   }

    public void UnloadBundle() {
        _loadedBundle.Unload(true);
    }

    public void LoadAllAssets() {
        var objects = _loadedBundle.LoadAllAssets();
    }

    public T GetAsset<T>(string assetName) where T : UnityEngine.Object {
        return _loadedBundle.LoadAsset<T>(assetName);
    }

    void OnDestroy() {
        _loadedBundle.Unload(true);
    }
}
