﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerHealth : GameLoop
{
    public GameObjectList EnemyList;
    public GameObjectVariable HexMapParent;

    public BoolVariable IsInvulnerableSO;
    public FloatVariable InvulnerabilityTimerSO;

    public FloatVariable MaxHealth;

    NavMeshAgent _navMeshAgent;

    Transform _playerParent;

    [SerializeField]
    private FloatVariable CurrentHealth;

    [Header("Events")]
    [SerializeField]
    private GameEvent TookDamageEvent;
    [SerializeField]
    private GameEvent PlayerDiedEvent;

    public void Start()
    {
        CurrentHealth.Value = MaxHealth.Value;
        _playerParent = transform.parent;

        _navMeshAgent = transform.parent.GetComponent<NavMeshAgent>();

    }

    public void TakeDamage(float damage)
    {

        if (IsInvulnerableSO.Value != true)

        {
            InvulnerabilityTimerSO.Value = InvulnerabilityTimerSO.InitialValue;
            IsInvulnerableSO.Value = true;
            //print("The player took " + damage);
            CurrentHealth.Value -= damage;

            print("player took " + damage + " current health: " + CurrentHealth.Value);

            if (TookDamageEvent != null)
            {
                TookDamageEvent.Raise(this.gameObject);
            }

            CheckIfDead();
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        
    }
    public override void LoopUpdate(float deltaTime)
    {
        if (InvulnerabilityTimerSO.Value > 0)
        {
            InvulnerabilityTimerSO.Value -= Time.deltaTime;
        } else
        {
            IsInvulnerableSO.Value = false;
        }

    }

    void CheckIfDead()
    {
        if (CurrentHealth.Value <= 0)
        {
            PlayerDiedEvent.Raise();
            Destroy(HexMapParent.Value);

            EnemyList.Items = new List<GameObject>();
            OverallSceneWorker.LoadScene("Death Scene");
        }
    }

    public void KnockBackDamage(Vector3 direction, float length,float damage)
    {
        if (IsInvulnerableSO.Value != true)
        {
            InvulnerabilityTimerSO.Value = InvulnerabilityTimerSO.InitialValue;
            IsInvulnerableSO.Value = true;

            CurrentHealth.Value -= damage;

            if (TookDamageEvent != null)
            {
                if (damage > 0f)
                {
                    TookDamageEvent.Raise(gameObject);
                }
            }

            CheckIfDead();

            _navMeshAgent.Move(direction * length);
            /*
            RaycastHit[] hits = Physics.CapsuleCastAll(_playerParent.position - (Vector3.up * 0.5f), _playerParent.position + (Vector3.up * 0.5f), .1f, direction, (direction * length).magnitude);
            if (hits.Length > 0)
            {
                float shortestDistance = float.MaxValue;
                for (int i = 0; i < hits.Length; i++)
                {
                    float distanceToObject = Vector3.Distance(hits[i].point, _playerParent.position);
                    if (distanceToObject < shortestDistance)
                        shortestDistance = distanceToObject;

                }
                print(direction);

                //print(hit.collider.gameObject.name);
                if (shortestDistance < (direction * length).magnitude)
                {

                }
                else
                {
                    _playerParent.Translate(direction * length);
                }
            }
            else
            {
                _playerParent.Translate(direction * length);
            }
            */

        }

    }
}
