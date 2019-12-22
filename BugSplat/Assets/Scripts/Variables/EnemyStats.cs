using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Stats/Enemy")]
public class EnemyStats : ScriptableObject
{
    public float MoveSpeed;
    public float HitPoints;
    public float AttackSpeed;
    public float AttackRange;
    public float AttackAngle;
    public float SpotDistance;
    public float AllySpotDistance;


    public float AttackChargeUpTime;
    public float AttackDamage;

    public int minPartsToDrop;
    public int maxPartsToDrop;
}
