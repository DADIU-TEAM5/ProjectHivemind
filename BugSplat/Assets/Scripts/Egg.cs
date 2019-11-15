using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Enemy
{

    EggStats _eggStats;
    private void Start()
    {
        _eggStats = (EggStats)stats;
    }

    public override void LoopBehaviour(float deltaTime)
    {
        
    }
    public override void LoopLateUpdate(float deltaTime)
    {
        
    }
    public override void LoopUpdate(float deltaTime)
    {
        
    }
    public override void TakeDamageBehaviour(float damage)
    {
        
        if( _currentHealth <= 0)
        {

            if (Random.Range(0, 100) < _eggStats.ChanceForEnemySpawn)
            {
                GameObject enemy = Instantiate(_eggStats.EnemyToSpawn, transform);
                enemy.transform.parent = null;
            }
            else
            {
                int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
                for (int i = 0; i < partsToDrop; i++)
                {
                    GameObject part = Instantiate(bodyPart);

                    part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
                }
            }
           

        }

    }
}
