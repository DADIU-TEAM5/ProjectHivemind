using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedStatChange : Effect
{
    public StatChangeEffect StatChange;

    public float Timer;

    private StatChangeEffect InverseStatChange;

    public override void Init()
    {
        InverseStatChange = new StatChangeEffect();
        InverseStatChange.Stat = StatChange.Stat;
        InverseStatChange.Change = -InverseStatChange.Change;
    }

    public override void Trigger(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();
        enemy?.StartCoroutine(EffectCountdown());

        if (enemy == null) {
            var player = target?.GetComponent<PlayerMovement>();
            player?.StartCoroutine(EffectCountdown());
        }
    }

    private IEnumerator EffectCountdown() {
        StatChange.Trigger();

        yield return new WaitForSeconds(Timer);

        InverseStatChange.Trigger();
    }
}