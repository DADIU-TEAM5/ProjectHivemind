using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager Instance;

    public string AssetBundleURL;

    public uint Version;

    public GameEvent BundledLoaded;

    protected AssetBundle _loadedBundle;


    virtual protected IEnumerator Start() {
        if (Instance == null) {
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

            yield return request.SendWebRequest();
            
            File.WriteAllBytes(path, handler.data);

            yield return LoadBundle();
       }
   }

    public void UnloadBundle() {
        _loadedBundle.Unload(true);
    }

    public void LoadAllAssets() {
        var objects = _loadedBundle.LoadAllAssets();

        BundledLoaded?.Raise(gameObject);
    }

    public T GetAsset<T>(string assetName) where T : UnityEngine.Object {
        return _loadedBundle.LoadAsset<T>(assetName);
    }

    void OnDestroy() {
        _loadedBundle.Unload(true);
    }
}