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

    private void Start()
    {
        IsDodging.Value = false;
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }


    public override void LoopUpdate(float deltaTime)
    {
        if (IsDodging.Value == true)
        {
            _lerpTime = Time.time;
            float _diffTime = _lerpTime - _currentTime;

            float curveTime = DashAnimationCurve.Evaluate(_diffTime/DashSpeedSO.Value);
            Debug.Log(curveTime);

            if (_diffTime < DashCooldownSO.Value)
            {
                if (_diffTime < DashSpeedSO.Value)
                {
                    PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, curveTime);
                }
                else
                {
                    PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, 1);
                }

                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.transform.position = _initialPos + PlayerVelocitySO.Value;
                }
            }
            else 
            {
                IsDodging.Value = false;
            }
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {

    }


    // Called from PlayerController
    public void PlayerDash()
    {
        if (IsDodging.Value == false)
        {
            _dashDirection = PlayerDirectionSO.Value;
            _currentTime = Time.time;
            _lerpTime = 0f;
            _initialPos = transform.position;
            IsDodging.Value = true;
        }
    }


    private void DashCurve()
    {


    }
        
}
