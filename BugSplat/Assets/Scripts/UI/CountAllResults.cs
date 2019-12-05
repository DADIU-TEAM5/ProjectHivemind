using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountAllResults : MonoBehaviour
{

    public IntVariable LevelEnemyKilled;
    public IntVariable TotalEarned;
    public IntVariable LevelEarned;

    public IntVariable TotalEnemiesKilled;
    public IntVariable LevelEnemiesKilled;


    public void CountEnemiesKilled()
    {
		LevelEnemiesKilled.Value = LevelEnemyKilled.Value;

	}

    public void SumAll()
    {
		//LevelEnemiesKilled.Value = LevelEnemyKilled.Value;
		TotalEarned.Value += LevelEarned.Value;
        TotalEnemiesKilled.Value += LevelEnemiesKilled.Value;
    }

}
