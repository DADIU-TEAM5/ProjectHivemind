using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boomer : Enemy
{

    
    public Material AttackMaterial;

    public GameObject Graphics;

    public GameObject bodyPart;

    BoomerStats _boomerStats;

    
    private bool _attacking;
    private float _attackCharge;
    private float _attackCooldown = 0;

    

    private NavMeshAgent _navMeshAgent;

    

    Color _startColor;

    [Header("Events")]
    public GameEvent TakeDamageEvent;
    
    public GameEvent AttackEvent;
    public GameEvent DeathEvent;
    public GameEvent AttackChargingEvent;

    Color SetColor(Color color)
    {
        return Color.Lerp(_startColor, color, 0.5f);
    }

    public void Start()
    {
        _boomerStats = (BoomerStats)stats;

        _currentHealth = _boomerStats.HitPoints;
        Renderer = Graphics.GetComponent<Renderer>();

        
        Initialize(_currentHealth);
        CreateCone();
        CreateOutline();

        ConeRenderer.material = AttackMaterial;
        OutlineRenderer.material = AttackMaterial;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _boomerStats.MoveSpeed;


        OutlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);

        _startColor = Renderer.material.color;

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



    public override bool IsVisible()
    {
        if(Renderer == null)
        {
            Renderer = Graphics.GetComponent<Renderer>();
        }

        return Renderer.isVisible;
    }

    public override void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);
        _currentHealth -= damage;
        UpdateHealthBar(_currentHealth);
        TakeDamageEvent.Raise(gameObject);

        if (_currentHealth <= 0)
        {
            int partsToDrop = Random.Range(_boomerStats.minPartsToDrop, _boomerStats.maxPartsToDrop);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);

                part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
            }

            DeathEvent.Raise(gameObject);
            EnemyList.Remove(gameObject);

            Destroy(Cone);
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
            Renderer.material.color = SetColor( Color.blue);
            DetectThePlayer();
        }
        else if (playerInAttackRange() || _attacking)
        {
            if (_attackCooldown <= 0)
            {
                

                Renderer.material.color = SetColor(Color.red);
                Attack();
            }
            else
            {
                Renderer.material.color = SetColor(Color.yellow);
            }
                MoveTowardsThePlayer();
            
        }
        else
        {
            Renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) { }

    void Attack()
    {
        if (_attacking == false)
        {
            _navMeshAgent.speed = _boomerStats.ChargeMoveSpeed;
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            Cone.SetActive(true);
            Outline.SetActive(true);
            DrawCone(20, OutlineMesh, true,_attackCharge);

        }

        ConeRenderer.material.color = Color.Lerp(Color.green, Color.red, _attackCharge / _boomerStats.AttackChargeUpTime);

        DrawCone(20,ConeMesh,false, _attackCharge);
        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= _boomerStats.AttackChargeUpTime)
        {
            AttackEvent.Raise(gameObject);
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, _boomerStats.AttackRange, LayerMask.GetMask("Player"));

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
                            

                            playerHealth.TakeDamage(_boomerStats.AttackDamage);
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

            _attackCooldown = _boomerStats.AttackSpeed;
            _attacking = false;
            _attackCharge = 0;
            Cone.SetActive(false);
            Outline.SetActive(false);
            _navMeshAgent.speed = _boomerStats.MoveSpeed;
        }

    }



    bool playerInAttackRange()
    {
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < _boomerStats.AttackRange *0.8f;
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

    

    


}
