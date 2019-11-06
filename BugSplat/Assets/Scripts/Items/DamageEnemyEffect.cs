using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Damage Enemy")]
public class DamageEnemyEffect : Effect
{
    public float AmountOfDamage;

    public override void Trigger(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();

        enemy?.TakeDamage(AmountOfDamage);
    }
}
