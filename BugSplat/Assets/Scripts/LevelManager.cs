using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public IntVariable LevelKilled;
    public IntVariable TotalEnemyCount;
    public EnemyObjectList EnemiesList;
    public IntVariable EnemiesKilled;
    public IntVariable NumberOfWaves;

    public TextFeedback TextFeedback;

    [Header("Events")]
    public GameEvent HasWonEvent;
    public GameEvent LevelClearedEvent;
}