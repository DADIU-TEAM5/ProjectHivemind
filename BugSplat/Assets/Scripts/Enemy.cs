using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameLoop
{
    public float difficultyValue = 1;

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

    private void OnEnable()
    {
        //Debug.Log(name + " spawned");
        EnemyList.Add(gameObject);
    }

    public abstract bool IsVisible();

    public abstract void TakeDamage(float damage);

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
}
