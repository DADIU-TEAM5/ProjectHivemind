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

    private float _currentTime;
    private float _lerpTime = 0f;
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
        if (_dashCooldownActive)
        {
            _lerpTime = Time.time;
            float _diffTime = _lerpTime - _currentTime;
            DashCooldownSO.Value = _diffTime;

            //Debug.Log(_diffTime);

            if (IsDodgingSO.Value == true)
            {
                float curveTime = 0; DashAnimationCurve.Evaluate(_diffTime / DashSpeedSO.Value);

                float curveCounter = 0;

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
                        _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
                    }
                }
                else
                {
                    PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, 1);
                    if (_navMeshAgent.isOnNavMesh)
                    {
                        _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
                    }
                    IsDodgingSO.Value = false;
                }
            }

            if (_diffTime > (DashSpeedSO.Value + DashInvulnerabilityTimeSO.Value))
            {
                IsInvulnerableSO.Value = false;
            }

            if (_diffTime > DashCooldownSO.InitialValue)
            {
                _dashCooldownActive = false;
                DashCooldownSO.Value = DashCooldownSO.InitialValue;
                IsInvulnerableSO.Value = false;
            }
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
