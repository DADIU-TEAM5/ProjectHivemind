using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankBeetle : Enemy

{
    

    public Material ConeMaterial;
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

    

    private GameObject _cone;
    private MeshRenderer _coneRenderer;
    private Mesh _coneMesh;
    private GameObject _outline;
    private MeshRenderer _outlineRenderer;
    private Mesh _outlineMesh;

    Color _startColor;

    [Header("Events")]
    public GameEvent TakeDamageEvent;
    public GameEvent AggroEvent;
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
        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();

        _cone = new GameObject();

        _navMeshAgent = GetComponent<NavMeshAgent>();


        CreateCone();
        CreateOutline();
        _coneRenderer.material = ConeMaterial;
        _outlineRenderer.material = ConeMaterial;

        _outlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);

        _startColor = _renderer.material.color;

        _navMeshAgent.speed = stats.MoveSpeed;

        _coneRenderer.material.color = Color.red;

    }

    void CreateCone()
    {
        _cone = new GameObject();
        _cone.name = "cone";
        _coneMesh = _cone.AddComponent<MeshFilter>().mesh;
        _coneRenderer = _cone.AddComponent<MeshRenderer>();

        Vector3 offset = transform.position;

        offset.y = 0.005f;
        _cone.transform.position = offset;

        _cone.transform.rotation = transform.rotation;


        _cone.transform.parent = transform;
        _cone.SetActive(false);



    }
    void CreateOutline()
    {
        _outline = new GameObject();
        _outline.name = "outline";
        _outlineMesh = _outline.AddComponent<MeshFilter>().mesh;
        _outlineRenderer = _outline.AddComponent<MeshRenderer>();



        //_outlineRenderer.material.color = new Color(.01f, .01f, .01f, .01f);

        Vector3 offset = transform.position;

        offset.y = 0;
        _outline.transform.position = offset;

        _outline.transform.rotation = transform.rotation;

        _outline.transform.parent = transform;

        _outline.SetActive(false);


    }

    public override void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);

        TakeDamageEvent.Raise(gameObject);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DeathEvent.Raise(gameObject);
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
            _renderer.material.color = SetColor( Color.blue);
            DetectThePlayer();
        }
        else
        {
            if (_cone.activeSelf == false)
                _cone.SetActive(true);

            MoveTowardsThePlayer(deltaTime);
            _renderer.material.color = SetColor(Color.red);


            
                
            Attack();
            
        }
        



    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }


    int[] _triangles = { };
    Vector3[] _normals = { };


    void drawCone(int points, Mesh mesh)
    {
        if (_triangles.Length != points)
        {
            _triangles = new int[points * 3 + 3];

            int triangleIndex = 0;

            for (int i = 0; i < points; i++)
            {
                if (i != points - 1)
                {



                    _triangles[triangleIndex] = 0;

                    _triangles[triangleIndex + 2] = i;
                    _triangles[triangleIndex + 1] = i + 1;



                }

                triangleIndex += 3;
            }

            _triangles[triangleIndex] = 0;

            _triangles[triangleIndex + 2] = points - 1;
            _triangles[triangleIndex + 1] = 1;

        }

        if (_normals.Length != points)
        {

            _normals = new Vector3[points];

            for (int i = 0; i < points; i++)
            {
                _normals[i] = Vector3.up;
            }
        }




        Vector3[] vertices = new Vector3[points];






        vertices[0] = Vector3.zero;


        Vector3 vectorToRotate;
        
            vectorToRotate = Vector3.forward * stats.AttackRange;
        

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

            vertices[i] = rotatedVector;
            step++;
        }

        mesh.vertices = vertices;

        if (mesh.triangles != _triangles)
            mesh.triangles = _triangles;

        if (mesh.normals != _normals)
            mesh.normals = _normals;





    }
    void Attack()
    {

        drawCone(10,_coneMesh);
        _coneRenderer.material.color = new Color(1, 0, 0, .4f);

        

        

        

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

                            AttackEvent.Raise(gameObject);
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
        if (Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp) < stats.AttackAngle)
        {
            
            _navMeshAgent.Move(transform.forward * deltaTime * stats.ChargeSpeed);
        }
        else
        {
            _navMeshAgent.Move(transform.forward * deltaTime * stats.MoveSpeed);
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

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, stats.TurnSpeed * deltaTime);

        //_navMeshAgent.SetPath(pathToPlayer);
        
        
        /*if (_navMeshAgent.destination != _playerTransform.position)
            _navMeshAgent.destination = _playerTransform.position;
            */

    }

    void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Player"));
        RaycastHit hit;

        if (potentialTargets.Length > 0)
        {
            if (Physics.Raycast(transform.position, potentialTargets[0].transform.position - transform.position, out hit, 10))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    AggroEvent.Raise(this.gameObject);
                    _playerDetected = true;
                    _playerTransform = potentialTargets[0].gameObject.transform;

                    _isAlly = true;

                    
                }
            }
        }
    }

   

}
