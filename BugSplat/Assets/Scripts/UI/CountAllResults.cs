using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountAllResults : MonoBehaviour
{
    public FloatVariable LevelTimeConsume;
    public FloatVariable TotalTimeConsume;
    public IntVariable TotalEarned;
    public IntVariable LevelEarned;

    public IntVariable TotalEnemiesKilled;
    public IntVariable LevelEnemiesKilled;


    public void CountEnemiesKilled()
    {
        LevelEnemiesKilled.Value += 1;
        TotalEnemiesKilled.Value += 1;
    }

    public void LevelClearLoadAfterShowing()
    {
        LevelEarned.Value = 0;
        TotalEnemiesKilled.Value += LevelEnemiesKilled.Value;
        LevelEnemiesKilled.Value = 0;
        LevelTimeConsume.Value = 0;
    }

    public void PlayerDiedLoadAfterShowing()
    {
        TotalEnemiesKilled.Value = 0;
        TotalTimeConsume.Value = 0;
        TotalEarned.Value = 0;
    }
}
