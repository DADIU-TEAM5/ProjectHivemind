using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : GameLoop
{
    public GameObject[] enemies;
    public float budget;
    float[] _values;
    float smallestValue;

    private void OnEnable()
    {
        smallestValue = float.MaxValue;

        _values = new float[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
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
        while (budget > 0)
        {
            if (smallestValue > budget)
                break;

            Debug.Log(budget);
            float valueToGet = float.MaxValue;
            int index = 0;
            while (valueToGet > budget)
            {
                index = Random.Range(0, enemies.Length);

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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
