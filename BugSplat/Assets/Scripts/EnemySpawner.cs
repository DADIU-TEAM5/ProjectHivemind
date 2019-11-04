using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GameLoop
{
    public GameObject[] enemies;
    public float budget;
    public override void LoopLateUpdate(float deltaTime)
    {
        
    }
    public override void LoopUpdate(float deltaTime)
    {
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, transform.localScale.magnitude);
    }

}
