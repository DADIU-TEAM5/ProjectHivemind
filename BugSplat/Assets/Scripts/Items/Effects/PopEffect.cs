using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Pop mark")]
public class PopEffect : Effect
{
    public MarkEffect Mark;

    public Effect Pop;

    private bool _poppingOff = false;

    public override void Init()
    {
        _poppingOff = false;
        Pop.Init();
    }

    public override void DoEffect(GameObject target = null)
    {
        if (!_poppingOff) {
            var deadEnemy = target?.GetComponent<Enemy>();

            // Only pop the enemies, if the enemy that died was marked
            if (deadEnemy == null || !Mark.MarkedEnemies.Contains(deadEnemy)) return;

            _poppingOff = true;

            foreach (var enemy in Mark.MarkedEnemies) {
                if (enemy == null) continue;

                if (enemy.isActiveAndEnabled) {
                    Pop.Trigger(enemy.gameObject);
                }
            }

            // reset the mark
            Mark.Init();

            _poppingOff = false;
        }
    }

}