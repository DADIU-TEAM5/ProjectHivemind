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

    public RectTransform ProgressBar;

    private float _barWidth;

    private float progress;

    

    IEnumerator Start() {

        if (ProgressBar != null)
        {
            _barWidth = ProgressBar.rect.width;
            ProgressBar.sizeDelta = new Vector2(0, ProgressBar.rect.height);
        }
           
        DontDestroyOnLoad(transform.root.gameObject);
        
        yield return LoadBundle();

       
        var sceneNames = _loadedBundle.GetAllScenePaths();


        PrimarySceneLoader.AddScene(SceneName, sceneNames[0]);

        LoadedScene?.Raise(gameObject);
    }

    public IEnumerator LoadBundle() {
        
        var request = UnityWebRequestAssetBundle.GetAssetBundle(AssetBundleURL);
        request.SendWebRequest();

        while (!request.isDone)
        {

            progress = request.downloadProgress;
            UpdateLoadProgress();
            yield return null;

        }

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

    private void UpdateLoadProgress()
    {
        Debug.Log("Asset bundle progress: " + progress);
        if (ProgressBar != null)
            ProgressBar.sizeDelta = new Vector2(progress * _barWidth, ProgressBar.rect.height);
    }
}
