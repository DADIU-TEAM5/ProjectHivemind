using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : GameLoop
{
    float _invulnerabilityTimer;

    public GameObjectList EnemyList;


    public float InvulnerabilityTime = 0.3f;

    public FloatVariable MaxHealth;

    Transform _playerParent;

    public GameObjectVariable HexMapParent;

    [SerializeField]
    private FloatVariable CurrentHealth;


    [SerializeField]
    private GameEvent TookDamageEvent;

    public void Start()
    {
        CurrentHealth.Value = MaxHealth.Value;
        _playerParent = transform.parent;
    }

    public void TakeDamage(float damage)
    {

        if (_invulnerabilityTimer <= 0)

        {
            _invulnerabilityTimer = InvulnerabilityTime;
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
        if (_invulnerabilityTimer > 0)
            _invulnerabilityTimer -= Time.deltaTime;
    }

    void CheckIfDead()
    {

        if (CurrentHealth.Value <= 0)
        {
            Destroy(HexMapParent.Value);


            EnemyList.Items = new List<GameObject>();
            OverallSceneWorker.LoadScene("Death Scene");


        }


    }

    public void KnockBackDamage(Vector3 direction, float length,float damage)
    {
        if (_invulnerabilityTimer <= 0)
        {
            _invulnerabilityTimer = InvulnerabilityTime;


            CurrentHealth.Value -= damage;

            if (TookDamageEvent != null)
            {
                if (damage > 0f)
                {
                    TookDamageEvent.Raise(gameObject);
                }
            }

            CheckIfDead();



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

        }

    }
}
