using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    // Setup ScriptableObjects for holding the PlayerMovementInfo
    public Vector3Variable PlayerSpeedDirectionSO;
    public FloatVariable PlayerMaxSpeedSO;
    public FloatVariable PlayerAccelerationSO;
    public Vector3Variable PlayerVelocitySO;
    public Transform TouchUIDot;
    public Transform MainCameraDirection;

    // Display sliders for altering the speed and acceleration of the Player - This could potentially be moved to an editor window for the designers
    [Tooltip("Maximum Speed in m/s")]
    public float PlayerMaxSpeed = 1f;
    [Tooltip("Acceleration time in seconds")]
    public float PlayerAcceleration = 1f;

    // Setup the private variables needed for the calculations in the current script
    private Vector3 _inputTouch;
    private Touch _touch;
    private bool _touching = false;
    private bool _recordMouse = true;
    private Vector3 _recordedMousePosition;
    private Vector3 _currentMousePosition;
    //private float _TouchMoveMaxThreshold = 0.75f;
    private float _TouchMoveMinThreshold = 100f;

    
    // Start is called before the first frame update
    void Start()
    {
        _inputTouch = new Vector3();

        PlayerMaxSpeedSO.Value = PlayerMaxSpeed;
        PlayerAccelerationSO.Value = PlayerAcceleration;
    }

    // Update is called once per frame
    void Update()
    {

        /*
        // Detect Touch
        if(Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            _touching = true;

            if (_touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = _touch.deltaPosition;
            }
        }
        */

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
        else
        {
            PlayerSpeedDirectionSO.Value = Vector3.zero;
        }

        // Simulate touch with mouse, if mouse present
        if (Input.mousePresent)
        {
            _currentMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0))
            {
                Debug.Log("MOUSE BUTTON PRESSED!");
                _touching = true;

                if (_recordMouse)
                {
                    _recordedMousePosition = Input.mousePosition;
                    _recordMouse = false;
                }
                
                // Check if mouse have moved more than the threshold
                if (Vector2.Distance(_recordedMousePosition, _currentMousePosition) > _TouchMoveMinThreshold) 
                {
                    Vector3 heading;
                    float distance;
                    Vector3 direction;

                    heading = _currentMousePosition - _recordedMousePosition;
                    distance = heading.magnitude;
                    direction = heading / distance;

                    // Export direction and speed to the PlayerSpeedDirectionSO
                    /*if (_currentMousePosition.x < _recordedMousePosition.x)
                    {
                        PlayerSpeedDirectionSO.Value.x = -direction.x;
                    } else
                    {
                        PlayerSpeedDirectionSO.Value.x = direction.x;
                    }
                    if (_currentMousePosition.y < _recordedMousePosition.y)
                    {
                        PlayerSpeedDirectionSO.Value.z = -direction.y;
                    }
                    else
                    {
                        PlayerSpeedDirectionSO.Value.z = direction.y;
                    }*/

                    PlayerSpeedDirectionSO.Value.x = direction.x;
                    PlayerSpeedDirectionSO.Value.z = direction.y;
                    Debug.Log(direction);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _touching = false;
                _recordMouse = true;

                PlayerSpeedDirectionSO.Value = Vector3.zero;
            }
        }
    }

    private void TRASH()
    {

    }
}
