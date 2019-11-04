using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Enemy
{

    public GameObject Graphics;

    public GameObject bodyPart;
    public ParticleSystem Spit;


    bool _playerDetected;
    bool _isAlly;
    public SpitterStats stats;
    Transform _playerTransform;
    bool _attacking;
    float _attackCharge;

    float _currentHealth;

    float _attackCooldown = 0;

    float _burrowLerp;

    float _fleeValue;

    bool _underground;

    float _waitForPathCalc;

    Renderer _renderer;

    NavMeshAgent _navMeshAgent;


    Color _startColor;
    [Header("Events")]
    public GameEvent TakeDamageEvent;
    public GameEvent AggroEvent;
    public GameEvent AttackEvent;
    public GameEvent DeathEvent;
    public GameEvent AttackChargingEvent;


    Color SetColor(Color color)
    {
        return Color.Lerp(_startColor, color, 0.5f);
    }

    public void Start()
    {
        var spitSettings = Spit.main;
        spitSettings.startSpeed = stats.ProjectileSpeed;
        spitSettings.startLifetime = stats.AttackRange/stats.ProjectileSpeed ;


        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();


        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.MoveSpeed;

        _startColor = _renderer.material.color;
        Burrow();

    }

    public override bool IsVisible()
    {
        if (_renderer == null)
        {
            _renderer = Graphics.GetComponent<Renderer>();
        }

        if (_underground)
        {
            return false;
                
        }

        return _renderer.isVisible;
    }

    public override void TakeDamage(float damage)
    {
        if (_burrowLerp <= 0)
        {
            // print(name + " took damage "+ damage);
            _currentHealth -= damage;

            if(_currentHealth < stats.FleeThreshold)
            _fleeValue = stats.FleeTime;

            TakeDamageEvent.Raise(gameObject);

            if (_currentHealth <= 0)
            {
                int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
                for (int i = 0; i < partsToDrop; i++)
                {
                    GameObject part = Instantiate(bodyPart);

                    part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
                }

                DeathEvent.Raise(gameObject);
                EnemyList.Remove(gameObject);


                Destroy(gameObject);
            }
        }
    }


    public void Burrow()
    {
        _underground = true;

        _navMeshAgent.obstacleAvoidanceType =ObstacleAvoidanceType.NoObstacleAvoidance;
        
       
    }
    public void Emerge()
    {
        
        _underground = false;
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

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
                _burrowLerp += deltaTime / stats.RetractionTime;
            }


        }
        else {

            if (_burrowLerp > 0)
            {
                _burrowLerp -= deltaTime / stats.RetractionTime;
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

        if (!_playerDetected)
        {
            _renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if (playerInAttackRange() || _attacking)
        {
            if (_fleeValue <= 0)
            {

                Emerge();

                _navMeshAgent.destination = transform.position;

                if (_attackCooldown <= 0 && _burrowLerp <= 0)
                {
                    if (_navMeshAgent.destination != transform.position)
                        _navMeshAgent.destination = transform.position;

                    _renderer.material.color = SetColor(Color.red);
                    Attack();
                }
                else
                {
                    LookAtPlayer();
                    _renderer.material.color = SetColor(Color.yellow);

                }
            }
            else
            {
                _renderer.material.color = SetColor(Color.yellow);
                MoveTowardsThePlayer();
            }
        }
        else
        {
            _renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    void LookAtPlayer()
    {
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        Quaternion diseredRotation= Quaternion.LookRotation(adjustedPlayerPos -transform.position );


        transform.rotation = Quaternion.Lerp(transform.rotation, diseredRotation, Time.deltaTime*5);


    }
    void Attack()
    {
        if (_attacking == false)
        {
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;
            transform.LookAt(adjustedPlayerPos);



        }

        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {
            


            AttackEvent.Raise(gameObject);
            Spit.Emit(1);

            _attackCooldown = stats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            

        }

    }



    bool playerInAttackRange()
    {
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) <= stats.AttackRange ;
    }

    void MoveTowardsThePlayer()
    {
        Burrow();

        if (_burrowLerp >= 1 && _waitForPathCalc<=0)
        {
            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            Vector3 destination = Vector3.zero;

            


            destination = adjustedPlayerPos+((transform.position-adjustedPlayerPos).normalized*stats.AttackRange);


            

            //destination = hit.position;


            if (_navMeshAgent.destination != destination)
                _navMeshAgent.destination = destination;

            _waitForPathCalc = .5f;
        }
    }

    void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Player"));

        if (potentialTargets.Length > 0)
        {
            AggroEvent.Raise();
            _playerDetected = true;
            _playerTransform = potentialTargets[0].gameObject.transform;

            _isAlly = true;

           
        }
    }
}
