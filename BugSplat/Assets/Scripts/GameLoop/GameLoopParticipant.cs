using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopParticipant : GameLoop
{
    public GameLoopInvoker GameLoopHandler;

    public GameLoop[] GameLoops;

    private void OnEnable()
    {
        GameLoopHandler.RegisterGameLoop(this);
    }

    private void OnDisable()
    {
        GameLoopHandler.UnregisterGameLoop(this);
    }

    public override void LoopUpdate(float deltaTime)
    {
        for (var i = 0; i < GameLoops.Length; i++) {
            GameLoops[i].LoopUpdate(deltaTime);
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        for (var i = 0; i < GameLoops.Length; i++) {
            GameLoops[i].LoopLateUpdate(deltaTime);
        }
    }
}
