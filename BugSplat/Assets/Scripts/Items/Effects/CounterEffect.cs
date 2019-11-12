﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Effects/Counter")]
public class CounterEffect : Effect
{
    public int TargetCount;
    public int CountTick; 

    public Effect OnTargetEffect;

    private int _counter; 

    public override void Init()
    {
        _counter = 0;
    }

    public override void Trigger(GameObject target = null)
    {
        _counter += CountTick;

        if (_counter >= TargetCount) {
            OnTargetEffect.Trigger(target);
            Init();
        }
    }
}