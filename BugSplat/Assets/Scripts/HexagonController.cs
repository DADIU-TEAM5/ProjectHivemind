using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class HexagonController : MonoBehaviour, IPreprocessBuildWithReport
{
    public EnemySpawnerController[] EnemySpawners;

    public int callbackOrder => 2000;

    public void OnPreprocessBuild(BuildReport report)
    {
        EnemySpawners = FindObjectsOfType<EnemySpawnerController>();
    }

    private void Awake() {
        #if UNITY_EDITOR
            OnPreprocessBuild(null);
        #endif
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < EnemySpawners.Length; i++)
        {
            EnemySpawners[i].SpawnEnemies();
        }
    }
}