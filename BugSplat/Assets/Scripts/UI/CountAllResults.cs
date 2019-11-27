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
    }

    public void SumAll()
    {
        TotalEarned.Value += LevelEarned.Value;
        TotalEnemiesKilled.Value += LevelEnemiesKilled.Value;
    }

}
