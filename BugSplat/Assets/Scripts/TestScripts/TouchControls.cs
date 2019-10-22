using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    public Vector3Variable PlayerSpeedDirection;
    public Transform TouchUIDot;

    private Vector3 _inputTouch;
    private Touch _touch;
    private bool _touching = false;
    private bool _recordMouse = true;
    private Vector2 _recordedMousePosition;
    private Vector2 _currentMousePosition;
    private float _TouchMoveMaxThreshold = 400f;
    private float _TouchMoveMinThreshold = 100f;


    
    // Start is called before the first frame update
    void Start()
    {
        _inputTouch = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {

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

        // Simulate touch with mouse, if mouse present
        if(Input.mousePresent)
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
                    float x = _currentMousePosition.x - _recordedMousePosition.x;

                    PlayerSpeedDirection.Value.x = x;

                    Debug.Log(PlayerSpeedDirection.Value.x);

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("MOUSEUP DISTANCE: " + Vector2.Distance(_recordedMousePosition, _currentMousePosition));
                _touching = false;
                _recordMouse = true;
            }

        }





    }

    private void TRASH()
    {

    }
}
