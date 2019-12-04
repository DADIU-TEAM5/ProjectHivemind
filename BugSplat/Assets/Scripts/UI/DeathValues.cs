using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathValues : MonoBehaviour
{
    public LevelOutcome LevelOutcome;
    public FloatVariable TimeUsed;
    public IntVariable Money;
    public IntVariable Enemy;
    public GameText Title;
    public Sprite DeathImg;
    public GameTextMeshProUpdater GameTextUp;

    void Awake()
    {
        LevelOutcome.SetImage(DeathImg);
        LevelOutcome.SetKilledEnemies(Enemy);
        LevelOutcome.SetMoneyEarned(Money);
        LevelOutcome.SetTimeUsed(TimeUsed);
        //LevelOutcome;
        GameTextUp.GameText = Title;
    }
}
