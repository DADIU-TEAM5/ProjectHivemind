using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleGameLoopInvoker : GameLoopInvoker
{
    protected readonly List<GameLoop> _gameLoops = new List<GameLoop>();

    public override void RegisterGameLoop(GameLoop gameLoop)
    {
        if (!_gameLoops.Contains(gameLoop)) _gameLoops.Add(gameLoop);
    }

    public override void UnregisterGameLoop(GameLoop gameLoop)
    {
        if (_gameLoops.Contains(gameLoop)) _gameLoops.Remove(gameLoop);
    }
}
