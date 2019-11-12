using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEvent : MonoBehaviour
{
    public GameEvent[] ChangeSceneEvents;


    public void Start()
    {
        if (ChangeSceneEvents == null)
        {
            ChangeSceneEvents = new GameEvent[0];
        }
    }

    public void RaiseEvent()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        Debug.Log("RaiseEvent() - SceneName = " + sceneName);
            
        //HardcodeLife
        if (sceneName == "AreneGeneration")
        {
            ChangeSceneEvents[0].Raise();
            Debug.Log("RaiseEvent() - Arena Raised");
        }
            
        if (sceneName == "Hub Scene")
        {
            ChangeSceneEvents[1].Raise();
            Debug.Log("RaiseEvent() - Hub Raised");
        }
        if (sceneName == "Shop Scene")
        {
            ChangeSceneEvents[2].Raise();
            Debug.Log("RaiseEvent() - Shop Raised");

        }

    }
}
