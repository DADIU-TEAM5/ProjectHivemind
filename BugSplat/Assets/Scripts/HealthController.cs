using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : GameLoop
{
    public IntVariable CurrentHealth;

    public GameEvent DeathEvent;

    public int MaxHealth;

    public override void LoopUpdate(float deltaTime)
    {
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }
}
