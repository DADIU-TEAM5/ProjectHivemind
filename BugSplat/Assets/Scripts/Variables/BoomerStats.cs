using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoomerStats : ScriptableObject
{
    public float MoveSpeed;
    public float ChargeMoveSpeed;

    public float HitPoints;
    public float AttackSpeed;
    public float AttackRange;
    
    public float SpotDistance;
    public float AttackChargeUpTime;
    public float AttackDamage;

    public int minPartsToDrop;
    public int maxPartsToDrop;

}
