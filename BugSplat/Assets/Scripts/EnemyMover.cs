using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : GameLoop
{


    public BoolVariable DoorAreOpen;

    private void OnEnable()
    {
        DoorAreOpen.Value = false;
    }


    public override void LoopUpdate(float deltaTime)
    {

        if (DoorAreOpen.Value)
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

                    Vector3 direction = enemypos - transform.position;
                    direction = direction.normalized;

                    EnemyToMove.transform.position = direction * 20;
                    EnemyToMove.GetComponent<NavMeshAgent>().enabled = true;

                }
                else if (colls[i].gameObject.layer == 9)
                {

                    GameObject PlayerToMove = colls[i].gameObject;

                    NavMeshAgent agent = PlayerToMove.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        print("WWAAYWAWEWAH");
                        agent.enabled = false;

                        Vector3 newPos = transform.position;

                        newPos.y = PlayerToMove.transform.position.y;


                        Vector3 direction = PlayerToMove.transform.position - newPos;
                        direction = direction.normalized;

                        PlayerToMove.transform.position = direction * 20;
                        
                        agent.enabled = true;

                    }
                        


                }

            }
        }
        
        
    }

    public override void LoopLateUpdate(float deltaTime)
    {
       
    }

    
}
