using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int[][] LevelBudgets;
    public int[] MinimumBudget;

    public IntVariable LevelBudget;

    public IntVariable CurrentLevel;
    public IntVariable CurrentWave;

    private Hexagon[] Hexagons;


    // Start is called before the first frame update
    void Start()
    {
        Hexagons = FindObjectsOfType<Hexagon>();
        CurrentWave.Value = 0;

        DistributeMinimumBudget();
    }

    public void DistributeMinimumBudget() {
        for (var i = 0; i < Hexagons.Length; i++) {
            var hex = Hexagons[i];

            for (var j = 0; j < hex.EnemySpawners.Length; j++) {
                var enemySpawner = hex.EnemySpawners[j];
                enemySpawner.Budget += MinimumBudget[CurrentLevel.Value];
            }
        }
    }

    public void DistributeLevelBudget() {
        var levelBudget = LevelBudgets[CurrentLevel.Value][CurrentWave.Value];


    }
}
