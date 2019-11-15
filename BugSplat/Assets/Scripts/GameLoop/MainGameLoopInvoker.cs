using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="GameLoops/MainHandler")]
public class MainGameLoopInvoker : SimpleGameLoopInvoker
{
    public override void LateUpdateGameLoop(float time)
    {
        for (var i = 0; i < _gameLoops.Count; i++) {
            if (_gameLoops[i].isActiveAndEnabled)
                _gameLoops[i].LoopLateUpdate(_deltaTime);
        }
    }

    public override void UpdateGameLoop(float time)
    {
        _deltaTime = time - _timeLastTrigger; 

        _timeLastTrigger = time;

        for (var i = 0; i < _gameLoops.Count; i++) {
            var loop = _gameLoops[i];
            if (loop.isActiveAndEnabled)
                _gameLoops[i].LoopUpdate(_deltaTime);
        }
    }
}
