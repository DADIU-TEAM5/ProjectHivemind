using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Enemy
{

    

    
    public ParticleSystem Spit;


    
    
    SpitterStats _spitterStats;
    
    bool _attacking;
    float _attackCharge;

    

    float _attackCooldown = 0;

    float _burrowLerp;

    float _fleeValue;

    bool _underground;

    float _waitForPathCalc;

    

    

    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    
    public GameEvent AttackChargingEvent;
    public GameEvent BurrowEvent;

    public GameEvent EmergeEvent;


    public void Start()
    {

        _spitterStats = (SpitterStats)stats;

        var spitSettings = Spit.main;
        spitSettings.startSpeed = _spitterStats.ProjectileSpeed;
        spitSettings.startLifetime = _spitterStats.AttackRange/_spitterStats.ProjectileSpeed ;

        
        Burrow();

        

    }

    

    

    


    public void Burrow()
    {
        if (!_underground)
        {
            BurrowEvent.Raise(gameObject);

            _underground = true;

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }
    public void Emerge()
    {
        if (_underground)
        {
            EmergeEvent.Raise(this.gameObject);

            _underground = false;

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }


    public override void LoopUpdate(float deltaTime)
    {

        if (_waitForPathCalc > 0)
            _waitForPathCalc -= deltaTime;

        RemoveFromLockedTargetIfNotVisible();

        if (_underground )
        {

            if (_burrowLerp < 1)
            {
                _burrowLerp += deltaTime / _spitterStats.RetractionTime;
            }


        }
        else {

            if (_burrowLerp > 0)
            {
                _burrowLerp -= deltaTime / _spitterStats.RetractionTime;
            }
        }
        Vector3 tempPos = Graphics.transform.localPosition;

        tempPos.y = Mathf.Lerp(0, -2.3f, _burrowLerp);

        Graphics.transform.localPosition = tempPos;

       // print(_fleeValue);


        if (_fleeValue > 0)
            _fleeValue -= deltaTime;

        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
        {
            Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if (playerInAttackRange() || _attacking)
        {
            if (_fleeValue <= 0)
            {

                Emerge();

                NavMeshAgent.destination = transform.position;

                if (_attackCooldown <= 0 && _burrowLerp <= 0)
                {
                    if (NavMeshAgent.destination != transform.position)
                        NavMeshAgent.destination = transform.position;

                    Renderer.material.color = SetColor(Color.red);
                    Attack();
                }
                else
                {
                    LookAtPlayer();
                    Renderer.material.color = SetColor(Color.yellow);

                }
            }
            else
            {
                Renderer.material.color = SetColor(Color.yellow);
                MoveTowardsThePlayer();
            }
        }
        else
        {
            Renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    void LookAtPlayer()
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        Quaternion diseredRotation= Quaternion.LookRotation(adjustedPlayerPos -transform.position );


        transform.rotation = Quaternion.Lerp(transform.rotation, diseredRotation, Time.deltaTime*5);


    }
    void Attack()
    {
        if (_attacking == false)
        {
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;
            transform.LookAt(adjustedPlayerPos);



        }

        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= _spitterStats.AttackChargeUpTime)
        {
            


            AttackEvent.Raise(gameObject);
            Spit.Emit(1);

            _attackCooldown = _spitterStats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            

        }

    }



    

    void MoveTowardsThePlayer()
    {
        Burrow();

        if (_burrowLerp >= 1 && _waitForPathCalc<=0)
        {
            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            Vector3 destination = Vector3.zero;

            


            destination = adjustedPlayerPos+((transform.position-adjustedPlayerPos).normalized*_spitterStats.AttackRange);


            

            //destination = hit.position;


            if (NavMeshAgent.destination != destination)
                NavMeshAgent.destination = destination;

            _waitForPathCalc = .5f;
        }
    }


    int[] _triangles = { };
    Vector3[] _normals = { };

    public void DrawSpitTrajectory()
    {
        if (_triangles.Length != 6)
        {
            _triangles = new int[6];

            

            _triangles[0] = 0;
            _triangles[1] = 1;
            _triangles[2] = 2;

            _triangles[3] = 0;
            _triangles[4] = 3;
            _triangles[5] = 2;



        }

        if (_normals.Length != 4)
        {

            _normals = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normals[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[4];






        vertices[0] = Vector3.zero;


        Vector3 vectorToRotate;


        
        vectorToRotate = Vector3.forward * stats.AttackRange;
        

        Vector3 rotatedVector = Vector3.zero;

        
        int step = 0;



        

        OutlineMesh.vertices = vertices;

        if (OutlineMesh.triangles != _triangles)
            OutlineMesh.triangles = _triangles;

        if (OutlineMesh.normals != _normals)
            OutlineMesh.normals = _normals;





    }


}
