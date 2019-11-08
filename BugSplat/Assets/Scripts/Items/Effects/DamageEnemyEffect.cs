using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Damage Enemy")]
public class DamageEnemyEffect : Effect
{
    public float AmountOfDamage;

    public override void Init()
    {
    }

    public override void Trigger(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();

        if (enemy == null) Debug.Log("Hold up what??");

        enemy?.TakeDamage(AmountOfDamage);
    }
}
