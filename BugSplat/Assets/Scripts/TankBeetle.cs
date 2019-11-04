using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{
    public GameObject Graphics;

    public GameObject bodyPart;

    bool _playerDetected;
    bool _isAlly;
    public TankStats stats;
    Transform _playerTransform;
    bool _attacking;
    float _attackCharge;

    float _currentHealth;

    float _attackCooldown = 0;


    Renderer _renderer;

    NavMeshAgent _navMeshAgent;

    GameObject _cone;
    LineRenderer _coneRenderer;


    [Header("Events")]
    public GameEvent TakeDamageEvent;
    public GameEvent AggroEvent;
    public GameEvent AttackEvent;
    public GameEvent DeathEvent;
    


    public override bool IsVisible()
    {
        return _renderer.isVisible;
    }

    public void Start()
    {
        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();

        _cone = new GameObject();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _cone.AddComponent<LineRenderer>();
        _coneRenderer = _cone.GetComponent<LineRenderer>();

        _cone.SetActive(false);
        _cone.transform.parent = transform;

        _navMeshAgent.speed = stats.MoveSpeed;

    }

    public override void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);

        TakeDamageEvent.Raise();
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DeathEvent.Raise();
            int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);


                part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
            }

            EnemyList.Remove(gameObject);

            Destroy(_cone);

            Destroy(gameObject);

        }
    }


    public override void LoopUpdate(float deltaTime)
    {

        RemoveFromLockedTargetIfNotVisible();

        //Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!_playerDetected)
        {
            _renderer.material.color = Color.blue;
            DetectThePlayer();
        }
        else
        {
            if (_cone.activeSelf == false)
                _cone.SetActive(true);

            MoveTowardsThePlayer();
            _renderer.material.color = Color.red;


            
                
            Attack();
            
        }
        



    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }


    void drawCone(int points)
    {
        Vector3[] pointsForTheCone = new Vector3[points];
        _coneRenderer.positionCount = points;

        pointsForTheCone[0] = transform.position;

        Vector3 vectorToRotate = transform.forward * stats.AttackRange;
        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points - 1);
        int step = 0;

        for (int i = 1; i < points; i++)
        {
            float angle = Mathf.Lerp(-stats.AttackAngle, stats.AttackAngle, step * stepSize);



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

        drawCone(10);
        

        

        

        

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));


        


            for (int i = 0; i < potentialTargets.Length; i++)
            {

            //Debug.DrawRay(transform.position, potentialTargets[i].transform.position- transform.position ,Color.red);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, potentialTargets[i].transform.position - transform.position, out hit, 10, LayerMask.GetMask("Player")))
            {
                if (hit.collider.gameObject.layer == 9)
                {

                    //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                    //if()
                    Vector3 temp = potentialTargets[i].transform.position;
                    temp.y = transform.position.y;


                    //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
                    if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
                    {
                        PlayerHealth playerHealth = potentialTargets[i].GetComponent<PlayerHealth>();
                        //apply damage to the player
                        if (playerHealth != null)
                        {
                            Vector3 directionToPush = potentialTargets[i].gameObject.transform.position - transform.position;
                            directionToPush.y = 0;
                            directionToPush = Vector3.Normalize(directionToPush);

                            AttackEvent.Raise();
                            playerHealth.KnockBackDamage(directionToPush, stats.PushLength, stats.AttackDamage);
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
            {
                Debug.LogError("this should never show i guess");
                Vector3 temp = potentialTargets[i].transform.position;
                temp.y = transform.position.y;


                //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
                if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
                {
                    PlayerHealth playerHealth = potentialTargets[i].GetComponent<PlayerHealth>();
                    //apply damage to the player
                    if (playerHealth != null)
                    {
                        Vector3 directionToPush = potentialTargets[i].gameObject.transform.position - transform.position;
                        directionToPush.y = 0;
                        directionToPush = Vector3.Normalize(directionToPush);

                        playerHealth.KnockBackDamage(directionToPush, stats.PushLength, stats.AttackDamage);
                    }
                    else
                    {
                        Debug.LogError("target of " + gameObject.name + " attack got no health");
                    }
                }

            }

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


        Vector3 temp = _playerTransform.position;
        temp.y = transform.position.y;


        //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
        if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
        {
            
            _navMeshAgent.Move(transform.forward * Time.deltaTime * stats.ChargeSpeed);
        }
        else
        {
            _navMeshAgent.Move(transform.forward * Time.deltaTime * stats.MoveSpeed);
        }




            Vector3 temPlayerPos = _playerTransform.position;
        temPlayerPos.y = transform.position.y;

        NavMeshPath pathToPlayer = new NavMeshPath();
        _navMeshAgent.CalculatePath(_playerTransform.position, pathToPlayer);

        Vector3 tempPathPos;

        if (pathToPlayer.corners.Length > 1)
            tempPathPos = pathToPlayer.corners[1];
        else
            tempPathPos = temPlayerPos;

        tempPathPos.y = transform.position.y;
        /*
        for (int i = 0; i < pathToPlayer.corners.Length; i++)
        {
            Debug.DrawLine(transform.position, pathToPlayer.corners[i], Color.red);
        }
        */
        

        //print(tempPathPos);

        Quaternion targetRotation = Quaternion.LookRotation(tempPathPos - transform.position );

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, stats.TurnSpeed * Time.deltaTime);

        //_navMeshAgent.SetPath(pathToPlayer);
        
        
        /*if (_navMeshAgent.destination != _playerTransform.position)
            _navMeshAgent.destination = _playerTransform.position;
            */

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
