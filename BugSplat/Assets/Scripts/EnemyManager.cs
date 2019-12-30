using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyManager : SerializedMonoBehaviour
{
    [TabGroup("Minimum")]
    public int[] MinimumBudget;

    [TabGroup("Level")]
    public int[][] LevelBudgets;

    [BoxGroup("Per tile budget")]
    [Range(0f, 1f)]
    public float MaximumTileBudgetPercentile, MinimumTileBudgetPercentile;

    [BoxGroup("Per tile budget")]
    public AnimationCurve RandomDistribution;

    [BoxGroup("Variables")]
    public IntVariable LevelBudget;

    [BoxGroup("Variables")]
    public IntVariable CurrentLevel;
    [BoxGroup("Variables")]
    public IntVariable CurrentWave;

    private HexagonController[] Hexagons;


    // Start is called before the first frame update
    void Start()
    {
        Hexagons = FindObjectsOfType<HexagonController>();
        CurrentWave.Value = 0;

        DistributeMinimumBudget();
        DistributeLevelBudget();
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

    // This likely needs some rewrite
    public void DistributeLevelBudget() {
        var levelBudget = LevelBudgets[CurrentLevel.Value][CurrentWave.Value];
        var diff = MaximumTileBudgetPercentile - MinimumTileBudgetPercentile;

        LevelBudget.Value = levelBudget;

        ShuffleHexagons();
        for (var i = 0; i < Hexagons.Length; i++) {
            var randomValue = Random.Range(0f, 1f);
            randomValue *= RandomDistribution.Evaluate(randomValue);
            var randomLevelBudget = (int) (((randomValue * diff) + MinimumTileBudgetPercentile) * levelBudget);

            LevelBudget.Value -= randomLevelBudget;

            var hexSpawners = Hexagons[i].EnemySpawners;
            var perSpawnerBudget = randomLevelBudget / hexSpawners.Length;
            LevelBudget.Value += (randomLevelBudget - (perSpawnerBudget * hexSpawners.Length));
            for (var j = 0; j < hexSpawners.Length; j++) {
                hexSpawners[j].Budget += perSpawnerBudget;
            }

            if (LevelBudget.Value < 0) break;
        }
    }

    private void ShuffleHexagons() {
        for (var i = 0; i < Hexagons.Length; i++) {
            var randomIndex = Random.Range(0, Hexagons.Length);

            var temp = Hexagons[i];
            Hexagons[i] = Hexagons[randomIndex];
            Hexagons[randomIndex] = temp;

        }
    }
}
