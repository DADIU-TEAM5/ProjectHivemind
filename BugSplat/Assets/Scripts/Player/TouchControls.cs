using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : GameLoop
{

    public GameObject PlayerGraphics;
    // Setup ScriptableObjects for holding the PlayerMovementInfo
    public Vector3Variable MoveDirectionSO;

    public GameEvent DashInitiatedSO;
    public GameEvent AttackTapSO;
    public FloatVariable InputMoveMinThresholdSO;
    public FloatVariable InputMoveMaxThresholdSO;
    public FloatVariable InputSwipeTapTimeSO;
    public FloatVariable InputSwipeThresholdSO; // Percentage of the screen width
    public BoolVariable PlayerControlOverrideSO;
    public BoolVariable IsStunned;

    public GameObjectVariable LockedTarget;
    public GameObject UICanvas;
    public RectTransform UIMenuButton;
    private bool _uiActivated = false;
    private Vector2 _uiOffset;

    // Setup the private variables needed for the calculations in the current script
    private Vector3 _inputTouch;
    private bool _recordPosition = true;
    private Vector3 _recordedInputPosition;
    public Vector3[] _currentInputPosition;
    private bool _inputMoved;
    private float _inputTime;
    private float _runtimeInputMax;
    private float _runtimeInputMin;
    private int _inputFrames;
    private int _inputSwipeThreshold;

    // Debug UI stuff
    [Header("Debug stuff")]
    public Text DebugText;
    public GameObject TouchUIDotCurrent;
    public GameObject TouchUIDotRecorded;
    public Transform TouchCanvas;

    private GameObject _uiRecord;
    private GameObject _uiCurrent;


    // Start is called before the first frame update
    void Start()
    {
        MoveDirectionSO.Value = Vector3.zero;
        // Disable Multitouch for the phone touch to fix problems with multiple touches. However, multiple touches should be implemented at a later stage.
        Input.multiTouchEnabled = false;

        _inputFrames = Mathf.RoundToInt(InputSwipeTapTimeSO.Value);
        _currentInputPosition = new Vector3[_inputFrames];
        _inputFrames--;

        _inputSwipeThreshold = Mathf.RoundToInt(Screen.width * (InputSwipeThresholdSO.Value / 100));

        _inputTouch = new Vector3();

        _runtimeInputMax = Screen.width * (InputMoveMaxThresholdSO.Value / 100);
        _runtimeInputMin = Screen.width * (InputMoveMinThresholdSO.Value / 100);
        
        if (PlayerControlOverrideSO.Value == false)
        {
        }

        if (UICanvas != null)
        {
            _uiActivated = UICanvas.activeSelf;

            _uiOffset = new Vector2(UIMenuButton.offsetMax.x, UIMenuButton.offsetMax.y);
        } else
        {
            _uiActivated = false;
            _uiOffset = new Vector2(0, 0);
        }
    }


    public override void LoopUpdate(float deltaTime)
    {
        if (PlayerControlOverrideSO.Value == false)
        {
            if (IsStunned.Value == false)
            {
                // Detect Touch
                if (Input.touchCount > 0)
                {
                    Touch touch0 = Input.GetTouch(0);

                    if (touch0.fingerId == 0)
                    {
                        Vector3 touchPosition = touch0.position;

                        if (_uiActivated == false)
                        {
                            if (touchPosition.x > _uiOffset.x || touchPosition.y > _uiOffset.y)
                            {
                                switch (touch0.phase)
                                {
                                    case TouchPhase.Moved:
                                        BeginMove(touchPosition);
                                        break;
                                    case TouchPhase.Ended:
                                        EndMove(touchPosition);
                                        break;
                                }
                            }
                        }
                    }
                }


                // Simulate touch with mouse, if mouse present
                if (Input.mousePresent)
                {
                    Vector3 inputPosition = Input.mousePosition;

                    if (Input.GetMouseButton(0))
                    {
                        if (_uiActivated == false)
                        {
                            if (inputPosition.x > _uiOffset.x || inputPosition.y > _uiOffset.y)
                            {
                                BeginMove(inputPosition);
                            }
                        }

                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (_uiActivated == false)
                        {
                            if (inputPosition.x > _uiOffset.x || inputPosition.y > _uiOffset.y)
                            {
                                EndMove(Input.mousePosition);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            ClearInputUI();
        }
    }


    public override void LoopLateUpdate(float deltaTime)
    {
 
    }

    private void PopulatePositionIndex(Vector3[] inputArray)
    {
        for (int i = 0; i < _inputFrames; i++)
        {
            inputArray[i] = inputArray[i+1];
        }
    }


    private void BeginMove(Vector3 inputPosition)
    {
        PopulatePositionIndex(_currentInputPosition); // Move old positions one frame backwards in array

        ReturnInputPosition(inputPosition); // Record Start Pos

        ReturnInputPosition(inputPosition); // Recording Current Pos

        //DebugText.text = "MOVING!";

        _inputMoved = true;


        if (Vector3.Distance(_currentInputPosition[_inputFrames],_recordedInputPosition) > InputMoveMinThresholdSO.Value)
        {
            Vector3 heading;
            float distance;
            Vector3 direction;

            heading = _currentInputPosition[_inputFrames] - _recordedInputPosition;

            // Calculates the outer rim position of the "joystick" if the player moves the finger beyond the borders of the designated joystick space
            float x = 0f;
            float y = 0f;
            float a = Mathf.Atan2(heading.y, heading.x);

            float circleCalc = (heading.x * heading.x) + (heading.y * heading.y);

            if (circleCalc > (_runtimeInputMax * _runtimeInputMax))
            {
                x = _runtimeInputMax * Mathf.Cos(a);
                heading.x = x;

                y = _runtimeInputMax * Mathf.Sin(a);
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
            MoveDirectionSO.Value.x = direction.x;
            MoveDirectionSO.Value.z = direction.y;
        }
    }


    private void ReturnInputPosition(Vector3 touchPos)
    {
        if(_recordPosition) // Record first position of touch
        {
            _recordedInputPosition = touchPos;
            _inputTime = Time.time;
            _recordPosition = false;

            // UI Debug Stuff
            _uiRecord = Instantiate(TouchUIDotRecorded, TouchCanvas);
            _uiRecord.transform.position = touchPos;
        }
        else
        {
            _currentInputPosition[_inputFrames] = touchPos;
        }

        // UI Debug Stuff
        if (_uiCurrent == null)
        {
            _uiCurrent = Instantiate(TouchUIDotCurrent, _uiRecord.transform.position, Quaternion.identity, _uiRecord.transform);
        }
        else
        {
            _uiCurrent.transform.position = _uiRecord.transform.position;
        }
    }


    private void EndMove(Vector3 touchPosition)
    {
        ClearInputUI();

        // Check if TAP has happened
        if (Vector3.Distance(_currentInputPosition[_inputFrames], _recordedInputPosition) < _runtimeInputMin)
        {
            float endTime = Time.time - _inputTime;

            if (endTime < InputSwipeTapTimeSO.Value)
            {
                //DebugText.text = "ATTACKED!";
                AttackTapSO.Raise(PlayerGraphics);
            }
        }
        // Check if SWIPE has happened
        else if (Vector3.Distance(_currentInputPosition[0], _currentInputPosition[_inputFrames]) > _inputSwipeThreshold)
        {
            //DebugText.text = "DODGED!";
            DashInitiatedSO.Raise(PlayerGraphics);
        }

        // Check if MOVE has happened
        if (_inputMoved)
        {
            //DebugText.text = "MOVED!";
        }

        MoveDirectionSO.Value = Vector3.zero;
        _inputMoved = false;
        _recordPosition = true;
        _inputFrames = Mathf.RoundToInt(InputSwipeTapTimeSO.Value);
        _currentInputPosition = new Vector3[_inputFrames + 1];
    }

    private void ClearInputUI()
    {
        // UI Debug Stuff
        if (_uiRecord != null)
        {
            Destroy(_uiRecord);
        }

        if (_uiCurrent != null)
        {
            Destroy(_uiCurrent);
        }
    }
}
