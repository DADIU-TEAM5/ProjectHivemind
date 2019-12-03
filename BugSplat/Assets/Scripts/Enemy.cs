using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : GameLoop
{
    [HideInInspector]
    public bool SpawnedEnemy;
    public GameEvent EnemySpawnedEvent;
    public GameEvent SpawnCamInit;
    public BoolVariable SpawnFirstTime;
    public GameEvent ZoomCamInit;
    public IntVariable ZoomCamFrequency;
    public FloatVariable PlayerCurrentSpeedSO;

    public bool IsUnderground = true;
    
    [HideInInspector]
    public Hexagon hex;

    public int difficultyValue = 1;
    public AnimationCurve AttackCurve;

    public Material ConeMaterial;
    public Material EdgeMaterial, LowerConeMaterial;

    public EnemyStats stats;

    public GameObject bodyPart;


    public GameObject DeadCutout;

    public GameObject Graphics;
    public GameObject RenderGraphics;

    public EnemyObjectList EnemyList;
    public GameObjectVariable LockedTarget;
    public GameObjectVariable CurrentEnemySO;

    public GameObjectVariable TargetGraphic;

    public BoolVariable NoVisibleEnemies;

    // Healthbar Variables
    private GUIStyle currentStyle = null;
    public int HealthBarWidth = 150;
    public int HealthBarHeight = 20;
    public float HealthBarOffsetX = -50f;
    public float HealthBarOffsetY = -200f;
    private bool _showHealthBar;
    private float _initialHealth;
    private float _newHealthBarWidth;
    private Vector2 _currentHealthPos;
    private Color _currentHealthColor;
    private Gradient _gradient = new Gradient();
    private GradientMode _gradientMode;
    private GradientColorKey[] _colorKey;
    private GradientAlphaKey[] _alphaKey;
    private bool _updateHealth;
    private Material _enemyMat;
    private float _enemyHighlightTime = 0.3f;
    public Material WhiteMaterial;

    [HideInInspector]
    public NavMeshAgent NavMeshAgent;
    [HideInInspector]
    public Color StartColor;

    [HideInInspector]
    public Renderer Renderer;
    [HideInInspector]
    public GameObject Cone;

    [HideInInspector]
    public GameObject Edge1;
    [HideInInspector]
    public GameObject Edge2;
    [HideInInspector]
    public Mesh EdgeMesh1, EdgeMesh2;
    [HideInInspector]
    public MeshRenderer EdgeRendere1, EdgeRendere2;

    [HideInInspector]
    public GameObject OuterEdge;
    [HideInInspector]
    public Mesh OuterEdgeMesh;
    [HideInInspector]
    public MeshRenderer OuterEdgeRenderer;

    [HideInInspector]
    public MeshRenderer ConeRenderer;
    [HideInInspector]
    public Mesh ConeMesh;
    [HideInInspector]
    public GameObject Outline;
    [HideInInspector]
    public MeshRenderer OutlineRenderer;
    [HideInInspector]
    public Mesh OutlineMesh;

    public Color ConeInitColor = new Color(0f, 1f, 0f, 1f);
    public Color ConeEndColor = new Color(1f, 0f, 0f, 1f);
    public Color ConeEmptyColor = new Color(.2f, .2f, .2f, .1f);



    public GameEvent AggroEvent;
    public GameEvent DefaultAggroEvent;
    public GameEvent TakeDamageEvent;
    public GameEvent TakeDamageAnyType;
    public GameEvent DeathEvent;
    public GameEvent EnemyDied;


    [HideInInspector]
    public bool PlayerDetected;
    [HideInInspector]
    public Transform PlayerTransform;
    [HideInInspector]
    public bool IsAlly;

    [HideInInspector]
    public float _currentHealth;

    [HideInInspector]
    public float _stunTime;

    private bool _spawned = false;


    public void Stun(float time)
    {
        _stunTime = time;
    }

    private void OnEnable()
    {
        //Debug.Log(name + " spawned");


        if (EnemyList != null)
            EnemyList.Add(this);

        if (RenderGraphics == null)
        {
            Renderer = Graphics.GetComponent<Renderer>();
        }
        else
        {
            //Renderer = RenderGraphics.GetComponent<Renderer>();
            Renderer = RenderGraphics.GetComponentsInChildren<Renderer>()[0];
        }


        NavMeshAgent = GetComponent<NavMeshAgent>();

        if (NavMeshAgent != null)
            NavMeshAgent.speed = stats.MoveSpeed;

        _currentHealth = stats.HitPoints;

        Initialize(_currentHealth);

        if (ConeMaterial != null)
        {
            CreateCone();
            CreateOutline();
            CreateEdges();
            CreateOuterEdge();

            EdgeRendere1.material = EdgeMaterial;
            EdgeRendere2.material = EdgeMaterial;
            OuterEdgeRenderer.material = ConeMaterial;
            ConeRenderer.material = LowerConeMaterial;
            OutlineRenderer.material = LowerConeMaterial;

            OutlineRenderer.material.color = ConeEmptyColor;

        }

        if (NavMeshAgent != null && NavMeshAgent.isOnNavMesh)
        {
            NavMeshAgent.destination = transform.position + (new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)));
        }

        _enemyMat = this.gameObject.GetComponentInChildren<Renderer>().material;

    }


    public override void LoopUpdate(float deltaTime)
    {

        if (!IsUnderground)
        {
            if (SpawnedEnemy)
            {
                NavMeshAgent.ResetPath();
                SpawnedEnemy = false;
            }


            if (_stunTime > 0)
            {
                _stunTime -= deltaTime;
            }

            if (_stunTime <= 0)
            {
                LoopBehaviour(deltaTime);
            }
            else
            {
                //  Renderer.material.color = SetColor(Color.blue);
            }
        } else
        {
            if (!PlayerDetected)
            {
                DetectThePlayer();
                //  Renderer.material.color = SetColor(Color.blue);
            }
            else if (PlayerDetected && _spawned == false)
            {
                SpawnFromUnderground();
                _spawned = true;
            }

        }


        
        
        Edge1.SetActive(Cone.activeSelf);
        Edge2.SetActive(Cone.activeSelf);
       // OuterEdge.SetActive(Cone.activeSelf);

    }

    public abstract void LoopBehaviour(float deltaTime);

    public abstract void SpawnFromUnderground();


    public bool IsVisible()
    {
        if (Renderer == null)
        {
            Renderer = Graphics.GetComponent<Renderer>();
        }

        return Renderer.isVisible;
    }

    public void TakeDamage(float damage)
    {
        // print(name + " took damage "+ damage);


        _currentHealth -= damage;
        UpdateHealthBar(_currentHealth);
        TakeDamageEvent.Raise(this.gameObject);
        TakeDamageAnyType.Raise(this.gameObject);
        TakeDamageBehaviour(damage);


        if (_currentHealth <= 0)
        {

            /*if (Graphics.name.ToLower().Contains("fodder") == false)
            {
                if (Graphics.name.ToLower().Contains("egg") == false)
                {
                    if (ZoomCamFrequency.Value == ZoomCamFrequency.Max)
                    {
                        ZoomCamInit.Raise(DeadCutout);

                        ZoomCamFrequency.Value = 0;
                    }

                    ZoomCamFrequency.Value++;
                }
            }*/

            if (DeadCutout == null)
            {
                int partsToDrop = Random.Range(stats.minPartsToDrop, stats.maxPartsToDrop);
                for (int i = 0; i < partsToDrop; i++)
                {
                    GameObject part = Instantiate(bodyPart);

                    part.transform.position = transform.position + ((Vector3.up * i) * 0.5f);
                }

            }
            else
            {
                //Graphics.SetActive(false);
                DeadCutout.transform.SetParent(null);
                DeadCutout.SetActive(true);
            }


            DeathEvent.Raise(this.gameObject);

            EnemyDied?.Raise(gameObject);


            Destroy(Cone);
            Destroy(Outline);
            Destroy(gameObject);
            Destroy(Edge1);
            Destroy(Edge2);
            Destroy(OuterEdge);
        }
    }

    public abstract void TakeDamageBehaviour(float damage);



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

    void CreateOuterEdge()
    {
        OuterEdge = new GameObject();
        OuterEdge.name = "OuterEdge";
        OuterEdgeMesh = OuterEdge.AddComponent<MeshFilter>().mesh;
        OuterEdgeRenderer = OuterEdge.AddComponent<MeshRenderer>();

        Vector3 offset = transform.position;

        offset.y = 0.010f;
        OuterEdge.transform.position = offset;

        OuterEdge.transform.rotation = transform.rotation;


        OuterEdge.transform.parent = transform;
        OuterEdge.SetActive(false);



    }


    void CreateEdges()
    {
        Edge1 = new GameObject();
        Edge1.name = "edge1";
        EdgeMesh1 = Edge1.AddComponent<MeshFilter>().mesh;
        EdgeRendere1 = Edge1.AddComponent<MeshRenderer>();

        Vector3 offset = transform.position;

        offset.y = 0.005f;
        Edge1.transform.position = offset;

        Edge1.transform.rotation = transform.rotation;


        Edge1.transform.parent = transform;
        Edge1.SetActive(false);




        Edge2 = new GameObject();
        Edge2.name = "edge2";
        EdgeMesh2 = Edge2.AddComponent<MeshFilter>().mesh;
        EdgeRendere2 = Edge2.AddComponent<MeshRenderer>();

        
        Edge2.transform.position = offset;

        Edge2.transform.rotation = transform.rotation;


        Edge2.transform.parent = transform;
        Edge2.SetActive(false);



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



    public void Initialize(float hitPoints)
    {
        _initialHealth = hitPoints;
        _newHealthBarWidth = (HealthBarWidth / _initialHealth) * hitPoints;

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        _colorKey = new GradientColorKey[4];
        _colorKey[0].color = Color.red;
        _colorKey[0].time = 0.0f;
        _colorKey[1].color = Color.red;
        _colorKey[1].time = 0.33f;
        _colorKey[2].color = Color.yellow;
        _colorKey[2].time = 0.66f;
        _colorKey[3].color = Color.green;
        _colorKey[3].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        _alphaKey = new GradientAlphaKey[4];
        _alphaKey[0].alpha = 0.5f;
        _alphaKey[0].time = 0.0f;
        _alphaKey[1].alpha = 0.5f;
        _alphaKey[1].time = 0.33f;
        _alphaKey[2].alpha = 0.5f;
        _alphaKey[2].time = 0.66f;
        _alphaKey[3].alpha = 0.5f;
        _alphaKey[3].time = 1.0f;

        _gradient.SetKeys(_colorKey, _alphaKey);
        _gradientMode = GradientMode.Fixed;
        _gradient.mode = _gradientMode;

        _currentHealthColor = _gradient.Evaluate(1f);


        _updateHealth = true;

    }

    private void OnDisable()
    {
        if (EnemyList != null)
            EnemyList.Remove(this);

        // This is to fix the bug where the Enemy Graphics would stay even after they have died

        

        CurrentEnemySO.Value = null;
    }

    public void RemoveFromLockedTargetIfNotVisible()
    {
        if (CurrentEnemySO.Value == gameObject)
        {
            
                
            

            if (IsVisible() == false)
            {
                
                CurrentEnemySO.Value = null;
            }

            _showHealthBar = true;

        }
        else
        {
            _showHealthBar = false;
        }

        if (CurrentEnemySO.Value == null)
        {
            
        }

        /*if (LockedTarget.Value == gameObject)
        {
            if(TargetGraphic.Value == null)
            {
                TargetGraphic.Value = GameObject.CreatePrimitive(PrimitiveType.Quad);
                TargetGraphic.Value.name = "Target Graphic";
                Destroy(TargetGraphic.Value.GetComponent<MeshCollider>());
                TargetGraphic.Value.transform.rotation = Quaternion.Euler(90, 0, 0);
                TargetGraphic.Value.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                if (TargetGraphic.Value.activeSelf == false)
                    TargetGraphic.Value.SetActive(true);

                TargetGraphic.Value.transform.position = transform.position;
            }

            if (IsVisible() == false)
            {
                TargetGraphic.Value.SetActive(false);
                LockedTarget.Value = null;
            }

            _showHealthBar = true;
        }

        if (LockedTarget.Value == null)
        {
            if(TargetGraphic.Value != null)
            TargetGraphic.Value.SetActive(false);
        }

        if (CurrentEnemySO.Value != gameObject && LockedTarget.Value != gameObject)
        {
            _showHealthBar = false;
        }*/
    }

    public void UpdateHealthBar(float hitPoints)
    {
        float percOfInitialHealth = hitPoints / _initialHealth;
        _currentHealthColor = _gradient.Evaluate(percOfInitialHealth);
        _newHealthBarWidth = (HealthBarWidth / _initialHealth) * hitPoints;
        _updateHealth = true;
        _showHealthBar = true;
    }

    private void UpdateBarPos()
    {
        _currentHealthPos = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnGUI()
    {
        InitStyles();
        UpdateBarPos();

        if (_showHealthBar == true)
        {
            GUI.Box(new Rect(_currentHealthPos.x + HealthBarOffsetX, Screen.height - _currentHealthPos.y + HealthBarOffsetY, _newHealthBarWidth, HealthBarHeight), "", currentStyle);
        }
    }

    private void InitStyles()
    {
        if (_updateHealth)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, _currentHealthColor);

            _updateHealth = false;
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }


    int[] _triangles = { };
    Vector3[] _normals = { };
    Vector2[] _uvs = { };


    public void DrawEdges(Vector3 edgePoint, Mesh mesh,bool flip)
    {
        int[] triangles = new int[6];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        Vector3[] verticies = new Vector3[4];


       
        if (flip)
        {
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 3;



            triangles[3] = 0;
            triangles[4] = 3;
            triangles[5] = 2;
        }
        else
        {
            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;



            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

        }



        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.up;
        }

        verticies[0] = Vector3.zero;
        verticies[1] = edgePoint;
        //verticies[1] = Vector3.forward;
        //verticies[2] = Vector3.right;
        //verticies[3] = Vector3.right + Vector3.forward;

        Vector3 rotatedVector = Vector3.zero;

        float angle = 90;
        if (flip)
            angle = 270;


        angle = angle * Mathf.Deg2Rad;

        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        rotatedVector.x = edgePoint.x * c - edgePoint.z * s;
        rotatedVector.z = edgePoint.x * s + edgePoint.z * c;

        

        verticies[2] = rotatedVector;
       

        //edgePoint = -edgePoint;
         

        verticies[3] = verticies[2]+edgePoint;
       

        uvs[0] = new Vector2(1, 1);
        uvs[1] = new Vector2(1, 0); 
        uvs[2] = new Vector2(0, 1);
        uvs[3] = new Vector2(0, 0);
        

        


        mesh.vertices = verticies;
        

        
            mesh.triangles = triangles;

        
            mesh.normals = normals;

        
            mesh.uv = uvs;


    }

    public void DrawCone(int points, Mesh mesh, bool constant, float attackCharge)
    {
        int zeroes = points ;

        points = points + zeroes;

       // zeroes -= 1;

        if (_triangles.Length != points * 3 + 3)
        {
            _triangles = new int[points * 3 + 3];

            int triangleIndex = 0;

            for (int i = 0; i < points-zeroes; i++)
            {
                if (i != points-zeroes - 1)
                {
                    _triangles[triangleIndex] = i;

                    _triangles[triangleIndex + 2] = i+ zeroes;
                    _triangles[triangleIndex + 1] = i + 1+ zeroes;
                }

                triangleIndex += 3;
            }

            _triangles[triangleIndex] = zeroes;

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
         _uvs = new Vector2[points];


        //vertices[0] = Vector3.zero;
        //_uvs[0] = new Vector2(0, 0);
        

        

        Vector3 vectorToRotate;
        if (constant)
            vectorToRotate = Vector3.forward * stats.AttackRange;
        else
            vectorToRotate = Vector3.forward * (stats.AttackRange * AttackCurve.Evaluate(attackCharge / stats.AttackChargeUpTime));

        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points-2 -zeroes);

        float stepSize2 = 1f / ((float)points - 1 - zeroes);

        int step = 0;
        for (int i = 0; i < zeroes; i++)
        {
            //print(i + " step " + (stepSize2 *  i));
            vertices[i] = Vector3.zero;
            _uvs[i] = new Vector2(stepSize2 * (i), 0);
        }


        for (int i = 1+zeroes; i < points; i++)
        {
            float angle = Mathf.Lerp(-stats.AttackAngle, stats.AttackAngle, step * stepSize);

            angle = angle * Mathf.Deg2Rad;

            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            rotatedVector.x = vectorToRotate.x * c - vectorToRotate.z * s;
            rotatedVector.z = vectorToRotate.x * s + vectorToRotate.z * c;

            

            vertices[i] = rotatedVector;
            _uvs[i] = new Vector2(stepSize*step, 1);
            step++;
        }

        if (constant && stats.AttackAngle<180)
        {
            DrawEdges(vertices[vertices.Length - 1], EdgeMesh1,false);
            //Edge1.SetActive(true);

            DrawEdges(vertices[1+zeroes], EdgeMesh2, true);
            //Edge2.SetActive(true);
        }
        

        mesh.vertices = vertices;
/*
        if (_uvs.Length != vertices.Length)
        {
            _uvs = new Vector2[vertices.Length];


            Bounds bounds = mesh.bounds;

            int i = 0;
            while (i < _uvs.Length)
            {
                _uvs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].z / bounds.size.z);
                i++;
            }

        }*/

        if (mesh.triangles != _triangles)
            mesh.triangles = _triangles;

        if (mesh.normals != _normals)
            mesh.normals = _normals;

        if (mesh.uv != _uvs)
            mesh.uv = _uvs;

        mesh.RecalculateBounds();
    }



    public void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, stats.SpotDistance, LayerMask.GetMask("Player"));
        RaycastHit hit;

        if (potentialTargets.Length > 0)
        {
            if (Physics.Raycast(transform.position, potentialTargets[0].transform.position - transform.position, out hit, stats.SpotDistance))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    DefaultAggroEvent.Raise(this.gameObject);
                    AggroEvent.Raise(this.gameObject);
                    PlayerDetected = true;
                    PlayerTransform = potentialTargets[0].gameObject.transform;

                    IsAlly = true;

                    if (hex != null)
                        hex.SpawnEnemies();


                    DetectAllies();
                }
            }
        }
    }

    void DetectAllies()
    {
        Collider[] potentialAllies = Physics.OverlapSphere(transform.position, stats.AllySpotDistance, LayerMask.GetMask("Enemy"));

        if (potentialAllies.Length > 0)
        {
            for (int i = 0; i < potentialAllies.Length; i++)
            {
                Enemy allyTransform = potentialAllies[i].gameObject.GetComponent<Enemy>();
                if (!allyTransform?.IsAlly ?? false)
                {
                    DefaultAggroEvent.Raise(allyTransform.gameObject);
                    allyTransform.PlayerDetected = true;
                    allyTransform.PlayerTransform = PlayerTransform;
                    allyTransform.IsAlly = true;
                    allyTransform.DetectAllies();
                }
            }
        }
    }



    public Color SetColor(Color color)
    {
        return Color.Lerp(StartColor, color, 0.5f);
    }


    public bool playerInAttackRange()
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < stats.AttackRange / 2;
    }
    public bool playerInRangedAttackRange()
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < stats.AttackRange * 1.1f;
    }


    public bool playerInCustomAttackRange(float length)
    {
        Vector3 adjustedPlayerPos = PlayerTransform.position;

        adjustedPlayerPos.y = transform.position.y;

        return Vector3.Distance(transform.position, adjustedPlayerPos) < stats.AttackRange * length;
    }

    public void HighlightThisBitch()
    {
        var allRender = RenderGraphics.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in allRender)
        {
            render.material = WhiteMaterial;
        }
        StartCoroutine(ResetColor(_enemyHighlightTime));
    }

    IEnumerator ResetColor(float lerptime)
    {
        yield return new WaitForSeconds(lerptime);
        //this.gameObject.GetComponentInChildren<Renderer>().material = _enemyMat;
        var allRender = RenderGraphics.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in allRender)
        {
            render.material = _enemyMat;
        }


    }

}
