using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Stats/Boomer")]
public class BoomerStats : EnemyStats
{
    
    public float ChargeMoveSpeed;

    public float OrbitRadius;

    public float OrbitSpeed;

    public float TargetDistanceToPlayer;


    public float SackSpawnCooldown;

    public bool SpawnsSacks;
    public float SackLifeTime;
    public GameObject SackToSpawn;


    public bool DiesWhenItExplode;



    public float AttackRangeTrigger = 1;
}
