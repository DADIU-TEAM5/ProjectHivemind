using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : GameLoop
{
    public override void LoopLateUpdate(float deltaTime)
    {
    }
    public override void LoopUpdate(float deltaTime)
    {
        //print(EnemySpawner.SpawningDone);
        if (EnemySpawner.SpawningDone)
        {
            

            if (EnemySpawner.QueuedSpawns.Count > 0)
            {
               // print("started spawning enemies for "+EnemySpawner.QueuedSpawns.Count+" enemyspawners");

                StartCoroutine(EnemySpawner.QueuedSpawns[0]);

                EnemySpawner.QueuedSpawns.Remove(EnemySpawner.QueuedSpawns[0]);

                EnemySpawner.SpawningDone = false;

            }
        }
    }


}
