using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTotalTime : GameLoop
{
    public FloatVariable LevelTimeConsume;
    public FloatVariable TotalTimeConsume;


    public override void LoopUpdate(float deltaTime)
    {
        if (deltaTime > 0)
        {
            TotalTimeConsume.Value += deltaTime;
            LevelTimeConsume.Value += deltaTime;
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }

    
}
