using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Mark Enemy")]
public class MarkEffect : Effect
{
    public List<Enemy> MarkedEnemies = new List<Enemy>();

    public override void Init()
    {
        MarkedEnemies = new List<Enemy>();
    }

    public override void Trigger(GameObject target = null)
    {
        Debug.Log("MARKING YOUR BOI");
        var enemy = target?.GetComponent<Enemy>();

        if (enemy == null) return;

        if (!MarkedEnemies.Contains(enemy)) {
            MarkedEnemies.Add(enemy);
        }
    }
}
