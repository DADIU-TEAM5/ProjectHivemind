using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cannonFodder : Enemy
{
    public GameObject Graphics;

    public GameObject bodyPart;

    bool _playerDetected;
    bool _isAlly;
    public SimpleEnemyStats stats;
    Transform _playerTransform;
    bool _attacking;
    float _attackCharge;

    float _currentHealth;

    float _attackCooldown = 0;


    Renderer _renderer;

    NavMeshAgent _navMeshAgent;

    GameObject _cone;
    LineRenderer _coneRenderer;

    public void Start()
    {
        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();

        _cone = new GameObject();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _cone.AddComponent<LineRenderer>();
        _coneRenderer = _cone.GetComponent<LineRenderer>();

        _cone.SetActive(false);

        _navMeshAgent.speed = stats.MoveSpeed;

    }

    public override void TakeDamage(float damage)
    {
       // print(name + " took damage "+ damage);
        _currentHealth -= damage;
        if(_currentHealth<= 0)
        {
            int partsToDrop = Random.Range(0, 4);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);

                
                part.transform.position = transform.position + (Vector3.up/(partsToDrop*i+1));
            }
            



            Destroy(_cone);

            Destroy(gameObject);

        }
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
        else if ( playerInAttackRange() || _attacking)
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

    public override void LoopLateUpdate(float deltaTime)
    {

    }


    void drawCone(int points )
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        pointsForTheCone[0] = transform.position;

        Vector3 vectorToRotate = transform.forward * stats.AttackRange;
        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f/((float)points-1);
        int step = 0;

        for (int i = 1; i < points; i++)
        {
            float angle = Mathf.Lerp(-stats.AttackAngle, stats.AttackAngle, step*stepSize);

            

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
            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            _cone.SetActive(true);
            drawCone(10);

        }

        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));

            for (int i = 0; i < potentialTargets.Length; i++)
            {

                //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                //if()
                Vector3 temp = potentialTargets[i].transform.position;
                temp.y = transform.position.y;


                //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
                if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
                {
                    //apply damage to the player
                    if (potentialTargets[i].GetComponent<PlayerHealth>() != null)
                    {

                        potentialTargets[i].GetComponent<PlayerHealth>().TakeDamage(stats.AttackDamage);
                    }
                    else
                    {
                        print("target got no health");
                    }
                }




            }
            _attackCooldown = stats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            _cone.SetActive(false);
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
        /*
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        transform.LookAt(adjustedPlayerPos);

        transform.Translate(Vector3.forward * stats.MoveSpeed * Time.deltaTime);
        */

        float jitter = 2;
        _navMeshAgent.Move(new Vector3(Random.Range(-jitter, jitter), 0, Random.Range(-jitter, jitter))*Time.deltaTime);

        if(_navMeshAgent.destination != _playerTransform.position)
        _navMeshAgent.destination = _playerTransform.position;

    }

    void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Player"));

        if (potentialTargets.Length > 0)
        {
            _playerDetected = true;
            _playerTransform = potentialTargets[0].gameObject.transform;

            _isAlly = true;

            DetectAllies();
        }
    }

    void DetectAllies()
    {
        Collider[] potentialAllies = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Enemy"));

        if(potentialAllies.Length > 0)
        {
            for (int i = 0; i < potentialAllies.Length; i++)
            {
                cannonFodder allyTransform = potentialAllies[i].gameObject.transform.GetComponent<cannonFodder>();
                if (!allyTransform._isAlly)
                {
                    allyTransform._playerDetected = true;
                    allyTransform._playerTransform = _playerTransform;
                    allyTransform._isAlly = true;
                    allyTransform.DetectAllies();
                }
            }
        }
    }

    
}
