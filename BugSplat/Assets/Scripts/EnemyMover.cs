using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : GameLoop
{

    public override void LoopUpdate(float deltaTime)
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 15);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.layer == 8)
            {
                GameObject EnemyToMove = colls[i].gameObject;

                EnemyToMove.GetComponent<NavMeshAgent>().enabled = false;

                Vector3 enemypos = EnemyToMove.transform.position;

                enemypos.y = transform.position.y;

                Vector3 direction =  enemypos - transform.position;
                direction = direction.normalized;

                EnemyToMove.transform.position = direction * 20;
                EnemyToMove.GetComponent<NavMeshAgent>().enabled = true;

            }

        }
        
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       
    }

    
}
