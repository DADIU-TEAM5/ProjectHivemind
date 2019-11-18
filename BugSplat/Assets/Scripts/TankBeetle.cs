using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{



    bool NeedNewWayPoint = true;
    
    TankStats TankStats;
   
    bool _attacking;
    float _attackCharge;

    int _WayPointsCleared =  -1;

    float _coolDown;


    float _attackCooldown = 0;



    Vector3 _currentWayPoint;



    bool _Chargeing;

    float _chargeDuration;



    Vector3 _playerAttackPos;

    Vector3 _chargeEndpos;


    public Animator Anim;

    IEnumerator _failSafe;


    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    public GameEvent AttackChargingEvent;




    public void Start()
    {
        TankStats = (TankStats)stats;
        ConeRenderer.material.color = Color.red;

        _currentWayPoint = Vector3.zero;

        _failSafe = SetNeedNewWayPointToTrue();

    }

    

    

    


    public override void LoopBehaviour(float deltaTime)
    {

       


        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        

        RemoveFromLockedTargetIfNotVisible();

        //Debug.DrawLine(transform.position, (transform.position + transform.forward), Color.red);
        
            if (!PlayerDetected)
            {

            if (NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.ResetPath();
            }
            //Renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
            }
            else if( _WayPointsCleared < TankStats.Repeat)
            {

                MoveToWayPoint(deltaTime);


            }
            else if (playerInChargeRange() || _attacking)
            {
            BehaviourCooldown(deltaTime);


            if (NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.ResetPath();
            }


            if (_attackCooldown <= 0)
                {

                  //  Renderer.material.color = SetColor(Color.red);
                    Attack(deltaTime);


                }
                else
                {
                  //  Renderer.material.color = SetColor(Color.yellow);
                    MoveTowardsThePlayer(deltaTime);
                }

            }
            else
            {
            if (NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.ResetPath();
            }
            //  Renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer(deltaTime);
            BehaviourCooldown(deltaTime);


        }


      



    }

    void BehaviourCooldown(float deltaTime)
    {
        if (_coolDown <= 0 && !_Chargeing)
        {
            _WayPointsCleared = -1;
            NeedNewWayPoint = true;

        }

        if (_coolDown > 0)
            _coolDown -= deltaTime;
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }

    float _chargeDistance;

    float DistanceToChargeEndPos()
    {
      return   Vector3.Distance(transform.position, _chargeEndpos);
    }

    public override void TakeDamageBehaviour(float damage)
    {
    }

    void Attack(float deltaTime)
    {

        if (_attacking == false)
        {
            AttackChargingEvent.Raise(gameObject);

            Vector3 adjustedPlayerPos = PlayerTransform.position;

            adjustedPlayerPos.y = transform.position.y;
            transform.LookAt(adjustedPlayerPos);


            _chargeEndpos = transform.position+( transform.forward * TankStats.ChargeDistance);



            _playerAttackPos = PlayerTransform.position;




            
            Outline.SetActive(true);
            Cone.SetActive(true);

            _attacking = true;

            
        }
        


        _attackCharge += Time.deltaTime;


        if (_attackCharge >= stats.AttackChargeUpTime)
        {
            _Chargeing = true;
        }
        else
        {
            NavMeshAgent.Move((transform.forward * -1* deltaTime) * (TankStats.MoveBackDistance / TankStats.AttackChargeUpTime));
        }
        


        if (_Chargeing)
        {
            if (Anim.GetBool("Attacking") == false)
            {
                
                Anim.SetBool("Attacking", true);

            }


            Outline.transform.parent = null;
            

            _chargeDuration += deltaTime;


            if (_chargeDuration<= _chargeDistance / TankStats.ChargeSpeed)
            {
                NavMeshAgent.Move(transform.forward * deltaTime * TankStats.ChargeSpeed);

                
            }
            else
            {
                EndCharge();
            }

            RaycastHit[] ColliderHits;

            var layer1  = 9;
            var layer2 = 8;
            var layermask1 = 1 << layer1;
            var layermask2 = 1 << layer2;
            var finalmask = ~((1 << layer1) | (1 << layer2));

            




            //ColliderHits = Physics.SphereCastAll(transform.position+Vector3.up,1, transform.forward,  1, finalmask);
            ColliderHits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, 1, finalmask);


            for (int i = 0; i < ColliderHits.Length; i++)
            {

                if(ColliderHits[i].collider.gameObject.layer != 8 || ColliderHits[i].collider.gameObject.layer != 9)
                {
                    print(ColliderHits[i].collider.name);
                    EndCharge();

                    Anim.SetBool("Idle", true);


                    Stun( TankStats.StunDuration);
                    return;
                }
                
            }

            AttackInCone();
        }
        else
        {
            DrawChargeTrajectory();
            ChargeFillup();
            ConeRenderer.material.color = Color.Lerp(new Color(0, 1, 0, 0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);
            _chargeDistance = DistanceToChargeEndPos()-TankStats.AttackRange;
        }



    }


    void AttackInCone()
    {
        DrawCone(10, ConeMesh, true, 0);
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
                            playerHealth.StunPlayer(TankStats.PlayerStunDuration);

                            EndCharge();
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
                        playerHealth.StunPlayer(TankStats.PlayerStunDuration);
                        EndCharge();
                    }
                    else
                    {
                        Debug.LogError("target of " + gameObject.name + " attack got no health");
                    }
                }

            }

        }
    }

    Vector3 oldDestination = Vector3.zero;

    void MoveToWayPoint(float deltaTime)
    {

        //_coolDown = TankStats.CooldownBetweenBehaviour;

        EndCharge();

        if (Anim.GetBool("Attacking") == false)
        {

            Anim.SetBool("Attacking", true);

        }

        Cone.SetActive(true);
        AttackInCone();


        



        NavMeshAgent.speed = TankStats.WayPointSpeed;
        NavMeshAgent.angularSpeed = 100*TankStats.TurnSpeed;

        
        

        if (NeedNewWayPoint)
        {
            Vector3 temPlayerPos = PlayerTransform.position;
            temPlayerPos.y = transform.position.y;



           // _WayPointsCleared++;

            print("waypoiints cleared "+_WayPointsCleared);


            _currentWayPoint = temPlayerPos;

            NavMeshAgent.SetDestination(_currentWayPoint);
            NeedNewWayPoint = false;
        }



        if(NavMeshAgent.path.corners[NavMeshAgent.path.corners.Length-1] != oldDestination)
        {
            _WayPointsCleared++;
            oldDestination = NavMeshAgent.path.corners[NavMeshAgent.path.corners.Length - 1];

           
            //print("way points cleared " + _WayPointsCleared);
        }


        if (NavMeshAgent.remainingDistance <= 0 )
        {


            NeedNewWayPoint = true;
        }




        if (_WayPointsCleared == TankStats.Repeat - 1)
        {
            EndWayPointing();
        }




    }

    void EndWayPointing()
    {
        Cone.SetActive(false);
        Anim.SetBool("Attacking", false);
        _coolDown = TankStats.CooldownBetweenBehaviour;

        NavMeshAgent.speed = 0;
        NavMeshAgent.angularSpeed = 0;
        NeedNewWayPoint = true;


        NavMeshAgent.ResetPath();
    }

    IEnumerator SetNeedNewWayPointToTrue()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("getting new waypoint");
        NeedNewWayPoint = true;

        
        

    }

    void EndCharge()
    {

        if (Anim.GetBool("Attacking") == true)
        {
            Anim.SetBool("Attacking", false);

        }


        _attackCooldown = TankStats.AttackSpeed;
        _attacking = false;
        _attackCharge = 0;

        _Chargeing = false;
        Outline.SetActive(false);
        Cone.SetActive(false);

        _chargeDuration = 0;
        Outline.transform.parent = transform;
        Outline.transform.position = transform.position;


    }


    int[] _trianglesfortraj = { };
    Vector3[] _normalsfotraj = { };

    public void DrawChargeTrajectory()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];



            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[4];




        float trajectoryWidt = TankStats.AttackRange;

        vertices[0] = Vector3.right * trajectoryWidt;
        vertices[1] = Vector3.left * trajectoryWidt;


        vertices[2] = (Vector3.right * trajectoryWidt) + (Vector3.forward * DistanceToChargeEndPos());
        vertices[3] = (Vector3.left * trajectoryWidt) + (Vector3.forward * DistanceToChargeEndPos());





        OutlineMesh.vertices = vertices;

        if (OutlineMesh.triangles != _trianglesfortraj)
            OutlineMesh.triangles = _trianglesfortraj;

        if (OutlineMesh.normals != _normalsfotraj)
            OutlineMesh.normals = _normalsfotraj;





    }

    public void ChargeFillup()
    {
        if (_trianglesfortraj.Length != 6)
        {
            _trianglesfortraj = new int[6];



            _trianglesfortraj[0] = 0;
            _trianglesfortraj[1] = 1;
            _trianglesfortraj[2] = 2;

            _trianglesfortraj[3] = 2;
            _trianglesfortraj[4] = 1;
            _trianglesfortraj[5] = 3;



        }

        if (_normalsfotraj.Length != 4)
        {

            _normalsfotraj = new Vector3[4];

            for (int i = 0; i < 4; i++)
            {
                _normalsfotraj[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[4];




        float trajectoryWidt = TankStats.AttackRange;

        vertices[0] = Vector3.right * trajectoryWidt;
        vertices[1] = Vector3.left * trajectoryWidt;


        vertices[2] = (Vector3.right * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, DistanceToChargeEndPos(), _attackCharge / stats.AttackChargeUpTime));
        vertices[3] = (Vector3.left * trajectoryWidt) + (Vector3.forward * Mathf.Lerp(0, DistanceToChargeEndPos(), _attackCharge / stats.AttackChargeUpTime));







        ConeMesh.vertices = vertices;

        if (ConeMesh.triangles != _trianglesfortraj)
            ConeMesh.triangles = _trianglesfortraj;

        if (ConeMesh.normals != _normalsfotraj)
            ConeMesh.normals = _normalsfotraj;





    }



    void MoveTowardsThePlayer(float deltaTime)
    {

        if(Anim.GetBool("Idle") == true)
        {

            Anim.SetBool("Idle", false);

            EndWayPointing();
        }

       

        Vector3 temp = PlayerTransform.position;
        temp.y = transform.position.y;


        NavMeshAgent.Move(transform.forward * deltaTime * TankStats.MoveSpeed);



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
       
        Quaternion targetRotation = Quaternion.LookRotation(tempPathPos - transform.position );

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, TankStats.TurnSpeed * deltaTime);

        //_navMeshAgent.SetPath(pathToPlayer);
        
        
        /*if (_navMeshAgent.destination != _playerTransform.position)
            _navMeshAgent.destination = _playerTransform.position;
            */

    }


    public bool playerInChargeRange()
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - adjustedPlayerPos) < TankStats.ChargeSpotAngle)
        {
            bool theBool = false;

            theBool = Vector3.Distance(transform.position, adjustedPlayerPos) < TankStats.ChargeDistance * 1.1f;


            RaycastHit[] ColliderHits;

            var layer1 = 9;
            var layer2 = 8;
            var layermask1 = 1 << layer1;
            var layermask2 = 1 << layer2;
            var finalmask = ~((1 << layer1) | (1 << layer2));




           // ColliderHits = Physics.SphereCastAll(transform.position + Vector3.up, 1, transform.forward, 1, finalmask);
            ColliderHits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, 1, finalmask);

            for (int i = 0; i < ColliderHits.Length; i++)
            {

                if (ColliderHits[i].collider.gameObject.layer != 8 || ColliderHits[i].collider.gameObject.layer != 9)
                {
                    //print(ColliderHits[i].collider.name);

                    theBool = false;
                    //Stun(TankStats.StunDuration);
                    
                }

            }


            return theBool;

        }

        else

            return false;
    }





}
