﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GameLoop
{

    public Animator Anim;

    public Transform PlayerGraphics;
    public Vector3Variable PlayerDirectionSO;
    public FloatVariable PlayerCurrentSpeedSO;
    public Vector3Variable PlayerVelocitySO;
    public FloatVariable PlayerAccelerationSO;
    public BoolVariable isDodging;

    private float _lerpTime = 0f;
    private Vector3 _velocity;
    private bool _isMoving;
    private float _currentTime;

    Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public override void LoopUpdate(float deltaTime)
    {
        // Lerp from 0 to 1 on Normal movement
        if (PlayerCurrentSpeedSO.Value != 0)
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
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, PlayerDirectionSO.Value, _diffTime / PlayerAccelerationSO.Value);
            }
            else
            {
                PlayerVelocitySO.Value = Vector3.Lerp(Vector3.zero, PlayerDirectionSO.Value, 1);
            }
        }
        else
        {
            PlayerVelocitySO.Value = Vector3.zero;
            _isMoving = false;
        }

        // Move player using translate
        //transform.Translate(PlayerVelocitySO.Value * PlayerMaxSpeedSO.Value * Time.deltaTime);


        // Rotate the graphics along the PlayerSpeedDirection
        if (PlayerDirectionSO.Value != Vector3.zero)
        {
            PlayerGraphics.localRotation = Quaternion.LookRotation(PlayerDirectionSO.Value, Vector3.up);
        }

    }


    private void FixedUpdate()
    {
        Anim.SetBool("Running", PlayerCurrentSpeedSO.Value != 0);
        _rigidbody.MovePosition(transform.position+(PlayerVelocitySO.Value * PlayerCurrentSpeedSO.Value * Time.deltaTime));
        _rigidbody.velocity = Vector3.zero;
    }
    public override void LoopLateUpdate(float deltaTime)
    {

    }
}
