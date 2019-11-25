using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cannonFodder : Enemy
{
    private bool _attacking;
    private float _attackCharge;
    private float _attackCooldown = 0;

    [Header("Events")]
    public GameEvent AttackEvent;
    public GameEvent AttackChargingEvent;

    public Animator FodderAnimator;
    public AnimationClip ChargeClip;
    float _percentIncrease;


    private void Start()
    {
        OutlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);

        float Increase = ChargeClip.length - stats.AttackChargeUpTime;

        float percenIncrease = Increase / stats.AttackChargeUpTime;
        _percentIncrease = percenIncrease;
    }



    public override void TakeDamageBehaviour(float damage)
    {
    }
    public override void LoopBehaviour(float deltaTime)
    {
        RemoveFromLockedTargetIfNotVisible();

        if (_attackCooldown > 0)
            _attackCooldown -= Time.deltaTime;

        Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
        {
            //Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if ( playerInAttackRange() || _attacking)
        {
            if (_attackCooldown <= 0)
            {
                if (NavMeshAgent.destination != transform.position)
                    NavMeshAgent.destination = transform.position;

                //Renderer.material.color = SetColor(Color.red);
                Attack();
            }
            else
            {
               // Renderer.material.color = SetColor(Color.yellow);
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

            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            Cone.SetActive(true);
            Outline.SetActive(true);
            OuterEdge.SetActive(true);

            FodderAnimator.SetTrigger("Attack");
            FodderAnimator.speed = 1 + _percentIncrease;

            DrawCone(4, OutlineMesh, true,_attackCharge);
            DrawCone(4, OuterEdgeMesh, true, _attackCharge);

        }
        DrawCone(4,ConeMesh,false,_attackCharge);

        ConeRenderer.material.color = Color.Lerp(ConeInitColor, ConeEndColor, _attackCharge / stats.AttackChargeUpTime);
        //Color.Lerp(new Color(0,1,0,0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);


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
            OuterEdge.SetActive(false);
            FodderAnimator.speed = 1 ;

        }
        
    }

    

    

    void MoveTowardsThePlayer()
    {
        

        float distanceToplayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PlayerTransform.position.x, PlayerTransform.position.z));

        if (distanceToplayer > stats.AttackRange/2)
        {
            FodderAnimator.SetBool("Walking", true);

            if (NavMeshAgent.destination != PlayerTransform.position)
                NavMeshAgent.destination = PlayerTransform.position;
        }
        else
        {

            FodderAnimator.SetBool("Walking", false);

            if (NavMeshAgent.destination != transform.position)
                NavMeshAgent.destination = transform.position;
        }
    }

    

}
