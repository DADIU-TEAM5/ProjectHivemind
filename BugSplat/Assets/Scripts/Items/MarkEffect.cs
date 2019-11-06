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
            Debug.Log("Triggering Mark");
            // Pop the mark
            Debug.Log($"Number of marked enemies {MarkedEnemies.Count}");
            foreach (var markedEnemy in MarkedEnemies) {
                if (markedEnemy.isActiveAndEnabled) {
                    MarkPopEffect.Trigger(markedEnemy.gameObject);
                }
            }

            MarkedEnemies.Clear();
        } else {
            Debug.Log("Oh hi Mark");
            if (!MarkedEnemies.Contains(enemy))
                MarkedEnemies.Add(enemy);
        }
    }

    public void OnDrawGizmos() {
        Debug.Log("Drawing gizzies");

        foreach (var enemy in MarkedEnemies) {
            Gizmos.DrawCube(enemy.transform.position, new Vector3(1, 0.1f, 1));
        }
    }

    public override void Init()
    {
        MarkedEnemies = new List<Enemy>();
    }
}
