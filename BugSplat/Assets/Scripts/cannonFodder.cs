using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cannonFodder : Enemy
{

   

    

    public GameObject Graphics;

    public GameObject bodyPart;

    public GameObject DeadFodder;

    

    
    
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

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.MoveSpeed;

        
        Renderer = Graphics.GetComponent<Renderer>();

        _currentHealth = stats.HitPoints;

        Initialize(_currentHealth);





        SetupVars();

        _startColor = Renderer.material.color;

    }

    public override void SetupVars()
    {
        
    }

    public override bool IsVisible()
    {
        if (Renderer == null)
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
        TakeDamageEvent.Raise(this.gameObject); 

        if(_currentHealth <= 0)
        {
            /*
            int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
            for (int i = 0; i < partsToDrop; i++)
            {
                GameObject part = Instantiate(bodyPart);
                
                part.transform.position = transform.position +(( Vector3.up*i)*0.5f);
            }
            */

            //Graphics.SetActive(false);
            DeadFodder.transform.SetParent(null);
            DeadFodder.SetActive(true);

            DeathEvent.Raise(this.gameObject);
            EnemyList.Remove(gameObject);

            Destroy(Cone);
            Destroy(Outline);
            Destroy(gameObject, 3f);
        }
    }

    

    Color SetColor(Color color)
    {
        return Color.Lerp(_startColor, color, 0.5f);
    }

    public override void LoopUpdate(float deltaTime)
    {
        RemoveFromLockedTargetIfNotVisible();

        if (_attackCooldown > 0)
            _attackCooldown -= Time.deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!_playerDetected)
        {
            Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if ( playerInAttackRange() || _attacking)
        {
            if (_attackCooldown <= 0)
            {
                if (_navMeshAgent.destination != transform.position)
                    _navMeshAgent.destination = transform.position;

                Renderer.material.color = SetColor(Color.red);
                Attack();
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

    public override void LoopLateUpdate(float deltaTime) {}


    
    void Attack()
    {
       


        if (_attacking == false)
        {
            AttackChargingEvent.Raise(this.gameObject);

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            Cone.SetActive(true);
            Outline.SetActive(true);

            DrawCone(10, OutlineMesh, true,_attackCharge);

        }
        DrawCone(10,ConeMesh,false,_attackCharge);

        ConeRenderer.material.color = Color.Lerp(new Color(0,1,0,0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);


        _attacking = true;
        _attackCharge += Time.deltaTime;

        if (_attackCharge >= stats.AttackChargeUpTime)
        {
            AttackEvent.Raise(this.gameObject);
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.AttackRange, LayerMask.GetMask("Player"));

            RaycastHit hit;
            if (potentialTargets.Length>0 && Physics.Raycast(transform.position, potentialTargets[0].transform.position - transform.position, out hit, 10))
            {
                if (hit.collider.gameObject.layer == 9)
                {

                    
                    Vector3 temp = potentialTargets[0].transform.position;
                    temp.y = transform.position.y;


                    
                    if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
                    {
                        PlayerHealth playerHealth = potentialTargets[0].GetComponent<PlayerHealth>();
                        //apply damage to the player
                        if (playerHealth != null)
                        {
                            Vector3 directionToPush = potentialTargets[0].gameObject.transform.position - transform.position;
                            directionToPush.y = 0;
                            directionToPush = Vector3.Normalize(directionToPush);

                            playerHealth.TakeDamage( stats.AttackDamage);
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
            Cone.SetActive(false);
            Outline.SetActive(false);

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
        _navMeshAgent.Move(new Vector3(Random.Range(-jitter, jitter), 0, Random.Range(-jitter, jitter))*Time.deltaTime);

        float distanceToplayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_playerTransform.position.x, _playerTransform.position.z));

        if (distanceToplayer > stats.AttackRange/2)
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
