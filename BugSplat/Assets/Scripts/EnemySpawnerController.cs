using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour, IPreprocessBuildWithReport
{
    public IntVariable LevelBudget;
    public int Budget;

    public EnemyType[] AvailableEnemyTypes;
    public EnemyPool EnemyPool;

    public IntVariable CurrentLevel;

    public int callbackOrder => 2000;

    private Vector3 _spawnPoint;

    [SerializeField]
    private int _smallestEnemyValue;

    [SerializeField]
    private Dictionary<int, Enemy[]> AvailableEnemies;

    public void OnPreprocessBuild(BuildReport report)
    {
        for (var i = 0; i < EnemyPool.Enemies.Length; i++) {
            var levelList = new List<Enemy>();

            var levelEnemies = EnemyPool.Enemies[i];
            for (var j = 0; j < levelEnemies.Length; j++) {
                var potentialEnemy = levelEnemies[j];
                if (AvailableEnemyTypes.Contains(potentialEnemy.EnemyType)) {
                    levelList.Add(potentialEnemy);
                }
            }

            if (levelList.Count > 0) {
                var sorted = levelList.OrderBy(x => x.difficultyValue);
                AvailableEnemies.Add(i, sorted.ToArray());
            }
        }
    }

    private void Awake() {
        var pos = transform.position;
        pos.y = 0f;
        _spawnPoint = pos;
    }

    public void SpawnEnemies() {
        // Spawner contains no enemies for this level
        if (!AvailableEnemies.ContainsKey(CurrentLevel.Value)) {
            LevelBudget.Value += Budget;
            Budget = 0;
            return;
        }

        var availableLevelEnemies = AvailableEnemies[CurrentLevel.Value];

        while (Budget > 0) {
            // Return remaining budget if it can't be used
            if (_smallestEnemyValue > Budget) {
                LevelBudget.Value += Budget;
                Budget = 0;
                break;
            }

            var randomIndex = Random.Range(0, availableLevelEnemies.Length);
            while (availableLevelEnemies[randomIndex].difficultyValue > Budget) {
                randomIndex--;
            }


            var enemy = Instantiate(availableLevelEnemies[randomIndex], _spawnPoint, Quaternion.identity);
            Budget -= enemy.difficultyValue;
        }
    }
}