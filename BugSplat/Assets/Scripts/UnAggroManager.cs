using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnAggroManager : GameLoop
{
    //Use A slower GameLoop for better performance;
    public EnemyList EnemyList;


    public override void LoopLateUpdate(float deltaTime)
    {

    }

    public override void LoopUpdate(float deltaTime)
    {
       foreach(Enemy in EnemyList)
    }
}
