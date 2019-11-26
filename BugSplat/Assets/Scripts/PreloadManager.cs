using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
    public List<ScriptableObject> ScriptableObjects = new List<ScriptableObject>();


    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.transform.root.gameObject);
    }

    private void Start()
    {
        //SceneManager.LoadScene(1);
    }

}
