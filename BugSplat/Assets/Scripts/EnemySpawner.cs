using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : GameLoop
{


    public static int LevelBudget;

    public static List<IEnumerator> QueuedSpawns;



    bool startedSpawning = false;

    public static bool SpawningDone;

    bool counted = false;

   // public levelBudget LevelBudget;
    public EnemySpawnerList EnemylevelList;
    public Color DisplayColor = Color.red;

    public IntVariable CurrentLevel;

    public IntVariable enemySpawnerCount;

    List<GameObject> enemies;
    public int budget;
    int[] _values;
    public int SmallestValue;

    float _timer;

    private void OnEnable()
    {
        budget = 0;

        //Debug.Log(name + " start budget " + budget);
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

        SmallestValue = int.MaxValue;

        _values = new int[enemies.Count];
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

        if (startedSpawning)
        {
            _timer += deltaTime;
            SpawnEnemiesRoutine();
        }

        if(_timer > 1.5)
        {
            Destroy(gameObject);
        }

    }

    public void SpawnEnemies()
    {
        startedSpawning = true;


    }

   

    void SpawnEnemiesRoutine()
    {


        

        if (LevelBudget+budget >=SmallestValue)
        {

            
            GameObject ChosenGuy = enemies[Random.Range(0, enemies.Count)];
            int FirsTValueToget = ChosenGuy.GetComponent<Enemy>().difficultyValue;

            if (LevelBudget > FirsTValueToget)
            {
                LevelBudget -= FirsTValueToget;
                

                GameObject spawnedEnemy = Instantiate(ChosenGuy, transform);

                spawnedEnemy.name = "Slightly Bigger Fucker";
                spawnedEnemy.transform.parent = null;

                Vector3 spawnPoint = transform.position;
                spawnPoint.y = 0;

                /*
                NavMeshHit hit;


                NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
                */
                spawnedEnemy.transform.position = spawnPoint;


            }
                
           


            
            while (budget > 0)
            {
                if (SmallestValue > budget)
                {
                    //Debug.Log("smallest value is bigger than budget");

                    //LevelBudget.usedBudget -= budget;
                    
                    LevelBudget += budget;
                    budget = 0;

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

                GameObject spawnedEnemy = Instantiate(enemies[index], transform);

                spawnedEnemy.name = "Little Fucker";
                spawnedEnemy.transform.parent = null;

                Vector3 spawnPoint = transform.position;
                spawnPoint.y = 0;

                /*
                NavMeshHit hit;


                NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
                */
                spawnedEnemy.transform.position = spawnPoint;


            }
            


        }
        else if(budget >0)
        {
            LevelBudget += budget;
            budget = 0;
        }
        // Debug.Log(name + " is done");

        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = DisplayColor;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
