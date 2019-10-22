using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonFodder : Enemy
{
    public override void TakeDamage(float damage)
    {
        print(name + " took damage "+ damage);
    }


    public override void LoopUpdate(float deltaTime)
    {

    }
    public override void LoopLateUpdate(float deltaTime)
    {

    }
}
