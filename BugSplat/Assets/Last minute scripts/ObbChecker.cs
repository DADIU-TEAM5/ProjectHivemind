
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ObbChecker : MonoBehaviour
{
    //Only use in Preloader Scene for android split APK
    private string nextScene = "Hub Scene";
    private bool obbisok = false;
    private bool loading = false;
    private bool replacefiles = false;
    //true if you wish to over copy each time

    private string[] paths = {
        "Vuforia/KVK.dat",
        "Vuforia/KVK.xml"
    };

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Application.dataPath.Contains(".obb") && !obbisok)
            {
                StartCoroutine(CheckSetUp());
                obbisok = true;
            }
        }
        else
        {
            if (!loading)
            {
                StartApp();
            }
        }
    }

    public void StartApp()
    {
        loading = true;
        SceneManager.LoadSceneAsync(nextScene);
    }

    public IEnumerator CheckSetUp()
    {
        yield return StartCoroutine(PullStreamingAssetFromObb("https://lukasdamgaard.dk/Assetbundles/", "obbextra.obb"));

        yield return new WaitForSeconds(1f);
        StartApp();
    }

    //Alternatively with movie files these could be extracted on demand and destroyed or written over
    //saving device storage space, but creating a small wait time.
    public IEnumerator PullStreamingAssetFromObb(string path, string name)
    {
        if (!File.Exists(Application.persistentDataPath + name) || replacefiles)
        {
            var request = UnityWebRequest.Get(path);
            var handler = request.downloadHandler;
            

            yield return request.SendWebRequest(); 
            
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log("Error unpacking:" + request.error + " path: " + request.url);

                yield break; //skip it
            }
            else
            {
                Debug.Log("Extracting " + name + " to Persistant Data");


                File.WriteAllBytes(Application.persistentDataPath + "/" + name, handler.data);
            }
        }
        yield return 0;
    }
}