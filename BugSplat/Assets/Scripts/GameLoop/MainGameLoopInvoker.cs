﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="GameLoops/MainHandler")]
public class MainGameLoopInvoker : SimpleGameLoopInvoker
{
    public override void LateUpdateGameLoop(float time)
    {
        for (var i = 0; i < _gameLoops.Count; i++) {
            _gameLoops[i].LoopLateUpdate(_deltaTime);
        }
    }

    public override void UpdateGameLoop(float time)
    {
        _deltaTime = time - _timeLastTrigger; 

        _timeLastTrigger = time;

        for (var i = 0; i < _gameLoops.Count; i++) {
            _gameLoops[i].LoopUpdate(_deltaTime);
        }
    }
}
