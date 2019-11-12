using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Enemy
{

    

    
    public ParticleSystem Spit;


    public Animator WormAnimator;
    
    
    SpitterStats _spitterStats;
    
    bool _attacking;
    float _attackCharge;

    [HideInInspector]
    public bool FullyUnderground;
    

    float _attackCooldown = 0;

    

    float _fleeValue;

    

    float _waitForPathCalc;

    Vector3 _playerAttackPos;

    

    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    
    public GameEvent AttackChargingEvent;
    public GameEvent BurrowEvent;

    public GameEvent EmergeEvent;


    float _percentIncrease;
    float _chargeCLipLength;
    public AnimationClip ChargeAnimation;

    public void Start()
    {

        _spitterStats = (SpitterStats)stats;

        var spitSettings = Spit.main;
        spitSettings.startSpeed = _spitterStats.ProjectileSpeed;
        spitSettings.startLifetime = _spitterStats.AttackRange/_spitterStats.ProjectileSpeed ;
        _chargeCLipLength = ChargeAnimation.length;


        Burrow();

        

        float Increase = _chargeCLipLength - stats.AttackChargeUpTime;

        float percenIncrease = Increase / stats.AttackChargeUpTime;
        _percentIncrease = percenIncrease;




    }

    

    

    


    public void Burrow()
    {
        if (!FullyUnderground)
        {
            WormAnimator.SetBool("Underground", true);
            BurrowEvent.Raise(gameObject);
            FullyUnderground = true;
            

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }
    public void Emerge()
    {
        if (FullyUnderground)
        {

            WormAnimator.SetBool("Underground", false);
            EmergeEvent.Raise(this.gameObject);
            FullyUnderground = false;
            

            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }


    public override void LoopUpdate(float deltaTime)
    {

        

        if (_waitForPathCalc > 0)
            _waitForPathCalc -= deltaTime;

        RemoveFromLockedTargetIfNotVisible();

        
        

       // print(_fleeValue);


        if (_fleeValue > 0)
            _fleeValue -= deltaTime;

        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
        {
            Burrow();
            Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if (playerInRangedAttackRange() || _attacking)
        {
            if (_fleeValue <= 0)
            {
                
                Emerge();

                NavMeshAgent.destination = transform.position;

                if (_attackCooldown <= 0 && !FullyUnderground)
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






            _playerAttackPos = PlayerTransform.position;




            WormAnimator.speed = 1 + _percentIncrease;

            WormAnimator.SetTrigger("ChargeAttack");

            Outline.SetActive(true);
            Cone.SetActive(true);
            DrawSpitTrajectory();


        }
        DrawSpitFillup();

        ConeRenderer.material.color = Color.Lerp(new Color(0, 1, 0, 0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);

        //print(_attackCharge / _spitterStats.AttackChargeUpTime);

        //WormAnimator.SetFloat("ChargeTime", _attackCharge/_spitterStats.AttackChargeUpTime);




        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= _spitterStats.AttackChargeUpTime)
        {
            print("spit emittet");

            Vector3 temppos = _playerAttackPos;
            temppos.y = Spit.transform.position.y;


            Spit.transform.LookAt(temppos);

            AttackEvent.Raise(gameObject);
            Spit.Emit(1);

            _attackCooldown = _spitterStats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            WormAnimator.speed = 1;

            Outline.SetActive(false);
            Cone.SetActive(false);

        }

    }



    

    void MoveTowardsThePlayer()
    {
        Burrow();

        if (FullyUnderground && _waitForPathCalc<=0)
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


    int[] _trianglesfortraj = { };
    Vector3[] _normalsfotraj = { };

    public void DrawSpitTrajectory()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];

            

            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[4];




        float trajectoryWidt = 0.5f;

        vertices[0] =  Vector3.right * trajectoryWidt;
        vertices[1] =  Vector3.left * trajectoryWidt;


        vertices[2] =  (Vector3.right* trajectoryWidt) + (Vector3.forward * stats.AttackRange);
        vertices[3] =  (Vector3.left* trajectoryWidt) + (Vector3.forward * stats.AttackRange);


        
        

        OutlineMesh.vertices = vertices;

        if (OutlineMesh.triangles != _trianglesfortraj)
            OutlineMesh.triangles = _trianglesfortraj;

        if (OutlineMesh.normals != _normalsfotraj)
            OutlineMesh.normals = _normalsfotraj;





    }


    public void DrawSpitFillup()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];



            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[4];




        float trajectoryWidt = 0.5f;

        vertices[0] = Vector3.right * trajectoryWidt;
        vertices[1] = Vector3.left * trajectoryWidt;


        vertices[2] = (Vector3.right * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, stats.AttackRange,_attackCharge/stats.AttackChargeUpTime));
        vertices[3] = (Vector3.left * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, stats.AttackRange, _attackCharge / stats.AttackChargeUpTime));







        ConeMesh.vertices = vertices;

        if (ConeMesh.triangles != _trianglesfortraj)
            ConeMesh.triangles = _trianglesfortraj;

        if (ConeMesh.normals != _normalsfotraj)
            ConeMesh.normals = _normalsfotraj;





    }


}
