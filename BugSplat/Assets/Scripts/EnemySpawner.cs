using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : GameLoop
{
    public FloatVariable LevelBudget;
    public EnemySpawnerList EnemylevelList;
    public Color DisplayColor = Color.red;

    public IntVariable CurrentLevel;

    public IntVariable enemySpawnerCount;

    List<GameObject> enemies;
    public float budget;
    float[] _values;
    float smallestValue;

    private void OnEnable()
    {
        enemies = new List<GameObject>();

        for (int i = 0; i < EnemylevelList.Levels[CurrentLevel.Value].Checks.Length; i++)
        {
            
            if (EnemylevelList.Levels[CurrentLevel.Value].Checks[i])
            {
                enemies.Add(EnemylevelList.Levels[CurrentLevel.Value].Enemies.SpawnableEnemies[i]);
            }


        }
        

        enemySpawnerCount.Value++;

        smallestValue = float.MaxValue;

        _values = new float[enemies.Count];
        for (int i = 0; i < enemies.Count; i++)
        {
            if (_values[i] < smallestValue)
                smallestValue = _values[i];

            _values[i] = enemies[i].GetComponent<Enemy>().difficultyValue;
        }
    }



    public override void LoopLateUpdate(float deltaTime)
    {
        
    }
    public override void LoopUpdate(float deltaTime)
    {
       
    }

    public void SpawnEnemies()
    {

        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        enemySpawnerCount.Value--;
        float extraBudget;
        if (enemySpawnerCount.Value <= 0)
        {
            extraBudget = LevelBudget.Value;
        }
        else
        {

            extraBudget = Random.Range(0, LevelBudget.Value);
            
        }

        LevelBudget.Value -= extraBudget;
        budget += extraBudget;

        while (budget > 0)
        {
            if (smallestValue > budget)
            {
                LevelBudget.Value += budget;
                break;
            }

            Debug.Log(budget);
            float valueToGet = float.MaxValue;
            int index = 0;
            while (valueToGet > budget)
            {
                index = Random.Range(0, enemies.Count);

                valueToGet = _values[index];
            }
            budget -= _values[index];

            GameObject spawnedEnemy = Instantiate(enemies[index]);
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
            spawnedEnemy.transform.position = hit.position;
            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = DisplayColor;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
