using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{
    
    public GameObject Graphics;

    public GameObject bodyPart;

   
    
    TankStats TankStats;
   
    bool _attacking;
    float _attackCharge;

    

    float _attackCooldown = 0;


    Renderer _renderer;

    NavMeshAgent _navMeshAgent;

    

    

    Color _startColor;

    [Header("Events")]
    public GameEvent TakeDamageEvent;
    
    public GameEvent AttackEvent;
    public GameEvent DeathEvent;

    Color SetColor(Color color)
    {
        return Color.Lerp(_startColor, color, 0.5f);
    }

    public override bool IsVisible()
    {
        if (_renderer == null)
        {
            _renderer = Graphics.GetComponent<Renderer>();
        }
        return _renderer.isVisible;
    }

    public void Start()
    {
        TankStats = (TankStats)stats;


        _currentHealth = TankStats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();

        Cone = new GameObject();

        Initialize(_currentHealth);

        _navMeshAgent = GetComponent<NavMeshAgent>();


        

        

        _startColor = _renderer.material.color;

        _navMeshAgent.speed = TankStats.MoveSpeed;

        ConeRenderer.material.color = Color.red;

        SetupVars();

    }

    public override void SetupVars()
    {
        
    }

    void CreateCone()
    {
        Cone = new GameObject();
        Cone.name = "cone";
        ConeMesh = Cone.AddComponent<MeshFilter>().mesh;
        ConeRenderer = Cone.AddComponent<MeshRenderer>();

        Vector3 offset = transform.position;

        offset.y = 0.005f;
        Cone.transform.position = offset;

        Cone.transform.rotation = transform.rotation;


        Cone.transform.parent = transform;
        Cone.SetActive(false);



    }
    void CreateOutline()
    {
        Outline = new GameObject();
        Outline.name = "outline";
        OutlineMesh = Outline.AddComponent<MeshFilter>().mesh;
        OutlineRenderer = Outline.AddComponent<MeshRenderer>();



        //_outlineRenderer.material.color = new Color(.01f, .01f, .01f, .01f);

        Vector3 offset = transform.position;

        offset.y = 0;
        Outline.transform.position = offset;

        Outline.transform.rotation = transform.rotation;

        Outline.transform.parent = transform;

        Outline.SetActive(false);


    }

    public override void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);

        TakeDamageEvent.Raise(gameObject);
        _currentHealth -= damage;
        UpdateHealthBar(_currentHealth);
        if (_currentHealth <= 0)
        {
            DeathEvent.Raise(gameObject);
            int partsToDrop = Random.Range(TankStats.minPartsToDrop, TankStats.maxPartsToDrop);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);


                part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
            }

            EnemyList.Remove(gameObject);

            Destroy(Cone);

            Destroy(gameObject);

        }
    }


    public override void LoopUpdate(float deltaTime)
    {

        RemoveFromLockedTargetIfNotVisible();

        //Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!_playerDetected)
        {
            _renderer.material.color = SetColor( Color.blue);
            DetectThePlayer();
        }
        else
        {
            if (Cone.activeSelf == false)
                Cone.SetActive(true);

            MoveTowardsThePlayer(deltaTime);
            _renderer.material.color = SetColor(Color.red);


            
                
            Attack();
            
        }
        



    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }


    void Attack()
    {

        DrawCone(10,ConeMesh,true,0);
        ConeRenderer.material.color = new Color(1, 0, 0, .4f);

        

        

        

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, TankStats.AttackRange, LayerMask.GetMask("Player"));


        


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
                    if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < TankStats.AttackAngle)
                    {
                        PlayerHealth playerHealth = potentialTargets[i].GetComponent<PlayerHealth>();
                        //apply damage to the player
                        if (playerHealth != null)
                        {
                            Vector3 directionToPush = potentialTargets[i].gameObject.transform.position - transform.position;
                            directionToPush.y = 0;
                            directionToPush = Vector3.Normalize(directionToPush);

                            AttackEvent.Raise(gameObject);
                            playerHealth.KnockBackDamage(directionToPush, TankStats.PushLength, TankStats.AttackDamage);
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
                if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < TankStats.AttackAngle)
                {
                    PlayerHealth playerHealth = potentialTargets[i].GetComponent<PlayerHealth>();
                    //apply damage to the player
                    if (playerHealth != null)
                    {
                        Vector3 directionToPush = potentialTargets[i].gameObject.transform.position - transform.position;
                        directionToPush.y = 0;
                        directionToPush = Vector3.Normalize(directionToPush);

                        playerHealth.KnockBackDamage(directionToPush, TankStats.PushLength, TankStats.AttackDamage);
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

        return Vector3.Distance(transform.position, adjustedPlayerPos) < TankStats.AttackRange / 2;
    }

    void MoveTowardsThePlayer(float deltaTime)
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
        if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < TankStats.AttackAngle)
        {
            
            _navMeshAgent.Move(transform.forward * deltaTime * TankStats.ChargeSpeed);
        }
        else
        {
            _navMeshAgent.Move(transform.forward * deltaTime * TankStats.MoveSpeed);
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

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, TankStats.TurnSpeed * deltaTime);

        //_navMeshAgent.SetPath(pathToPlayer);
        
        
        /*if (_navMeshAgent.destination != _playerTransform.position)
            _navMeshAgent.destination = _playerTransform.position;
            */

    }

    

   

}
