using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : GameLoop
{

    public static List<IEnumerator> QueuedSpawns;



    public static bool SpawningDone;

    bool counted = false;

    public FloatVariable LevelBudget;
    public EnemySpawnerList EnemylevelList;
    public Color DisplayColor = Color.red;

    public IntVariable CurrentLevel;

    public IntVariable enemySpawnerCount;

    List<GameObject> enemies;
    public float budget;
    float[] _values;
    public float SmallestValue;

    private void OnEnable()
    {
        SpawningDone = true;

        if(QueuedSpawns == null)
        {
            QueuedSpawns = new List<IEnumerator>();
        }


        enemies = new List<GameObject>();

        for (int i = 0; i < EnemylevelList.Levels[CurrentLevel.Value].Checks.Length; i++)
        {
            
            if (EnemylevelList.Levels[CurrentLevel.Value].Checks[i])
            {
                enemies.Add(EnemylevelList.Levels[CurrentLevel.Value].Enemies.SpawnableEnemies[i]);
            }


        }

        //print(name + " has enemies " + enemies.Count);


        if (!counted)
        {
            enemySpawnerCount.Value++;
            counted = true;
        }

        SmallestValue = float.MaxValue;

        _values = new float[enemies.Count];
        for (int i = 0; i < enemies.Count; i++)
        {
            

            _values[i] = enemies[i].GetComponent<Enemy>().difficultyValue;


            if (_values[i] < SmallestValue)
                SmallestValue = _values[i];
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
        

        StartCoroutine( SpawnEnemiesRoutine() );
        
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        enemySpawnerCount.Value--;
        float extraBudget;
        if (enemySpawnerCount.Value <= 0)
        {
            Debug.Log("last enemy spawned left wit " + LevelBudget.Value + " budget to spawn for");
            extraBudget = LevelBudget.Value;
        }
        else
        {
            
            extraBudget = Random.Range(0, LevelBudget.Value);
            Debug.Log(name + " spawn enemies with a budget of " + (budget + extraBudget));


        }

        LevelBudget.Value -= extraBudget;
        budget += extraBudget;

        while (budget > 0)
        {
            if (SmallestValue > budget)
            {
                //Debug.Log("smallest value is bigger than budget");
                LevelBudget.Value += budget;
                
                break;
            }

            //Debug.Log("curren budget "+ budget);
            //Debug.Log("smallest value "+ _smallestValue);
            float valueToGet = float.MaxValue;
            int index = 0;
            while (valueToGet > budget)
            {
                index = Random.Range(0, enemies.Count);

                valueToGet = _values[index];

                
            }
            budget -= _values[index];

            GameObject spawnedEnemy = Instantiate(enemies[index],transform);

            Vector3 spawnPoint = transform.position;
            spawnPoint.y = 0;

            /*
            NavMeshHit hit;
            

            NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
            */
            spawnedEnemy.transform.position = spawnPoint;

            yield return new WaitForSeconds(0.2f);
        }


        Debug.Log(name + " is done");
        
        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = DisplayColor;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
