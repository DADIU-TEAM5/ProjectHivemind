using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLevelResults : MonoBehaviour
{
    public FloatVariable LevelTimeConsume;
    public IntVariable LevelEarned;
    public IntVariable LevelEnemiesKilled;
    void Start()
    {
        LevelTimeConsume.Value = 0;
        LevelEarned.Value = 0;
        LevelEnemiesKilled.Value = 0;
    }
}
