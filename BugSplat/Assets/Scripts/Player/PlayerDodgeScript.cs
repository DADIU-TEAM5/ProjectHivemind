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

    public float PlayerCollideRadius = 1f;
    public float PlayerCollideEndRadius = 1f;

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

    private List<Enemy> CollidedEnemies = new List<Enemy>();


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

        CollidedEnemies.Clear();

        for (var time = 0f; time <= DashSpeedSO.Value; time += Time.deltaTime) {
            var curveTime = DashAnimationCurve?.Evaluate(time / DashSpeedSO.Value) ?? time / DashSpeedSO.Value; 

            PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, curveTime);

            if (_navMeshAgent.isOnNavMesh)
            {
                if (StaminaIsActive)
                    _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value * DashPower.Value;
                else _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;

                DashCollideEnemies(PlayerCollideRadius);
            }

            yield return null;
        }

        PlayerVelocitySO.Value = _dashDirection * DashLengthSO.Value;
        DashCollideEnemies(PlayerCollideEndRadius);

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

    private void DashCollideEnemies(float radius) {
        var collides = Physics.OverlapSphere(_navMeshAgent.transform.position, radius, (1 << 8), QueryTriggerInteraction.Collide);
        foreach (var potentialEnemy in collides) {
            var enemy = potentialEnemy.GetComponent<Enemy>();
            if (enemy == null || CollidedEnemies.Contains(enemy)) continue;

            CollidedEnemies.Add(enemy);
            DashCollideEvent.Raise(enemy.gameObject);
            enemy.TakeDamage(DashDamage.Value);
        }
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

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
       // Gizmos.DrawSphere(transform.position, PlayerCollideRadius);
    }
}
