using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDodgeScript : GameLoop
{
    public BoolVariable IsDodging;

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
        IsDodging.Value = false;
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

            Debug.Log(_diffTime);

            if (IsDodging.Value == true)
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
                    IsDodging.Value = false;
                }
            }

            if (_diffTime > DashCooldownSO.InitialValue)
            {
                _dashCooldownActive = false;
                DashCooldownSO.Value = DashCooldownSO.InitialValue;
            }
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }


    // Called from PlayerController
    public void PlayerDash()
    {
        if (IsDodging.Value == false && !_dashCooldownActive)
        {
            _dashDirection = PlayerDirectionSO.Value;
            _currentTime = Time.time;
            _lerpTime = 0f;
            _initialPos = transform.position;
            IsDodging.Value = true;
            _dashCooldownActive = true;
        }
    }


    private void DashCurve()
    {


    }
        
}
