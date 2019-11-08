using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boomer : Enemy
{

    
    

    

    BoomerStats _boomerStats;

    
    private bool _attacking;
    private float _attackCharge;
    private float _attackCooldown = 0;

    

    

    

    

    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    
    public GameEvent AttackChargingEvent;

    

    public void Start()
    {
        _boomerStats = (BoomerStats)stats;

        

        
        


        OutlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);

        

        

    }

    

    



    

    


    public override void LoopUpdate(float deltaTime)
    {
        RemoveFromLockedTargetIfNotVisible();

        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
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
            NavMeshAgent.speed = _boomerStats.ChargeMoveSpeed;
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = PlayerTransform.position;

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
            NavMeshAgent.speed = _boomerStats.MoveSpeed;
        }

    }



    

    void MoveTowardsThePlayer()
    {
        float distanceToplayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PlayerTransform.position.x, PlayerTransform.position.z));

        if (distanceToplayer > 2)
        {

            if (NavMeshAgent.destination != PlayerTransform.position)
                NavMeshAgent.destination = PlayerTransform.position;
        }
        else
        {
            if (NavMeshAgent.destination != transform.position)
                NavMeshAgent.destination = transform.position;
        }
    }

    

    


}
