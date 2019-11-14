using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToReturn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.UnloadScene("ArenaGeneration");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Loading tap");
            OverallSceneWorker.LoadScene("ArenaGeneration");
           
        }
    }
}
