using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerHealth : GameLoop
{

    public GameObjectVariable ThePlayer;

    public BoolVariable IsStunned;

    public GameObject PlayerGraphics;
    public EnemyObjectList EnemyList;
    public GameObjectVariable HexMapParent;

    public BoolVariable IsInvulnerableSO;
    public FloatVariable InvulnerabilityTimerSO;
    private bool _invulnerabilityTrigger;

    public FloatVariable MaxHealth;


    float _stunTimer;



    public NavMeshAgent NavAgent;

    Transform _playerParent;

    [SerializeField]
    private FloatVariable CurrentHealth;

    [Header("Events")]
    [SerializeField]
    private GameEvent TookDamageEvent;
    [SerializeField]
    private GameEvent PlayerDiedEvent;



    private void OnEnable()
    {
        ThePlayer.Value = gameObject;
    }
    public void Start()
    {
        if(CurrentHealth.Value<=0)
            CurrentHealth.Value = MaxHealth.Value;
        _playerParent = transform.parent;

        _invulnerabilityTrigger = false;

        if (NavAgent == null)
            NavAgent = GetComponent<NavMeshAgent>();
       
            
    }

    public void TakeDamage(float damage)
    {

        if (InvulnerabilityTimerSO.Value <= 0 && IsInvulnerableSO.Value != true)
        {
            InvulnerabilityTimerSO.Value = InvulnerabilityTimerSO.InitialValue;
            IsInvulnerableSO.Value = true;
            print("The player took " + damage);
            CurrentHealth.Value -= damage;

            print("player took " + damage + " current health: " + CurrentHealth.Value);

            if (TookDamageEvent != null)
            {
                TookDamageEvent.Raise(PlayerGraphics);
            }

            CheckIfDead();
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }
    public override void LoopUpdate(float deltaTime)
    {
        // Health sanitizing
        if (CurrentHealth.Value > MaxHealth.Value) {
            CurrentHealth.Value = MaxHealth.Value;
        }

        if (_stunTimer > 0)
        {
            _stunTimer -= deltaTime;
            IsStunned.Value = true;
        }
        else
        {
            IsStunned.Value = false;
        }


        if (InvulnerabilityTimerSO.Value > 0)
        {
            InvulnerabilityTimerSO.Value -= Time.deltaTime;
            _invulnerabilityTrigger = true;

        }
        else if (_invulnerabilityTrigger == true)
        {
            IsInvulnerableSO.Value = false;
            _invulnerabilityTrigger = false;
        }
    }

    void CheckIfDead()
    {
        if (CurrentHealth.Value <= 0)
        {
            PlayerDiedEvent.Raise(PlayerGraphics);
           // Destroy(HexMapParent.Value);

            EnemyList.Items = new List<Enemy>();
            Invoke("LoadDeadScene", 2f);
        }
    }

    void LoadDeadScene()
    {
        OverallSceneWorker.LoadScene("Death Scene");
    }

    public void KnockBackDamage(Vector3 direction, float length, float damage)
    {
        //Debug.Log("KnockBackDamage " + direction + ", " + length);
        if (InvulnerabilityTimerSO.Value < 0 && IsInvulnerableSO.Value != true)
        {
            InvulnerabilityTimerSO.Value = InvulnerabilityTimerSO.InitialValue;
            IsInvulnerableSO.Value = true;

            CurrentHealth.Value -= damage;

            if (TookDamageEvent != null)
            {
                if (damage > 0f)
                {
                    TookDamageEvent.Raise(PlayerGraphics);
                }
            }

            CheckIfDead();


            NavAgent.Move(direction * length);


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


    public void StunPlayer(float time)
    {

        _stunTimer = time;
    }
}
