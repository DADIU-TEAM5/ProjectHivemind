using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : GameLoop
{
    public GameEvent SpawnAllEnemies;
    public GameEvent NextWaveEvent;
    public GameEvent InitialSpawnEvent;
    public GameObjectVariable ThePlayer;

    public FloatVariable InitalAggroDelay;
    public IntVariable EnemiesLeftBeforeNewWave;

    public IntVariable TotalEnemyCount;
    
    public static int LevelBudget;

    public static int[] WaveLevelBudget;
    public IntVariable NumberOfWavesSO;
    public static bool IsWave;

    public static bool SingleEnemySpawned;

    public static List<GameObject>[] EnemiesInWaves;

    public static List<IEnumerator> QueuedSpawns;

    

    public float SpawnCD = 2;

    float _waveTimeDelay;


    int _currentWave;


    bool startedSpawning = false;
    bool firstSpawn = false;

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
    [HideInInspector]
    public Hexagon hex;

    bool SpawningEnemiesToSpawn;

    List<GameObject> EnemiesToSpawn = new List<GameObject>();


    bool _waveBegun;

    private void OnEnable()
    {
      

        _waveTimeDelay = 0;
        _currentWave = 0;
        _waveBegun = false;

        if (IsWave && EnemiesInWaves  == null)
        {
         
            NumberOfWavesSO.Value = WaveLevelBudget.Length;
            EnemiesInWaves = new List<GameObject>[WaveLevelBudget.Length];

            for (int i = 0; i < EnemiesInWaves.Length; i++)
            {
                EnemiesInWaves[i] = new List<GameObject>();
            }

        }
        else
        {
   
        }


        SingleEnemySpawned = false;
        SpawningEnemiesToSpawn = false;

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


    public void RemoveEnemyFromWave(GameObject enemy)
    {
        if (WaveLevelBudget == null || WaveLevelBudget.Length <= _currentWave)
            return;


        if (EnemiesInWaves[_currentWave].Contains(enemy))
        {
            EnemiesInWaves[_currentWave].Remove(enemy);
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {
        
    }
    public override void LoopUpdate(float deltaTime)
    {
       // print(_waveTimeDelay);

       // print("delta time" + deltaTime);

       // print("current wave is " + _currentWave + " budget is " + WaveLevelBudget[_currentWave]+ " wave begun equal "+_waveBegun+" enemies in wave"+ EnemiesInWaves[_currentWave].Count);

        //print("Enemies left in wave "+ EnemiesInWaves[_currentWave].Count);
        //print("wave is active equals " + _waveBegun);

        if (startedSpawning)
        {
            


            if (IsWave)
            {

                if (_waveTimeDelay > 0)
                    _waveTimeDelay -= deltaTime;


                if (EnemiesInWaves.Length > _currentWave && EnemiesInWaves[_currentWave].Count > 0 && _waveBegun == false)
                {

                   // print("wave should have begun");
                    _waveBegun = true;

                    _waveTimeDelay = 3;
                }

                

                SpawnWaves();


                if(_waveBegun && _waveTimeDelay <= 0)
                {
                    //print("nexy wave is Able TO begin");

                    if(EnemiesInWaves[_currentWave].Count < EnemiesLeftBeforeNewWave.Value)
                    {
                        NextWaveEvent.Raise();
                        _currentWave++;
                        _waveBegun = false;


                    }

                }

            }
            else
            {

                SpawnEnemiesRoutine();


                
            }


            if (_timer > 0)
                _timer -= deltaTime;


            if (_timer <= 0)
            {
                SpawnQueuedEnemies();
                _timer = SpawnCD;
            }

        }


        
        

    }

    public void SpawnEnemies()
    {

        SpawningEnemiesToSpawn = true;
        


    }

    public void SpawnFirstEnemy()
    {
        startedSpawning = true;
        //SpawnEnemies();
        /*
        if (!SingleEnemySpawned && SmallestValue <LevelBudget)
        {
            SingleEnemySpawned = true;

            GameObject ChosenGuy = enemies[Random.Range(0, enemies.Count)];
            int FirsTValueToget = ChosenGuy.GetComponent<Enemy>().difficultyValue;
            while(FirsTValueToget > LevelBudget)
            {
                ChosenGuy = enemies[Random.Range(0, enemies.Count)];
                FirsTValueToget = ChosenGuy.GetComponent<Enemy>().difficultyValue;
            }


            if (LevelBudget > FirsTValueToget)
            {
                LevelBudget -= FirsTValueToget;


                GameObject spawnedEnemy = Instantiate(ChosenGuy, transform);
                spawnedEnemy.GetComponent<Enemy>().SpecialAggroEvent = SpawnAllEnemies;

                spawnedEnemy.name = "Slightly Bigger Fucker";
                spawnedEnemy.transform.parent = null;

                Vector3 spawnPoint = transform.position;
                spawnPoint.y = 0;

                /*
                NavMeshHit hit;


                NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
                
                spawnedEnemy.transform.position = spawnPoint;


            }


        }
*/


    }


   

    void SpawnQueuedEnemies()
    {

        if (SpawningEnemiesToSpawn || IsWave)
        {
            int count = EnemiesToSpawn.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject spawnedEnemy = Instantiate(EnemiesToSpawn[0], transform);


                Enemy enemyScript = spawnedEnemy.GetComponent<Enemy>();
                spawnedEnemy.name = "Delayed Fucker";
                spawnedEnemy.transform.parent = null;



                if (IsWave && _currentWave == 0)
                {
                    StartCoroutine(WaitAndThenAggro(enemyScript));
                }
                else
                {
                    enemyScript.PlayerTransform = ThePlayer.Value.transform;
                    enemyScript.PlayerDetected = true;
                }


                

                Vector3 spawnPoint = transform.position;
                spawnPoint.y = 0;

                /*
                NavMeshHit hit;


                NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
                */
                spawnedEnemy.transform.position = spawnPoint;

                if (IsWave && EnemiesInWaves.Length > _currentWave)
                {

                    print("we are waveing");
                    spawnedEnemy.name += " " + _currentWave;
                    EnemiesInWaves[_currentWave].Add(spawnedEnemy);

                }

                EnemiesToSpawn.Remove(EnemiesToSpawn[0]);

            }
        }
    }


    IEnumerator WaitAndThenAggro(Enemy enemyScript)
    {
        print("we doing a coroutine");

        yield return new WaitForSeconds(InitalAggroDelay.Value);

        print("we waited");
        enemyScript.PlayerTransform = ThePlayer.Value.transform;
        enemyScript.PlayerDetected = true;

        yield return null;
    }


    void SpawnEnemiesRoutine()
    {


        

        if (LevelBudget+budget >=SmallestValue)
        {

            if(Random.Range(0,100)<40)
            {
                GameObject ChosenGuy = enemies[Random.Range(0, enemies.Count)];
                int FirsTValueToget = ChosenGuy.GetComponent<Enemy>().difficultyValue;

                if (LevelBudget >= FirsTValueToget)
                {
                    LevelBudget -= FirsTValueToget;


                    EnemiesToSpawn.Add(ChosenGuy);
                    TotalEnemyCount.Value++;
                    InitialSpawnEvent.Raise();
                }
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


                spawnedEnemy.GetComponent<Enemy>().hex = hex;
                spawnedEnemy.name = "Initial Fucker";
                TotalEnemyCount.Value++;
                InitialSpawnEvent.Raise(spawnedEnemy);
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


    void SpawnWaves()
    {

        if (WaveLevelBudget == null || WaveLevelBudget.Length <= _currentWave)
            return;

        

        if (WaveLevelBudget[_currentWave] + budget >= SmallestValue)
        {

            if (Random.Range(0, 100) < 40)
            {
                GameObject ChosenGuy = enemies[Random.Range(0, enemies.Count)];
                int FirsTValueToget = ChosenGuy.GetComponent<Enemy>().difficultyValue;

                if (WaveLevelBudget[_currentWave] >= FirsTValueToget)
                {
                    WaveLevelBudget[_currentWave] -= FirsTValueToget;

                    TotalEnemyCount.Value++;
                    InitialSpawnEvent.Raise();
                    Debug.Log("Add tha ChosenGuy to wave");
                    EnemiesToSpawn.Add(ChosenGuy);
                    
                }
            }
             





            while (budget > 0)
            {
                if (SmallestValue > budget)
                {
                    //Debug.Log("smallest value is bigger than budget");

                    //LevelBudget.usedBudget -= budget;

                    WaveLevelBudget[_currentWave] += budget;
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


                Enemy enemyScript = spawnedEnemy.GetComponent<Enemy>();


                

                if (IsWave && _currentWave == 0)
                {
                    StartCoroutine(WaitAndThenAggro(enemyScript));
                }
                else
                {
                    enemyScript.PlayerTransform = ThePlayer.Value.transform;
                    enemyScript.PlayerDetected = true;

                }




                spawnedEnemy.name = "Initial Fucker";
                TotalEnemyCount.Value++;
                InitialSpawnEvent.Raise(spawnedEnemy);
                spawnedEnemy.transform.parent = null;

                Vector3 spawnPoint = transform.position;
                spawnPoint.y = 0;

                /*
                NavMeshHit hit;


                NavMesh.SamplePosition(transform.position, out hit, 3, NavMesh.AllAreas);
                */


                if (IsWave && EnemiesInWaves.Length > _currentWave)
                {
                    spawnedEnemy.name += " " + _currentWave;
                    EnemiesInWaves[_currentWave].Add(spawnedEnemy);
                }

                spawnedEnemy.transform.position = spawnPoint;


            }



        }
        else if (budget > 0)
        {
            WaveLevelBudget[_currentWave] += budget;
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
