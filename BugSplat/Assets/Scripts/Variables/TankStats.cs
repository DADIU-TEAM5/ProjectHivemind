using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TankStats : ScriptableObject
{
    public float MoveSpeed;
    public float HitPoints;
    

    public float AttackRange;
    public float AttackAngle;


    public float SpotDistance;
    

    public float AttackDamage;
    public float PushLength;

    public int minPartsToDrop;
    public int maxPartsToDrop;

}
