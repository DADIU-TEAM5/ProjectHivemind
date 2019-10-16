using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameLoopInvoker : ScriptableObject
{
    protected float _timeLastTrigger = 0f;
    protected float _deltaTime = 0f;


    public abstract bool UpdateGameLoop(float time);
    public abstract void LateUpdateGameLoop();

    public abstract void RegisterGameLoop(GameLoop gameLoop);

    public abstract void UnregisterGameLoop(GameLoop gameLoop);

    protected virtual void OnEnable() {
        Init();
    }

    protected virtual void Init() {
        _timeLastTrigger = 0f;
        _deltaTime = 0f;
    }
}