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
    public int HealthBarWidth;
    public int HealthBarHeight;
    public float HealthBarOffsetX;
    public float HealthBarOffsetY;
    private bool _showHealthBar;
    private float _initialHealth;
    private Color _maxHealthColor = new Color(0f, 1f, 0f);
    private Color _lowHealthColor = new Color(1f, 0f, 0f);
    private float _newHealthBarWidth;
    private Vector2 _currentHealthPos;
    private Color _currentHealthColor;


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
        _currentHealthColor = _maxHealthColor;
        _newHealthBarWidth = (HealthBarWidth / _initialHealth) * hitPoints;
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
        else
        {
            _showHealthBar = false;
        }

        if (CurrentEnemySO.Value == null)
        {
            if (CurrentEnemyGraphic.Value != null)
                CurrentEnemyGraphic.Value.SetActive(false);
        }

        if (LockedTarget.Value == gameObject)
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
        }

        if (LockedTarget.Value == null)
        {
            if(TargetGraphic.Value != null)
            TargetGraphic.Value.SetActive(false);
        }
    }

    public void UpdateHealthBar(float hitPoints)
    {
        float percOfInitialHealth = hitPoints / _initialHealth;
        _currentHealthColor = Color.Lerp(_lowHealthColor, _maxHealthColor, percOfInitialHealth);
        _newHealthBarWidth = (HealthBarWidth / _initialHealth) * hitPoints;
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
