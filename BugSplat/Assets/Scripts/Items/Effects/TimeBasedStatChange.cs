﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Timebased StatChange")]
public class TimeBasedStatChange : Effect
{
    public StatChangeEffect StatChange;

    public float Timer;

    private StatChangeEffect InverseStatChange;

    private EmptyMono CoroutineBoy;

    public override void Init()
    {
        InverseStatChange = (StatChangeEffect) CreateInstance(typeof(StatChangeEffect));
        InverseStatChange.Stat = StatChange.Stat;
        InverseStatChange.Change = -StatChange.Change;

        CoroutineBoy = MakeCoroutineObject();
        CoroutineBoy.gameObject.name = "TimeBasedStatChange_CoroutineBoy";
    }

    public override void DoEffect(GameObject target = null)
    {
        CoroutineBoy.StartCoroutine(EffectCountdown());
    }

    private IEnumerator EffectCountdown() {
        StatChange.Trigger();

        yield return new WaitForSeconds(Timer);

        InverseStatChange.Trigger();
    }
}