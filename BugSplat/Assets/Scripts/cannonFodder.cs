using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cannonFodder : Enemy
{

    public AnimationCurve AttackCurve;

    public Material ConeMaterial;

    public GameObject Graphics;

    public GameObject bodyPart;

    public GameObject DeadFodder;

    public SimpleEnemyStats stats;

    private bool _playerDetected;
    private bool _isAlly;
    private Transform _playerTransform;
    private bool _attacking;
    private float _attackCharge;
    private float _attackCooldown = 0;

    private float _currentHealth;

    private NavMeshAgent _navMeshAgent;

    private Renderer _renderer;
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
    public GameEvent AttackChargingEvent;

    public void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.MoveSpeed;

        _currentHealth = stats.HitPoints;
        _renderer = Graphics.GetComponent<Renderer>();

        
        Initialize(_currentHealth);
        CreateCone();
        CreateOutline();
        _coneRenderer.material = ConeMaterial;
        _outlineRenderer.material = ConeMaterial;

        _outlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);
        

        _startColor = _renderer.material.color;

    }

    public override bool IsVisible()
    {
        if (_renderer == null)
        {
            _renderer = Graphics.GetComponent<Renderer>();
        }

        return _renderer.isVisible;
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

            Destroy(_cone);
            Destroy(_outline);
            Destroy(gameObject, 3f);
        }
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
            _renderer.material.color = SetColor(Color.blue);
            DetectThePlayer();
        }
        else if ( playerInAttackRange() || _attacking)
        {
            if (_attackCooldown <= 0)
            {
                if (_navMeshAgent.destination != transform.position)
                    _navMeshAgent.destination = transform.position;

                _renderer.material.color = SetColor(Color.red);
                Attack();
            }
            else
            {
                _renderer.material.color = SetColor(Color.yellow);
                MoveTowardsThePlayer();
            }
        }
        else
        {
            _renderer.material.color = SetColor(Color.yellow);
            MoveTowardsThePlayer();
        }
    }

    public override void LoopLateUpdate(float deltaTime) {}


    int[] _triangles = { };
    Vector3[] _normals= { };


    void drawCone(int points, Mesh mesh,bool constant)
    {
        if(_triangles.Length != points)
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

        if(_normals.Length != points)
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
        if (constant)
            vectorToRotate = Vector3.forward * stats.AttackRange;
        else
            vectorToRotate = Vector3.forward * (stats.AttackRange * AttackCurve.Evaluate(_attackCharge / stats.AttackChargeUpTime));

        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f/((float)points-1);
        int step = 0;

        

        for (int i = 1; i < points; i++)
        {
            float angle = Mathf.Lerp(-stats.AttackAngle, stats.AttackAngle, step*stepSize);

            

            angle = angle * Mathf.Deg2Rad;

            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            rotatedVector.x = vectorToRotate.x * c - vectorToRotate.z * s;
            rotatedVector.z = vectorToRotate.x * s + vectorToRotate.z * c;

            vertices[i] =  rotatedVector;
            step++;
        }

        mesh.vertices = vertices;

        if(mesh.triangles != _triangles)
            mesh.triangles = _triangles;

        if (mesh.normals != _normals)
            mesh.normals = _normals;


        

        
    }
    void Attack()
    {
       


        if (_attacking == false)
        {
            AttackChargingEvent.Raise(this.gameObject);

            Vector3 adjustedPlayerPos = _playerTransform.position;

            adjustedPlayerPos.y = transform.position.y;

            transform.LookAt(adjustedPlayerPos);

            _cone.SetActive(true);
            _outline.SetActive(true);

            drawCone(10, _outlineMesh,true);

        }
        drawCone(10,_coneMesh,false);

        _coneRenderer.material.color = Color.Lerp(new Color(0,1,0,0.5f), new Color(1, 0, 0, 0.5f), _attackCharge / stats.AttackChargeUpTime);


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

                    //print(Vector3.Angle(transform.position + transform.forward, potentialTargets[i].transform.position - transform.position));
                    //if()
                    Vector3 temp = potentialTargets[0].transform.position;
                    temp.y = transform.position.y;


                    //print( Vector3.Angle(transform.position - (transform.position + transform.forward), transform.position - temp));
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
            _cone.SetActive(false);
            _outline.SetActive(false);

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

                    DetectAllies();
                }
            }
        } 
    }

    void DetectAllies()
    {
        Collider[] potentialAllies = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Enemy"));

        if(potentialAllies.Length > 0)
        {
            for (int i = 0; i < potentialAllies.Length; i++)
            {
                cannonFodder allyTransform = potentialAllies[i].gameObject.GetComponent<cannonFodder>();
                if (!allyTransform?._isAlly ?? false)
                {
                    allyTransform._playerDetected = true;
                    allyTransform._playerTransform = _playerTransform;
                    allyTransform._isAlly = true;
                    allyTransform.DetectAllies();
                }
            }
        }
    }

}
