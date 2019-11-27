using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AssetBundleSceneLoader : MonoBehaviour
{
    public string AssetBundleURL;

    public string SceneName;

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
        var request = UnityWebRequestAssetBundle.GetAssetBundle(AssetBundleURL);
        yield return request.SendWebRequest();

        Debug.Log($"REQUEST CODE: {request.responseCode}");
        _loadedBundle = DownloadHandlerAssetBundle.GetContent(request);
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
