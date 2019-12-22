using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Stats/Spitter")]
public class SpitterStats : EnemyStats
{
    public float ProjectileSpeed;

    public bool ShootEggs;
    public float ChanceToShootEgg;

    public float FleeTime;
    public float FleeThreshold;

    public float RetractionTime;
}
