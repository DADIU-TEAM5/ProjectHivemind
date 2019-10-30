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
    public AnimationCurve DodgeAnimationCurve;

    private float _currentTime;
    private float _lerpTime = 0f;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _dashDirection;

    private void Start()
    {
        IsDodging.Value = true;
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }


    public override void LoopUpdate(float deltaTime)
    {
        if (IsDodging.Value == true)
        {
            Vector3 newPosition = _dashDirection;

            if (_lerpTime - _currentTime < DashSpeedSO.Value)
            {
                _lerpTime = Time.time;
                float _diffTime = _lerpTime - _currentTime;
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, _dashDirection * DashLengthSO.Value, _diffTime / DashSpeedSO.Value);

                _navMeshAgent.Move(PlayerVelocitySO.Value * DashLengthSO.Value * Time.deltaTime);
                PlayerDirectionSO.Value = newPosition;
            }
            else
            {
                IsDodging.Value = false;
                _lerpTime = 0f;
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
            IsDodging.Value = true;
        }
    }


    private void DashCurve()
    {


    }
        
}
