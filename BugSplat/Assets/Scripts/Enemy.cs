using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameLoop
{
    public float difficultyValue = 1;
    public AnimationCurve AttackCurve;

    public Material ConeMaterial;


    public GameObjectList EnemyList;
    public GameObjectVariable LockedTarget;
    public GameObjectVariable CurrentEnemySO;
    public GameObjectVariable CurrentEnemyGraphic;
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




    public Renderer Renderer;
    public GameObject Cone;
    public MeshRenderer ConeRenderer;
    public Mesh ConeMesh;
    public GameObject Outline;
    public MeshRenderer OutlineRenderer;
    public Mesh OutlineMesh;

    public float AttackAngle;
    public float AttackChargeUpTime;
    public float SpotDistance;
    public float AttackRange;

    public GameEvent AggroEvent;
    public bool _playerDetected;
    public Transform _playerTransform;
    public bool _isAlly;


    public float _currentHealth;
    public float MaxHealth;



    private void OnEnable()
    {
        //Debug.Log(name + " spawned");
        EnemyList.Add(gameObject);

        


        CreateCone();
        CreateOutline();
        ConeRenderer.material = ConeMaterial;
        OutlineRenderer.material = ConeMaterial;

        OutlineRenderer.material.color = new Color(.2f, .2f, .2f, .1f);

    }

    public abstract bool IsVisible();

    public abstract void TakeDamage(float damage);



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

    }

    private void OnDisable()
    {
        EnemyList.Remove(gameObject);
    }

    public void RemoveFromLockedTargetIfNotVisible()
    {
        if (CurrentEnemySO.Value == gameObject)
        {
            if (CurrentEnemyGraphic.Value == null)
            {
                CurrentEnemyGraphic.Value = GameObject.CreatePrimitive(PrimitiveType.Quad);
                CurrentEnemyGraphic.Value.name = "CurrentEnemy Graphic";
                Destroy(CurrentEnemyGraphic.Value.GetComponent<MeshCollider>());
                CurrentEnemyGraphic.Value.transform.rotation = Quaternion.Euler(90, 0, 0);
                CurrentEnemyGraphic.Value.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                if (CurrentEnemyGraphic.Value.activeSelf == false)
                    CurrentEnemyGraphic.Value.SetActive(true);

                CurrentEnemyGraphic.Value.transform.position = transform.position;
            }

            if (IsVisible() == false)
            {
                CurrentEnemyGraphic.Value.SetActive(false);
                CurrentEnemySO.Value = null;
            }

            _showHealthBar = true;

        }

        if (CurrentEnemySO.Value == null)
        {
            if (CurrentEnemyGraphic.Value != null)
                CurrentEnemyGraphic.Value.SetActive(false);
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
    }

    private void UpdateBarPos()
    {
        _currentHealthPos = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnGUI()
    {
        InitStyles();
        UpdateBarPos();

        if (_showHealthBar)
        {
            GUI.Box(new Rect(_currentHealthPos.x + HealthBarOffsetX, Screen.height - _currentHealthPos.y + HealthBarOffsetY, _newHealthBarWidth, HealthBarHeight), "", currentStyle);
        }
    }

    private void InitStyles()
    {
        currentStyle = new GUIStyle(GUI.skin.box);
        currentStyle.normal.background = MakeTex(2, 2, _currentHealthColor);
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


    public void DrawCone(int points, Mesh mesh, bool constant,float attackCharge)
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
        if (constant)
            vectorToRotate = Vector3.forward * AttackRange;
        else
            vectorToRotate = Vector3.forward * (AttackRange * AttackCurve.Evaluate(attackCharge / AttackChargeUpTime));

        Vector3 rotatedVector = Vector3.zero;

        float stepSize = 1f / ((float)points - 1);
        int step = 0;



        for (int i = 1; i < points; i++)
        {
            float angle = Mathf.Lerp(-AttackAngle, AttackAngle, step * stepSize);



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



    public void DetectThePlayer()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, SpotDistance, LayerMask.GetMask("Player"));
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
        Collider[] potentialAllies = Physics.OverlapSphere(transform.position, SpotDistance, LayerMask.GetMask("Enemy"));

        if (potentialAllies.Length > 0)
        {
            for (int i = 0; i < potentialAllies.Length; i++)
            {
                Enemy allyTransform = potentialAllies[i].gameObject.GetComponent<Enemy>();
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
