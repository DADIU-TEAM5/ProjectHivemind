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
    }

    public override void Trigger(GameObject target = null)
    {
        Debug.Log("POPPING OFF YO");
        if (!_poppingOff) {
            _poppingOff = true;

            foreach (var enemy in Mark.MarkedEnemies) {
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