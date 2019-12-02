using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager Instance;

    public string AssetBundleURL;

    public GameEvent BundledLoaded;

    public RectTransform ProgressBar;
    private float _barWidth;

    protected AssetBundle _loadedBundle;

    

    private float progress = 0;

    virtual protected IEnumerator Start() {
        if (Instance == null) {
            Instance = this;
            _barWidth = ProgressBar.rect.width;
            DontDestroyOnLoad(transform.root.gameObject);

            yield return LoadBundle();

            LoadAllAssets();
        }
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
        _loadedBundle = DownloadHandlerAssetBundle.GetContent(request);
    }

    public void UnloadBundle() {
        _loadedBundle.Unload(true);
    }

    public void LoadAllAssets() {
        //bundleSize = _loadedBundle.LoadAllAssets().Length;
        var objects = _loadedBundle.LoadAllAssets();

        BundledLoaded?.Raise(gameObject);
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
        ProgressBar.sizeDelta = new Vector2(progress * _barWidth, ProgressBar.rect.height);
    }
}