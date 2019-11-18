using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDodgeScript : GameLoop
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


    public GameObject PlayerGraphics;

    public GameEvent DashDoneEvent;

    private float _currentTime;
    private float _lerpTime = 0f;
    private float _diffTime = 0f;
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


    public override void LoopUpdate(float deltaTime)
    {
        _lerpTime = Time.time;
        _diffTime = _lerpTime - _currentTime;

        if (_dashCooldownActive)
        {
            DashCooldownSO.Value = _diffTime;

            if (_diffTime > DashCooldownSO.InitialValue)
            {
                _dashCooldownActive = false;
                DashCooldownSO.Value = DashCooldownSO.InitialValue;
                IsInvulnerableSO.Value = false;
            }
        }

        if (IsDodgingSO.Value == true)
        {
            // DashPower Edit
            float curveTime = 0; DashAnimationCurve.Evaluate(_diffTime / DashSpeedSO.Value);

            float curveCounter = 0;

            // Check if anim curve has been assigned
            for (int i = 0; i < DashAnimationCurve.length; i++)
            {
                curveCounter += DashAnimationCurve[i].value;
            }

            if (curveCounter == 0)
            {
                curveTime = _diffTime / DashSpeedSO.Value;
            }
            else
            {
                curveTime = DashAnimationCurve.Evaluate(_diffTime / DashSpeedSO.Value);
            }
                
            if (_diffTime < DashSpeedSO.Value)
            {
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, curveTime);
                if (_navMeshAgent.isOnNavMesh)
                {
                    if (StaminaIsActive)
                        _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value * DashPower.Value;
                    else _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
                }
            }
            else
            {
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, 1);
                if (_navMeshAgent.isOnNavMesh)
                {
                    if (StaminaIsActive)
                        _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value * DashPower.Value;
                    else _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
                }
                IsDodgingSO.Value = false;
                DashDoneEvent.Raise(PlayerGraphics);
            }
        }

        if (_diffTime > (DashSpeedSO.Value + DashInvulnerabilityTimeSO.Value))
        {
            IsInvulnerableSO.Value = false;
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }


    // Called from PlayerController
    public void PlayerDash()
    {
        if (IsDodgingSO.Value == false && !_dashCooldownActive)
        {
            if (!StaminaIsActive || DashCost.Value <= Stamina.Value)
            {
                _dashDirection = PlayerDirectionSO.Value;
                _currentTime = Time.time;
                _lerpTime = 0f;
                _initialPos = transform.position;
                IsDodgingSO.Value = true;
                _dashCooldownActive = true;
                IsInvulnerableSO.Value = true;
            }
        }
    }
}
