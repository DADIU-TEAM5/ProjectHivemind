using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Stats/Egg")]
public class EggStats : EnemyStats
{
    public float ChanceForEnemySpawn = 50;
    public float ChanceForCurrency = 10;

    public GameObject EnemyToSpawn;
}
