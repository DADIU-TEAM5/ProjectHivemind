using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToReturn : MonoBehaviour
{
    public BoolVariable PlayerControlOverrideSO;
    public IntVariable CurrentLevelSO;
    public StringVariable LastScene;


    public FloatVariable TotalTime;
    public IntVariable TotalEarned;
    public IntVariable TotalEnemiesKilled;

    public IntVariable LevelEarned;
    public IntVariable LevelEnemyKilled;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.UnloadScene("ArenaGeneration");
        TotalEarned.Value += LevelEarned.Value;
        TotalEnemiesKilled.Value += LevelEnemyKilled.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.touchCount > 0)
        {
            Debug.Log("Loading tap");
            PlayerControlOverrideSO.Value = false;
            CurrentLevelSO.InitialValue = 0;
            LastScene.Value = "";
            OverallSceneWorker.LoadScene("_PreloadScene");

            ClearAllResults();
        }
    }


    void ClearAllResults()
    {
        TotalTime.Value = 0;
        TotalEarned.Value = 0;
        TotalEnemiesKilled.Value = 0;
    }

    
}
