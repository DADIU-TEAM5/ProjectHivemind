using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDodgeScript : MonoBehaviour
{
    public BoolVariable IsDodgingSO;
    public BoolVariable IsInvulnerableSO;
    public FloatVariable DashInvulnerabilityTimeSO;
    public Vector3Variable PlayerDirectionSO;
    public Vector3Variable PlayerVelocitySO;
    public FloatVariable DashSpeedSO;
    public FloatVariable DashLengthSO;
    public FloatVariable DashCooldownSO;
    public AnimationCurve DashAnimationCurve;

    // Stamina stuff ------------
    public bool StaminaIsActive;
    // Used to change Dash legnth Depending on Stamina
    public FloatVariable DashPower;
    public FloatVariable DashCost;
    public FloatVariable Stamina;

    public FloatVariable DashDamage;


    public GameObject PlayerGraphics;

    public GameEvent DashInitiated;
    public GameEvent DashDoneEvent;

    public GameEvent DashCollideEvent;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _dashDirection;
    private Vector3 _initialPos;
    private bool _dashCooldownActive;



    private void Start()
    {
        IsDodgingSO.Value = false;
        IsInvulnerableSO.Value = false;
        _dashCooldownActive = false;
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    private IEnumerator DashCooldown() {
        if (_dashCooldownActive) yield break;

        _dashCooldownActive = true;

        Debug.Log("DASHCOOLDOWN ON: " + _dashCooldownActive);

        for (var time = 0f; time <= DashCooldownSO.InitialValue; time += Time.deltaTime) {
            DashCooldownSO.Value = time;
            yield return null;
        }
        DashCooldownSO.Value = DashCooldownSO.InitialValue;

        _dashCooldownActive = false;
    }

    private IEnumerator Dash() {
        IsDodgingSO.Value = true;
        IsInvulnerableSO.Value = true;

        DashInitiated.Raise(PlayerGraphics);

        _initialPos = transform.position;
        _dashDirection = PlayerDirectionSO.Value;

        for (var time = 0f; time <= DashSpeedSO.Value; time += Time.deltaTime) {
            var curveTime = DashAnimationCurve?.Evaluate(time / DashSpeedSO.Value) ?? time / DashSpeedSO.Value; 

            PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, curveTime);

            if (_navMeshAgent.isOnNavMesh)
            {
                if (StaminaIsActive)
                    _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value * DashPower.Value;
                else _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
            }

            yield return null;
        }

        PlayerVelocitySO.Value = _dashDirection * DashLengthSO.Value;

        if (_navMeshAgent.isOnNavMesh)
        {
            if (StaminaIsActive)
                _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value * DashPower.Value;
            else _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
        }

        DashDoneEvent.Raise(PlayerGraphics);
        IsDodgingSO.Value = false;

        StartCoroutine(DashCooldown());

        yield return new WaitForSeconds(DashInvulnerabilityTimeSO.Value);
        IsInvulnerableSO.Value = false;

    }

    // Called from PlayerController
    public void PlayerDash()
    {
        if (!_dashCooldownActive && !IsDodgingSO.Value) {
            if (!StaminaIsActive || DashCost.Value <= Stamina.Value) {
                StartCoroutine(Dash());
            }
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (IsDodgingSO.Value) {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            DashCollideEvent.Raise(enemy.gameObject);
            enemy.TakeDamage(DashDamage.Value);
        }
    }
}
