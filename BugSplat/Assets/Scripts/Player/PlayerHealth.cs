using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : GameLoop
{
    float _invulnerabilityTimer;


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

            if (TookDamageEvent != null)
            {
                if (damage > 0f)
                {
                    TookDamageEvent.Raise();
                }
            }

            if (CurrentHealth.Value <= 0)
            {
                Destroy(HexMapParent.Value);

                OverallSceneWorker.LoadScene("Death Scene");


            }
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
                    TookDamageEvent.Raise();
                }
            }

            if (CurrentHealth.Value <= 0)
            {
                Destroy(HexMapParent.Value);

                OverallSceneWorker.LoadScene("Death Scene");


            }




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
