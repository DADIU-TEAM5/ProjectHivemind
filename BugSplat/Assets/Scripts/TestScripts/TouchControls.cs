﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : GameLoop
{
    // Setup ScriptableObjects for holding the PlayerMovementInfo
    public Vector3Variable PlayerSpeedDirectionSO;
    public FloatVariable PlayerMaxSpeedSO;
    public FloatVariable PlayerAccelerationSO;
    public Vector3Variable PlayerVelocitySO;
    public GameEvent DashInitiatedSO;
    public GameEvent AttackInitiatedSO;
    public Transform MainCameraDirection; // Not used yet, but should be used for rotating the coordinate system of the touch input to  match the direction of the player

    // Display sli  rs for altering the speed and acceleration of the Player - This could potentially be moved to an editor window for the designers
    [Header("Player Variables")]
    [Tooltip("Maximum Speed in m/s")]
    public float PlayerMaxSpeed = 2f;
    [Tooltip("Acceleration time in seconds, from 0 to max speed")]
    public float PlayerAcceleration = 1f;

    //Display Sliders for tweaking the amount of touch
    [Header ("Touch Variables")]
    [Tooltip("A percentage of the total screen width for setting the extent of the player's touch on the screen")]
    public float _inputMoveMaxThreshold = 25f;

    [Tooltip("A percentage of the total screen width for setting how far the player must move the finger to make it react as a move and not e.g. a tap")]
    public float _inputMoveMinThreshold = 5f;

    [Tooltip("Time in miliseconds before it is registered as a swipe or tap")]
    public float _inputSwipeTapTime = 200f;

    // Setup the private variables needed for the calculations in the current script
    private Vector3 _inputTouch;
    //private Touch _touch;
    private bool _recordPosition = true;
    public Vector3 _recordedInputPosition;
    public Vector3 _currentInputPosition;
    private bool _inputMoved;
    private float _inputTime;

    // Debug UI stuff
    public GameObject TouchUIDotCurrent;
    public GameObject TouchUIDotRecorded;
    public Transform TouchCanvas;
    private GameObject _uiRecord;
    private GameObject _uiCurrent;

    [Header("Debug stuff")]
    public Text DebugText;


    // Start is called before the first frame update
    void Start()
    {
        
        _inputTouch = new Vector3();

        _inputMoveMaxThreshold = Screen.width * (_inputMoveMaxThreshold / 100);
        _inputMoveMinThreshold = Screen.width * (_inputMoveMinThreshold / 100);

        _inputSwipeTapTime /= 1000;

        PlayerMaxSpeedSO.Value = 0;
        PlayerAccelerationSO.Value = PlayerAcceleration;
        PlayerSpeedDirectionSO.Value = Vector3.zero;
    }

    
   
public override void LoopUpdate(float deltaTime)
    
{
        // Detect Touch
        if (Input.touchCount > 0)
        {
            Touch touch0 = Input.GetTouch(0);

            Vector3 touchPosition = touch0.position;

            if (touch0.phase == TouchPhase.Moved)
            {
                BeginMove(touchPosition);

                if (Input.touchCount > 1)
                {
                    Touch touch1 = Input.GetTouch(1);

                    touchPosition = touch1.position;

                    if (touch1.phase == TouchPhase.Moved)
                    {
                        BeginMove(touchPosition);
                    }

                    if (touch1.phase == TouchPhase.Ended)
                    {
                        EndMove();
                    }
                }
            }

            if (touch0.phase == TouchPhase.Ended)
            {
                EndMove();
            }
        }


        // Allow for usage of the keyboard as controls as well
        if (Input.anyKey)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                PlayerSpeedDirectionSO.Value.x = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                PlayerSpeedDirectionSO.Value.z = Input.GetAxisRaw("Vertical");
            }
        }

        // Simulate touch with mouse, if mouse present
        if (Input.mousePresent)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 inputPosition = Input.mousePosition;

                BeginMove(inputPosition);

            }
            if (Input.GetMouseButtonUp(0))
            {
                EndMove();
            }
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {
 
    }


    private void BeginMove(Vector3 inputPosition)
    {
        ReturnInputPosition(inputPosition); // Record Start Pos
        ReturnInputPosition(inputPosition); // Begin recording Current Pos

        // Check if mouse have moved more than the threshold
        if (Vector3.Distance(_recordedInputPosition, _currentInputPosition) > _inputMoveMinThreshold)
        {
            DebugText.text = "MOVING!";

            _inputMoved = true;

            PlayerMaxSpeedSO.Value = PlayerMaxSpeed;

            Vector3 heading;
            float distance;
            Vector3 direction;

            heading = _currentInputPosition - _recordedInputPosition;

            // Calculates the outer rim position of the "joystick" if the player moves the finger beyond the borders of the designated joystick space
            float x = 0f;
            float y = 0f;
            float a = Mathf.Atan2(heading.y, heading.x);

            float circleCalc = (heading.x * heading.x) + (heading.y * heading.y);

            if (circleCalc > (_inputMoveMaxThreshold * _inputMoveMaxThreshold))
            {
                x = _inputMoveMaxThreshold * Mathf.Cos(a);
                heading.x = x;

                y = _inputMoveMaxThreshold * Mathf.Sin(a);
                heading.y = y;
            }
            else
            {
                x = heading.x;
                y = heading.y;
            }

            // Normalize the direction and speed vector
            distance = heading.magnitude;
            direction = heading.normalized;

            // UI debug stuff
            _uiCurrent.transform.localPosition = new Vector3(x, y);

            // Export direction and speed vector to the PlayerSpeedDirectionSO
            PlayerSpeedDirectionSO.Value.x = direction.x;
            PlayerSpeedDirectionSO.Value.z = direction.y;
        }
    }


    private void ReturnInputPosition(Vector3 touchPos)
    {
        if(_recordPosition)
        {
            _recordedInputPosition = touchPos;
            _inputTime = Time.time;
            _recordPosition = false;

            // UI Debug Stuff
            _uiRecord = Instantiate(TouchUIDotRecorded, TouchCanvas);
            _uiRecord.transform.position = touchPos;

        } else
        {
            _currentInputPosition = touchPos;
        }

        // UI Debug Stuff
        if (_uiCurrent == null)
        {
            _uiCurrent = Instantiate(TouchUIDotCurrent, _uiRecord.transform.position, Quaternion.identity, _uiRecord.transform);
        } else
        {
            _uiCurrent.transform.position = _uiRecord.transform.position;
        }
    }


    private void EndMove()
    {
        // UI Debug Stuff
        Object.Destroy(_uiRecord);
        Object.Destroy(_uiCurrent);
        float endTime = Time.time - _inputTime;

        if (endTime < _inputSwipeTapTime) 
        {
            if (_inputMoved)
            {
                DebugText.text = "DODGED!";
                DashInitiatedSO.Raise();
                PlayerMaxSpeedSO.Value = 0;
            }
            else
            {
                DebugText.text = "ATTACKED!";
                AttackInitiatedSO.Raise();
                PlayerMaxSpeedSO.Value = 0;
            }
        }
        else if(_inputMoved) 
        {
            DebugText.text = "MOVED!";
            PlayerMaxSpeedSO.Value = 0;
        }

        _inputMoved = false;
        _recordPosition = true;
    }
}
