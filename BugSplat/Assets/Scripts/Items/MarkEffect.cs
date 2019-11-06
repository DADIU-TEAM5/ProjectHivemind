using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Mark Enemy")]
public class MarkEffect : Effect
{
    public Effect MarkPopEffect;

    private List<Enemy> MarkedEnemies = new List<Enemy>();

    public override void Trigger(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();

        if (enemy == null) {
            // Pop the mark
            foreach (var markedEnemy in MarkedEnemies) {
                if (markedEnemy.isActiveAndEnabled) {
                    MarkPopEffect.Trigger(markedEnemy.gameObject);
                }
            }

            MarkedEnemies.Clear();
        } else {
            MarkedEnemies.Add(enemy);
        }
    }

}
