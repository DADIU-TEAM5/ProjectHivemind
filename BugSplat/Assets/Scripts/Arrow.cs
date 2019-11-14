using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GameLoop
{
    public GameObjectList EnemyList;
    public GameObjectVariable VictoryObject;

    public BoolVariable NoVisibleEnemies;

    public Enemy[] _enemyScripts;


    public GameObject ArrowPrefab;

    private GameObject _arrow;

    private void Start()
    {
        _enemyScripts = new Enemy[0];

        _arrow = Instantiate(ArrowPrefab);
        _arrow.transform.position = transform.position;
        _arrow.transform.rotation = transform.rotation;

        _arrow.gameObject.SetActive(true);
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public override void LoopUpdate(float deltaTime)
    {
        if (EnemyList.Items.Count != 0)
        {
            if (EnemyList.Items.Count != _enemyScripts.Length)
            {
                _enemyScripts = new Enemy[EnemyList.Items.Count];

                for (int i = 0; i < EnemyList.Items.Count; i++)
                {
                    _enemyScripts[i] = EnemyList.Items[i].GetComponent<Enemy>();
                }
            }

            NoVisibleEnemies.Value = true;

            for (int i = 0; i < _enemyScripts.Length; i++)
            {
                bool enemyIsVisible = _enemyScripts[i].IsVisible();
                if (enemyIsVisible)
                {
                    NoVisibleEnemies.Value = false;
                }

            }

            if (NoVisibleEnemies.Value)
            {

                if (_arrow.activeSelf == false)
                    _arrow.SetActive(true);

                _arrow.transform.position = transform.position;

                float distance = float.MaxValue;
                int index = 0;
                for (int i = 0; i < EnemyList.Items.Count; i++)
                {


                    float distanceToEnemy = Vector3.Distance(transform.position, EnemyList.Items[i].transform.position);

                    if (distanceToEnemy < distance)
                    {
                        index = i;
                        distance = distanceToEnemy;
                    }
                }

                Vector3 enemyPos = EnemyList.Items[index].transform.position;
                enemyPos.y = _arrow.transform.position.y;
                _arrow.transform.LookAt(enemyPos);
                //_arrow.transform.Rotate(0, 20 * Time.deltaTime, 0);
            }
            else
            {
                if (_arrow.activeSelf)
                    _arrow.SetActive(false);
            }

        }
        else
        {
            if (_arrow.activeSelf == false)
                _arrow.SetActive(true);

            _arrow.transform.position = transform.position;

            if (VictoryObject.Value != null)
            {
                Vector3 tempVictory = VictoryObject.Value.transform.position;
                tempVictory.y = transform.position.y;
                _arrow.transform.LookAt(tempVictory);
            }

            //_arrow.transform.Rotate(0, 20 * Time.deltaTime, 0);
        }
    }
}
