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
    public Transform MainCameraDirection; // Not used yet, but should be used for rotating the coordinate system of the touch input to  match the direction of the player

    // Debug UI stuff
    public GameObject TouchUIDotCurrent;
    public GameObject TouchUIDotRecorded;
    public Transform TouchCanvas;
    private GameObject _uiRecord;
    private GameObject _uiCurrent;

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
    private float _touchMoveMaxThreshold = 0.25f;
    private float _touchMoveMinThreshold = 100f;


    // Start is called before the first frame update
    void Start()
    {
        _inputTouch = new Vector3();

        _touchMoveMaxThreshold *= Screen.width;

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
                _touching = true;

                if (_recordMouse)
                {
                    _recordedMousePosition = Input.mousePosition;
                    _recordMouse = false;

                    // UI Debug Stuff
                    _uiRecord = Instantiate(TouchUIDotRecorded, TouchCanvas);
                    _uiRecord.transform.position = _recordedMousePosition;

                    // UI Debug Stuff
                    if (_uiCurrent == null)
                    {
                        _uiCurrent = Instantiate(TouchUIDotCurrent, _uiRecord.transform);
                    }
                }

                // Check if mouse have moved more than the threshold
                if (Vector2.Distance(_recordedMousePosition, _currentMousePosition) > _touchMoveMinThreshold)
                {
                    Vector3 heading;
                    float distance;
                    Vector3 direction;

                    heading = _currentMousePosition - _recordedMousePosition;
                    distance = heading.magnitude;
                    direction = heading / distance;

                    // Export direction and speed to the PlayerSpeedDirectionSO
                    PlayerSpeedDirectionSO.Value.x = direction.x;
                    PlayerSpeedDirectionSO.Value.z = direction.y;

                    // UI Debug Stuff
                    float x = 0f;
                    float y = 0f;
                    float a = Mathf.Atan2(heading.y, heading.x);

                    float circleCalc = (heading.x * heading.x) + (heading.y * heading.y);

                    if (circleCalc > (_touchMoveMaxThreshold * _touchMoveMaxThreshold))
                    {
                        x = _touchMoveMaxThreshold * Mathf.Cos(a);
                        y = _touchMoveMaxThreshold * Mathf.Sin(a);
                    } else
                    {
                        x = heading.x;
                        y = heading.y;
                    }

                    _uiCurrent.transform.localPosition = new Vector3(x, y);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _touching = false;
                _recordMouse = true;

                // UI Debug Stuff
                Object.Destroy(_uiRecord);
                Object.Destroy(_uiCurrent);

                PlayerSpeedDirectionSO.Value = Vector3.zero;
            }
        }
    }

    private void TRASH()
    {

    }
}
