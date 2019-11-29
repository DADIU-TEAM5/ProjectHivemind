using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stun Enemy")]
public class StunEnemyEffect : Effect
{   
    public float Duration = 3;

    private EmptyMono CoroutineBoy;

    public override void Init()
    {
    }

    public override void DoEffect(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();
        if (enemy == null) return;

        enemy.Stun(Duration);
    }
}
