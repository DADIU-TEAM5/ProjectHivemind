using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager Instance;

    public string AssetBundleURL;

    public uint Version;

    public GameEvent BundledLoaded;

    public RectTransform ProgressBar;
    private float _barWidth = 890;

    protected AssetBundle _loadedBundle;



    private float progress = 0;

    virtual protected IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;


            DontDestroyOnLoad(transform.root.gameObject);

            yield return LoadBundle();

            LoadAllAssets();
        }
    }

    public IEnumerator LoadBundle() {
        var path = Path.Combine(Application.persistentDataPath, "base.unity3d");
        if (File.Exists(path)) {
            var bundle = AssetBundle.LoadFromFileAsync(path);
            yield return bundle;

            _loadedBundle = bundle.assetBundle;
        } else {
            var request = UnityWebRequest.Get(AssetBundleURL);
            var handler = request.downloadHandler;
            request.SendWebRequest();

            while (!request.isDone)
            {

                progress = request.downloadProgress;

                UpdateLoadProgress();
                yield return null;

            }
            
            File.WriteAllBytes(path, handler.data);

            yield return LoadBundle();
       }
   }

    public void UnloadBundle()
    {
        _loadedBundle.Unload(true);
    }

    public void LoadAllAssets()
    {
        //bundleSize = _loadedBundle.LoadAllAssets().Length;
        var objects = _loadedBundle.LoadAllAssets();

        BundledLoaded?.Raise(gameObject);
    }

    public T GetAsset<T>(string assetName) where T : UnityEngine.Object
    {
        return _loadedBundle.LoadAsset<T>(assetName);
    }

    void OnDestroy()
    {
        _loadedBundle.Unload(true);
    }

    private void UpdateLoadProgress()
    {
        Debug.Log("Asset bundle progress: " + progress);
        if (ProgressBar != null)
            ProgressBar.sizeDelta = new Vector2(progress * _barWidth, ProgressBar.rect.height);
    }
}