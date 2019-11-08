using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{
    
    

   
    
    TankStats TankStats;
   
    bool _attacking;
    float _attackCharge;

    

    float _attackCooldown = 0;


    

    

    

    

    

    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    


    public void Start()
    {
        TankStats = (TankStats)stats;
        ConeRenderer.material.color = Color.red;

    }

    

    

    


    public override void LoopUpdate(float deltaTime)
    {

        RemoveFromLockedTargetIfNotVisible();

        //Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);

        if (!PlayerDetected)
        {
            Renderer.material.color = SetColor( Color.blue);
            DetectThePlayer();
        }
        else
        {
            if (Cone.activeSelf == false)
                Cone.SetActive(true);

            MoveTowardsThePlayer(deltaTime);
            Renderer.material.color = SetColor(Color.red);


            
                
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

    

    void MoveTowardsThePlayer(float deltaTime)
    {
        /*
        Vector3 adjustedPlayerPos = _playerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        transform.LookAt(adjustedPlayerPos);

        transform.Translate(Vector3.forward * stats.MoveSpeed * Time.deltaTime);
        */


        Vector3 temp = PlayerTransform.position;
        temp.y = transform.position.y;


        //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
        if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < TankStats.AttackAngle)
        {
            
            NavMeshAgent.Move(transform.forward * deltaTime * TankStats.ChargeSpeed);
        }
        else
        {
            NavMeshAgent.Move(transform.forward * deltaTime * TankStats.MoveSpeed);
        }




            Vector3 temPlayerPos = PlayerTransform.position;
        temPlayerPos.y = transform.position.y;

        NavMeshPath pathToPlayer = new NavMeshPath();
        NavMeshAgent.CalculatePath(PlayerTransform.position, pathToPlayer);

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
