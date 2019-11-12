using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickBasedEffect : Effect
{
    public Effect TickEffect;

    public float SecondsPerTick; 

    public override void Init()
    {
    }

    public override void Trigger(GameObject target = null)
    {
       var enemy = target.GetComponent<Enemy>();
       enemy?.StartCoroutine(Tick(target));
    }

    private IEnumerator Tick(GameObject target) {
        while (true) {
            if (!(target?.activeInHierarchy ?? false)) break;

            TickEffect.Trigger(target);
            yield return new WaitForSeconds(SecondsPerTick);
        }
    }
}
