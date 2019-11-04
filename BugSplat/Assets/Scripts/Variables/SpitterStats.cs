using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpitterStats : ScriptableObject
{
    public float MoveSpeed;
    
    public float HitPoints;

    public float ProjectileSpeed;

    public float AttackRange;

    public float FleeTime;
    public float FleeThreshold;

    public float RetractionTime;

    public float SpotDistance;
    

    public float AttackDamage;
    public float AttackChargeUpTime;
    public float AttackSpeed;



    public int minPartsToDrop;
    public int maxPartsToDrop;

}
