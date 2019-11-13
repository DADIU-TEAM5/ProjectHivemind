using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoomerStats : EnemyStats
{
    
    public float ChargeMoveSpeed;

    public float OrbitRadius;

    public float OrbitSpeed;

    public float SackSpawnCooldown;

    public bool SpawnsSacks;
    public float SackLifeTime;
    public GameObject SackToSpawn;

}
