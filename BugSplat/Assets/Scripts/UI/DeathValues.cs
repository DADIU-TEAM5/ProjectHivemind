using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathValues : MonoBehaviour
{
    public LevelOutcome LevelOutcome;
    public FloatVariable TimeUsed;
    public IntVariable Money;
    public IntVariable Enemy;
    public string Title;
    public Sprite DeathImg;

    void Start()
    {
        LevelOutcome.SetImage(DeathImg);
        LevelOutcome.SetKilledEnemies(Enemy);
        LevelOutcome.SetMoneyEarned(Money);
        LevelOutcome.SetTimeUsed(TimeUsed);
        LevelOutcome.SetTitle(Title);
    }
}
