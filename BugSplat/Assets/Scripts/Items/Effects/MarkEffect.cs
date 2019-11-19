using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Mark Enemy")]
public class MarkEffect : Effect
{
    public List<Enemy> MarkedEnemies = new List<Enemy>();
    
    public ParticleController MarkParticleEffect;

    private List<ParticleController> MarkedParticles;

    public override void Init()
    {
        MarkedEnemies = new List<Enemy>();

        if (MarkedParticles?.Count > 0) {
            for (var i = 0; i < MarkedParticles.Count; i++) {
                var mp = MarkedParticles[i];
                if (mp.isActiveAndEnabled) {
                    Destroy(mp.gameObject);
                }
            }
        }

        MarkedParticles = new List<ParticleController>();
    }

    public override void Trigger(GameObject target = null)
    {
        var enemy = target?.GetComponent<Enemy>();

        if (enemy == null) return;

        if (!MarkedEnemies.Contains(enemy)) {
            MarkedEnemies.Add(enemy);

            // Visual
            var particleController = Instantiate(MarkParticleEffect, enemy.transform);
            particleController.transform.localPosition = Vector3.zero;
            particleController.Play();
        }
    }
}
