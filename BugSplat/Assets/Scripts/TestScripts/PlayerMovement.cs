using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GameLoop
{

    public Transform PlayerGraphics;
    public Vector3Variable PlayerSpeedDirectionSO;
    public FloatVariable PlayerMaxSpeedSO;
    public Vector3Variable PlayerVelocitySO;
    public FloatVariable PlayerAccelerationSO;
    public BoolVariable isDodging;

    private float _lerpTime = 0f;
    private Vector3 _velocity;
    private bool _isMoving;
    private float _currentTime;

    public override void LoopUpdate(float deltaTime)
    {
        // Lerp from 0 to 1 on Normal movement
        if (PlayerMaxSpeedSO.Value != 0)
        {
            if (_isMoving == false)
            {
                _currentTime = Time.time;
                _lerpTime = 0;
                _isMoving = true;
            }

            if (_lerpTime - _currentTime < PlayerAccelerationSO.Value)
            {
                _lerpTime = Time.time;
                float _diffTime = _lerpTime - _currentTime;
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, PlayerSpeedDirectionSO.Value, _diffTime / PlayerAccelerationSO.Value);
            }
            else
            {
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, PlayerSpeedDirectionSO.Value, 1);
            }
        }
        else
        {
            PlayerVelocitySO.Value = Vector3.zero;
            _isMoving = false;
        }

        // Move player using translate
        transform.Translate(PlayerVelocitySO.Value * PlayerMaxSpeedSO.Value * Time.deltaTime);

        // Rotate the graphics along the PlayerSpeedDirection
        if (PlayerSpeedDirectionSO.Value != Vector3.zero)
        {
            PlayerGraphics.localRotation = Quaternion.LookRotation(PlayerSpeedDirectionSO.Value, Vector3.up);
        }
        
    }

        public override void LoopLateUpdate(float deltaTime)
    {

    }
}
