using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{

    float _wayPointCD;


    float _lerp;
    
    Vector3 _lerpPoint;
    Vector3 _lerpStart;
    
    
    TankStats TankStats;
   
    bool _attacking;
    float _attackCharge;

    int _currentWayPath =  0;

    float _coolDown;


    float _attackCooldown = 0;

    Vector3 [] _wayPoints;




    bool _Chargeing;

    float _chargeDuration;

    float _WaySpeed;
    bool _Acellerate = true;


    Vector3 _playerAttackPos;

    Vector3 _chargeEndpos;


    public Animator Anim;



    [Header("Events")]
    
    
    public GameEvent AttackEvent;
    public GameEvent AttackChargingEvent;




    public void Start()
    {
        TankStats = (TankStats)stats;

        
        _wayPoints = new Vector3[TankStats.Repeat];

    }

    

    

    


    public override void LoopBehaviour(float deltaTime)
    {

       


        if (_attackCooldown > 0)
            _attackCooldown -= deltaTime;

        if (_coolDown > 0 && !_Chargeing)
            _coolDown -= deltaTime;



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
            else if(_coolDown <=0 && TankStats.Repeat >0)
            {

                MoveToWayPoint(deltaTime);


            }
            else if (playerInChargeRange() || _attacking)
            {
            


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
            


        }


      



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
                OuterEdge.SetActive(true);

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
            ConeRenderer.material.color = Color.Lerp(ConeInitColor, ConeEndColor, _attackCharge / stats.AttackChargeUpTime);
            _chargeDistance = DistanceToChargeEndPos()-TankStats.AttackRange;
        }



    }


    void AttackInCone()
    {
        DrawCone(10, ConeMesh, true, 0);

        
        DrawCone(10, OuterEdgeMesh, true, _attackCharge);
        ConeRenderer.material.color = ConeEmptyColor;
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


    NavMeshPath _currentPath;



    int _currentCorner = 0;
    void MoveToWayPoint(float deltaTime)
    {

        if (_wayPointCD > 0)
            _wayPointCD -= deltaTime;

        //_coolDown = TankStats.CooldownBetweenBehaviour;

        EndCharge();

        if (Anim.GetBool("Attacking") == false)
        {

            Anim.SetBool("Attacking", true);
            OuterEdge.SetActive(true);
        }

        Cone.SetActive(true);

        AttackInCone();



        //NavMeshAgent.Move(transform.forward * deltaTime * TankStats.MoveSpeed);
        //print(_wayPoints[_currentWayPath]);

        if (_wayPoints[_currentWayPath] == Vector3.zero)
        {

            //print("fuck you all");

            Vector3 temPlayerPos = PlayerTransform.position;
            temPlayerPos.y = transform.position.y;
            _wayPoints[_currentWayPath] = temPlayerPos;

            _currentPath = new NavMeshPath();
            NavMeshAgent.CalculatePath(_wayPoints[_currentWayPath], _currentPath);

            if (_currentPath.corners.Length > 1)
            {
                Vector3 newEndCorner = _currentPath.corners[_currentPath.corners.Length - 1] - _currentPath.corners[_currentPath.corners.Length - 2];

                newEndCorner = newEndCorner.normalized;
                newEndCorner *= TankStats.waypointOverShoot;

                newEndCorner = _currentPath.corners[_currentPath.corners.Length - 1] + newEndCorner;

                _currentPath.corners[_currentPath.corners.Length - 1] = newEndCorner;
                _currentCorner = 1;

            }
            else
            {
                Vector3 newEndCorner = _currentPath.corners[_currentPath.corners.Length - 1] - transform.position;

                newEndCorner = newEndCorner.normalized;
                newEndCorner *= TankStats.waypointOverShoot;

                newEndCorner = _currentPath.corners[_currentPath.corners.Length - 1] + newEndCorner;

                _currentPath.corners[_currentPath.corners.Length - 1] = newEndCorner;

                _currentCorner = 0;

            }

        }



        if(_lerpPoint != _currentPath.corners[_currentCorner])
        {

           // print("new lerp point");
            _lerpPoint = _currentPath.corners[_currentCorner];
            _lerpStart = transform.position;
            _lerp = 0;
        }

        //transform.position = Vector3.Lerp(_lerpStart, _lerpPoint, _lerp);

        NavMeshAgent.Move(-transform.position + Vector3.Lerp(_lerpStart, _lerpPoint,TankStats.ChargeCurve.Evaluate( _lerp)));

        //print(_lerp);

        if (_lerp < 1)
        {
            _lerp += (deltaTime/Vector3.Distance(_lerpStart,_lerpPoint))* TankStats.WayPointSpeed;
        }


        if (_lerp >= 1)
        {
           // print("New Way pint");
            if (_currentPath.corners.Length-1 > _currentCorner)
            {
                _currentCorner++;


                _lerpPoint = _currentPath.corners[_currentCorner];
                _lerpStart = transform.position;
                _lerp = 0;
            }
            else
            {
                _currentWayPath++;
                if (_currentWayPath == TankStats.Repeat)
                    EndWayPointing();
            }


           // _currentWayPath++;
        }


       Quaternion targetRotation = Quaternion.LookRotation(_currentPath.corners[_currentCorner] - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, TankStats.TurnSpeed * deltaTime);


        /*
        if(Vector3.Distance(transform.position, _wayPoints[_currentWayPath]) < 0.1f)
        {
            _currentWayPath++;

            if(_currentWayPath >= TankStats.Repeat)
            {
                
                EndWayPointing();
            }
        }
        */
        

    }

    void EndWayPointing()
    {
        Cone.SetActive(false);

        OuterEdge.SetActive(false);

        Anim.SetBool("Attacking", false);
        _coolDown = TankStats.CooldownBetweenBehaviour;
        _currentWayPath = 0;


        _wayPoints = new Vector3[TankStats.Repeat];
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            _wayPoints[i] = Vector3.zero;
        }

        

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
        OuterEdge.SetActive(false);
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

        OutlineMesh.RecalculateBounds();




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



        ConeMesh.RecalculateBounds();

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

    public override void SpawnFromUnderground()
    {
        EnemySpawnedEvent.Raise(this.gameObject);

        Debug.Log("SPAWN");

        if (SpawnFirstTime.Value == true)
        {
            Debug.Log("SPAWNED FIRST TIME");
            PlayerCurrentSpeedSO.Value = 0;
            SpawnCamInit.Raise(RenderGraphics);
            SpawnFirstTime.Value = false;
        }

        Anim.SetBool("Spawn", true);

    }



    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_lerpPoint, .5f);


        if(_currentPath != null)
        {
            for (int i = 0; i < _currentPath.corners.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_currentPath.corners[i], 0.3f);


            }
        }
        */
    }


}
