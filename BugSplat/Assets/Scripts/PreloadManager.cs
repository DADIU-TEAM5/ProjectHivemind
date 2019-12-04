using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
    public static PreloadManager Instance;

    public List<ScriptableObject> ScriptableObjects = new List<ScriptableObject>();


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) {
            DontDestroyOnLoad(this.transform.root.gameObject);
            Instance = this;
        } else {
            DestroyImmediate(this.gameObject);
        }
    }

    private void Start()
    {
        //SceneManager.LoadScene(1);
    }

}
