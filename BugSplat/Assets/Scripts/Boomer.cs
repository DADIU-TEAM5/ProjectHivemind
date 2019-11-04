using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boomer : Enemy
{
    public GameObject Graphics;

    public GameObject bodyPart;

    public BoomerStats stats;

    private bool _playerDetected;
    
    private Transform _playerTransform;
    private bool _attacking;
    private float _attackCharge;
    private float _attackCooldown = 0;

    private float _currentHealth;

    private NavMeshAgent _navMeshAgent;

    private Renderer _renderer;
    private GameObject _cone;
    private LineRenderer _coneRenderer;

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

        _cone = new GameObject();
        _cone.AddComponent<LineRenderer>();
        _coneRenderer = _cone.GetComponent<LineRenderer>();
        _cone.SetActive(false);
        _cone.transform.parent = transform;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.MoveSpeed;

    }

    public override bool IsVisible()
    {
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

            Destroy(_cone);
            Destroy(gameObject);
        }
    }


    public override void LoopUpdate(float deltaTime)
    {
        RemoveFromLockedTargetIfNotVisible();

        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

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
                

                _renderer.material.color = Color.red;
                Attack();
            }
            else
            {
                _renderer.material.color = Color.yellow;
            }
                MoveTowardsThePlayer();
            
        }
        else
        {
            _renderer.material.color = Color.yellow;
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    void drawCone(int points)
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        //pointsForTheCone[0] = transform.position;

        Vector3 vectorToRotate = transform.forward * stats.AttackRange;
        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points - 1);
        int step = 0;

        for (int i = 0; i < points; i++)
        {
            float angle = Mathf.Lerp(0, 360, step * stepSize);



            angle = angle * Mathf.Deg2Rad;

            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            rotatedVector.x = vectorToRotate.x * c - vectorToRotate.z * s;
            rotatedVector.z = vectorToRotate.x * s + vectorToRotate.z * c;

            pointsForTheCone[i] = transform.position + rotatedVector;
            step++;
        }

        _coneRenderer.SetPositions(pointsForTheCone);
        _coneRenderer.widthMultiplier = 0.1f;

        _coneRenderer.loop = true;
    }
    void Attack()
    {
        if (_attacking == false)
        {
            _navMeshAgent.speed = stats.ChargeMoveSpeed;
            AttackChargingEvent.Raise();

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            _cone.SetActive(true);
            
        }
        drawCone(20);
        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {
            AttackEvent.Raise();
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));

            RaycastHit hit;
            if (potentialTargets.Length > 0 && Physics.Raycast(transform.position, potentialTargets[0].transform.position - transform.position, out hit,10, LayerMask.GetMask("Player")))
            {
                
                if (hit.collider.gameObject.layer == 9)
                {

                    //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                    //if()
                    Vector3 temp = potentialTargets[0].transform.position;
                    temp.y = transform.position.y;


                    
                        PlayerHealth playerHealth = potentialTargets[0].GetComponent<PlayerHealth>();
                        //apply damage to the player
                        if (playerHealth != null)
                        {
                            

                            playerHealth.TakeDamage(stats.AttackDamage);
                        }
                        else
                        {
                            Debug.LogError("target of " + gameObject.name + " attack got no health");
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
            _cone.SetActive(false);
            _navMeshAgent.speed = stats.MoveSpeed;
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
        float distanceToplayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_playerTransform.position.x, _playerTransform.position.z));

        if (distanceToplayer > 2)
        {

            if (_navMeshAgent.destination != _playerTransform.position)
                _navMeshAgent.destination = _playerTransform.position;
        }
        else
        {
            if (_navMeshAgent.destination != transform.position)
                _navMeshAgent.destination = transform.position;
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

            

            
        }
    }

    


}
