using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Enemy
{

    public GameObject Graphics;

    public GameObject bodyPart;

    bool _playerDetected;
    bool _isAlly;
    public SpitterStats stats;
    Transform _playerTransform;
    bool _attacking;
    float _attackCharge;

    float _currentHealth;

    float _attackCooldown = 0;


    bool _underground;

    Renderer _renderer;

    NavMeshAgent _navMeshAgent;



    [Header("Events")]
    public GameEvent TakeDamageEvent;
    public GameEvent AggroEvent;
    public GameEvent AttackEvent;
    public GameEvent DeathEvent;
    public GameEvent AttackChargingEvent;

    public void Start()
    {
        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();


        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.MoveSpeed;

    }

    public override bool IsVisible()
    {
        if (_underground)
        {
            return false;
                
        }

        return _renderer.isVisible;
    }

    public override void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);
        _currentHealth -= damage;
        TakeDamageEvent.Raise();

        if (_currentHealth <= 0)
        {
            int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);

                part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
            }

            DeathEvent.Raise();
            EnemyList.Remove(gameObject);

            
            Destroy(gameObject);
        }
    }


    public void Burrow()
    {
        _underground = true;
        Graphics.SetActive(false);
       
    }

    public override void LoopUpdate(float deltaTime)
    {
        if (_attackCooldown > 0)
            _attackCooldown -= Time.deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!_playerDetected)
        {
            _renderer.material.color = Color.blue;
            DetectThePlayer();
        }
        else if (playerInAttackRange() || _attacking)
        {
            if (_attackCooldown <= 0)
            {
                if (_navMeshAgent.destination != transform.position)
                    _navMeshAgent.destination = transform.position;

                _renderer.material.color = Color.red;
                Attack();
            }
            else
            {
                _renderer.material.color = Color.yellow;
                MoveTowardsThePlayer();
            }
        }
        else
        {
            _renderer.material.color = Color.yellow;
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    
    void Attack()
    {
        if (_attacking == false)
        {
            AttackChargingEvent.Raise();

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            
        }

        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {
            AttackEvent.Raise();
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));

            RaycastHit hit;
            if (potentialTargets.Length > 0 && Physics.Raycast(transform.position, potentialTargets[0].transform.position - transform.position, out hit))
            {
                if (hit.collider.gameObject.layer == 9)
                {

                    //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                    //if()
                    Vector3 temp = potentialTargets[0].transform.position;
                    temp.y = transform.position.y;


                    //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
                    if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < 10)
                    {
                        PlayerHealth playerHealth = potentialTargets[0].GetComponent<PlayerHealth>();
                        //apply damage to the player
                        if (playerHealth != null)
                        {
                            Vector3 directionToPush = potentialTargets[0].gameObject.transform.position - transform.position;
                            directionToPush.y = 0;
                            directionToPush = Vector3.Normalize(directionToPush);

                            playerHealth.TakeDamage(stats.AttackDamage);
                        }
                        else
                        {
                            Debug.LogError("target of " + gameObject.name + " attack got no health");
                        }
                    }
                }
                else
                    print("attack blocked by terrain or something");

            }
            else
                print("this should never show i guess");

            _attackCooldown = stats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            

        }

    }



    bool playerInAttackRange()
    {
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < stats.AttackRange / 2;
    }

    void MoveTowardsThePlayer()
    {
        float jitter = 2;
        _navMeshAgent.Move(new Vector3(Random.Range(-jitter, jitter), 0, Random.Range(-jitter, jitter)) * Time.deltaTime);

        if (_navMeshAgent.destination != _playerTransform.position)
            _navMeshAgent.destination = _playerTransform.position;
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
