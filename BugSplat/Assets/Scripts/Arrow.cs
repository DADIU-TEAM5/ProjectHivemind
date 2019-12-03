using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arrow : GameLoop
{
    public EnemyObjectList EnemyList;
    public GameObjectVariable VictoryObject;

    public float DisableDistance = 5f;

    public BoolVariable NoVisibleEnemies;

    public GameObject ArrowPrefab;

    private GameObject _arrow;

    NavMeshAgent NavAgent;


    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();

        _arrow = Instantiate(ArrowPrefab, transform);

        _arrow.gameObject.SetActive(true);
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public override void LoopUpdate(float deltaTime)
    {
        if (EnemyList.Items.Count != 0)
        {
            NoVisibleEnemies.Value = true;

            for (int i = 0; i < EnemyList.Items.Count; i++)
            {
                if (EnemyList.Items[i].IsVisible()) {
                    NoVisibleEnemies.Value = false;
                    break;
                }
            }

            // If there is no visible enemies, definitely show arrow
            if (NoVisibleEnemies.Value)
            {
                if (_arrow.activeSelf == false)
                    _arrow.SetActive(true);
                
                var closestEnemy = FindClosestEnemy();

                var enemy = EnemyList.Items[closestEnemy.Item1];
                RotateArrow(enemy.gameObject);
            }
            else
            {
                var closestEnemy = FindClosestEnemy();

                if (closestEnemy.Item2 > DisableDistance) {
                    _arrow.SetActive(true);

                    var enemy = EnemyList.Items[closestEnemy.Item1];
                    RotateArrow(enemy.gameObject);
                } else {
                    _arrow.SetActive(false);
                }
            }

        }
        else
        {
            if (VictoryObject.Value != null)
            {
                _arrow.SetActive(true);

                RotateArrow(VictoryObject.Value);
            } else {
                _arrow.SetActive(false);
            }
        }
    }

    private Tuple<int, float> FindClosestEnemy() {
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

        return new Tuple<int, float>(index, distance);
    }

    private void RotateArrow(GameObject enemy) {
        var targetPos = enemy.transform.position;
        


        NavMeshPath path = new NavMeshPath();

        if(NavAgent.isOnNavMesh)
        NavAgent.CalculatePath(enemy.transform.position, path);

        if (path.corners.Length > 1)
            targetPos = path.corners[1];
        

        targetPos.y = _arrow.transform.position.y;

        _arrow.transform.LookAt(targetPos);

    }
}
