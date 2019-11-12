using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEvent : MonoBehaviour
{
    public GameEvent[] ChangeSceneEvents;
    private string currentSceneName;

    public void Awake()
    {
        currentSceneName = "";
        if (ChangeSceneEvents == null)
        {
            ChangeSceneEvents = new GameEvent[0];
        }
    }

    public void OnEnable()
    {
        currentSceneName = "";
        if (ChangeSceneEvents == null)
        {
            ChangeSceneEvents = new GameEvent[0];
        }

        RaiseEvent();
    }



    public void RaiseEvent()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("RaiseEvent() - SceneName = " + sceneName);

        //HardcodeLife
        if (sceneName.Equals("ArenaGeneration"))
        {
            ChangeSceneEvents[0].Raise();
            Debug.Log("RaiseEvent() - Arena Raised");
        }

        if (sceneName.Equals("Hub Scene"))
        {
            ChangeSceneEvents[1].Raise();
            Debug.Log("RaiseEvent() - Hub Raised");
        }
        if (sceneName.Equals("Shop Scene"))
        {
            ChangeSceneEvents[2].Raise();
            Debug.Log("RaiseEvent() - Shop Raised");

        }

    }
}
